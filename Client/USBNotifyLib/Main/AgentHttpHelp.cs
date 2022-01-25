using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using USBCommon;

namespace USBNotifyLib
{
    public class AgentHttpHelp
    {
        #region + public static HttpClient CreateHttpClient()
        public static HttpClient CreateHttpClient()
        {
            var http = new HttpClient();
            http.Timeout = TimeSpan.FromSeconds(10);
            http.DefaultRequestHeaders.Add("AgentHttpKey", AgentRegistry.AgentHttpKey);
            return http;
        }
        #endregion

        #region + public static AgentHttpResponseResult HttpClient_Get(string url)
        public static AgentHttpResponseResult HttpClient_Get(string url)
        {
            var http = new HttpClient();
            try
            {
                http.Timeout = TimeSpan.FromSeconds(10);
                http.DefaultRequestHeaders.Add("AgentHttpKey", AgentRegistry.AgentHttpKey); // add AgentHttpKey to Header

                var response = http.GetAsync(url).Result;

                if (!response.IsSuccessStatusCode)
                {
                    string error = response.Content.ReadAsStringAsync().Result;
                    throw new Exception($"Http StatusCode: {(int)response.StatusCode}\r\nContent: {error}");
                }

                string resultJson = response.Content.ReadAsStringAsync().Result;

                var agentResult = DeserialAgentResult(resultJson);

                if (!agentResult.Succeed)
                {
                    throw new Exception("AgentHttpResponseResult Error:\r\n" + agentResult.Msg);
                }

                return agentResult;
            }
            catch (AggregateException aex)
            {
                var realException = aex as Exception;  // take first real exception

                while (realException != null && realException.InnerException != null)
                {
                    realException = realException.InnerException;
                }

                throw realException;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                http?.Dispose();
            }
        }
        #endregion

        #region + public static void HttpClient_Post(string url, string postJson)
        public static void HttpClient_Post(string url, string postJson)
        {
            var http = new HttpClient();

            try
            {
                http.Timeout = TimeSpan.FromSeconds(10);
                http.DefaultRequestHeaders.Add("AgentHttpKey", AgentRegistry.AgentHttpKey); // add AgentHttpKey to Header

                StringContent content = new StringContent(postJson, Encoding.UTF8, "application/json");

                var response = http.PostAsync(url, content).Result;

                if (!response.IsSuccessStatusCode)
                {
                    string error = response.Content.ReadAsStringAsync().Result;
                    throw new Exception($"Http StatusCode: {response.StatusCode}, Content: {error}");
                }

                var resultJson = response.Content.ReadAsStringAsync().Result;

                var agentResult = JsonConvert.DeserializeObject<AgentHttpResponseResult>(resultJson);

                if (!agentResult.Succeed)
                {
                    throw new Exception("AgentHttpResponseResult Error:\r\n" + agentResult.Msg);
                }
            }
            catch (AggregateException aex)
            {
                var realException = aex as Exception;  // take first real exception

                while (realException != null && realException.InnerException != null)
                {
                    realException = realException.InnerException;
                }

                throw realException;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                http?.Dispose();
            }
        }
        #endregion

        #region + public static AgentHttpResponseResult DeserialAgentResult(string json)
        public static AgentHttpResponseResult DeserialAgentResult(string json)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    Converters = {
                        new AbstractJsonConverter<AgentSetting, IAgentSetting>(),
                        new AbstractJsonConverter<PrintTemplate, IPrintTemplate>()
                    }
                };

                var agent = JsonConvert.DeserializeObject<AgentHttpResponseResult>(json, settings);
                return agent;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public void GetUsbWhitelist_Http()
        public void GetUsbWhitelist_Http()
        {
            try
            {
                var agentResult = HttpClient_Get(AgentRegistry.UsbWhitelistUrl);

                UsbWhitelistHelp.Set_UsbWhitelist_byHttp(agentResult.UsbFilterData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public void GetAgentSetting_Http()
        public void GetAgentSetting_Http()
        {
            try
            {
                var agentResult = HttpClient_Get(AgentRegistry.AgentSettingUrl);
                var agentSetting = agentResult.AgentSetting;

                AgentRegistry.AgentTimerMinute = agentSetting.AgentTimerMinute;
                AgentRegistry.UsbFilterEnabled = agentSetting.UsbFilterEnabled;
                AgentRegistry.UsbHistoryEnabled = agentSetting.UsbHistoryEnabled;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public void PostPerComputer_Http()
        public void PostPerComputer_Http()
        {
            try
            {
                var com = PerComputerHelp.GetPerComputer() as IPerComputer;
                var comJson = JsonConvert.SerializeObject(com);

                HttpClient_Post(AgentRegistry.PostPerComputerUrl, comJson);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public void PostPerUsbHistory_byDisk_Http(string diskPath)
        public void PostPerUsbHistory_byDisk_Http(string diskPath)
        {
            try
            {
                var comIdentity = PerComputerHelp.GetComputerIdentity();
                var usb = new UsbFilter().Find_UsbDisk_By_DiskPath(diskPath);

                IPerUsbHistory usbHistory = new PerUsbHistory
                {
                    ComputerIdentity = comIdentity,
                    DeviceDescription = usb.DeviceDescription,
                    Manufacturer = usb.Manufacturer,
                    Pid = usb.Pid,
                    Product = usb.Product,
                    SerialNumber = usb.SerialNumber,
                    Vid = usb.Vid,
                    PluginTime = DateTime.Now
                };

                var usbHistoryJosn = JsonConvert.SerializeObject(usbHistory);

                HttpClient_Post(AgentRegistry.PostPerUsbHistoryUrl, usbHistoryJosn);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public void PostUsbRequest_Http(UsbRequest usb)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="usb"></param>
        /// <exception cref="throw"></exception>
        public void PostUsbRequest_Http(UsbRequest post)
        {
            try
            {
                if (post == null)
                {
                    throw new Exception("UsbRequest null reference.");
                }

                var usbJson = JsonConvert.SerializeObject(post);

                HttpClient_Post(AgentRegistry.PostUsbRequestUrl, usbJson);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public PrintTemplate GetPrintTemplate_Http()
        public PrintTemplate GetPrintTemplate_Http()
        {
            try
            {
                var SubnetAddr = PerComputerHelp.GetSubnetAddr();
                string url = AgentRegistry.PrintTemplateUrl + "?SubnetAddr=" + SubnetAddr;

                var agentResult = HttpClient_Get(url);
                var template = agentResult.PrintTemplate as PrintTemplate;
                return template;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
