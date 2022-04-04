using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using ToolsCommon;

namespace AgentLib
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

        #region + public static void HttpClient_Post(string url, object post)
        public static void HttpClient_Post(string url, object postObject)
        {
            if (postObject == null)
            {
                return;
            }

            var postJson = JsonConvert.SerializeObject(postObject);

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
                        new AbstractJsonConverter<AgentRule, IAgentRule>(),
                        new AbstractJsonConverter<IPPrinterInfo, IIPPrinterInfo>()
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

        #region + public void UpdateUSBWhitelist_Http()
        public void UpdateUSBWhitelist_Http()
        {
            try
            {
                var agentResult = HttpClient_Get(AgentRegistry.UsbWhitelistUrl);

                UsbWhitelistHelp.Set_And_Load_UsbWhitelist_byHttp(agentResult.UsbWhitelist);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public void GetAgentRule_Http()
        public void GetAgentRule_Http()
        {
            try
            {
                var comid = PerComputerHelp.GetComputerIdentity();
                var url = AgentRegistry.AgentRuleUrl + "?computerIdentity=" + comid;
                var agentResult = HttpClient_Get(AgentRegistry.AgentSettingUrl);

                var agentRule = agentResult.AgentRule;

                AgentRegistry.AgentTimerMinute = agentRule.AgentTimerMinute;
                AgentRegistry.UsbFilterEnabled = agentRule.UsbFilterEnabled;
                AgentRegistry.PrintJobLogEnabled = agentRule.PrintJobLogEnabled;
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

                HttpClient_Post(AgentRegistry.PostPerComputerUrl, com);
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

                IPerUsbLog usbHistory = new PerUsbLog
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

                HttpClient_Post(AgentRegistry.PostPerUsbLogUrl, usbHistory);
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
                HttpClient_Post(AgentRegistry.PostUsbRequestUrl, post);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public List<IIPPrinterInfo> GetSitePrinterList_Http()
        public List<IPPrinterInfo> GetSitePrinterList_Http()
        {
            try
            {
                var subnetAddr = PerComputerHelp.GetSubnetAddr();
                string url = AgentRegistry.SitePrinterListUrl + "?subnetAddr=" + subnetAddr;

                var agentResult = HttpClient_Get(url);
                var printerList = agentResult.SitePrinterList;

                var list = new List<IPPrinterInfo>();
                foreach (var p in printerList)
                {
                    list.Add(p as IPPrinterInfo);
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public void PostPerPrintJob_Http(PerPrintJob printJob)
        public void PostPerPrintJob_Http(PerPrintJob printJob)
        {
            try
            {
                HttpClient_Post(AgentRegistry.PostPerPrintJobUrl, printJob);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
