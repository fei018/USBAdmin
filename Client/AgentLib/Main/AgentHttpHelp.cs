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
                    throw new Exception($"HttpUrl: {url}\r\nStatusCode: {(int)response.StatusCode}\r\nContent: {error}");
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
                    throw new Exception($"HttpUrl: {url}\r\nStatusCode: {(int)response.StatusCode}\r\nContent: {error}");
                }

                var resultJson = response.Content.ReadAsStringAsync().Result;

                var agentResult = DeserialAgentResult(resultJson);

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
                        new AbstractJsonConverter<AgentConfig, IAgentConfig>(),
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

        #region + public void UpdateUSBWhitelist()
        public void UpdateUSBWhitelist()
        {
            try
            {
                var agentResult = HttpClient_Get(AgentRegistry.UsbWhitelistUrl);

                UsbWhitelist.Set_And_Load_UsbWhitelist_byHttp(agentResult.UsbWhitelist);
            }
            catch (Exception ex)
            {
                throw new Exception("AgentHttpHelp.UpdateUSBWhitelist():\r\n" + ex.Message);
            }
        }
        #endregion

        #region + public void GetAgentRule()
        public void GetAgentRule()
        {
            try
            {
                var comid = ComputerInfoHelp.GetComputerIdentity();
                var url = AgentRegistry.AgentRuleUrl + "?computerIdentity=" + comid;
                var agentResult = HttpClient_Get(url);

                var agentRule = agentResult.AgentRule;

                AgentRegistry.AgentTimerMinute = agentRule.AgentTimerMinute;
                AgentRegistry.UsbFilterEnabled = agentRule.UsbFilterEnabled;
                AgentRegistry.PrintJobLogEnabled = agentRule.PrintJobLogEnabled;
            }
            catch (Exception ex)
            {
                throw new Exception("AgentHttpHelp.GetAgentRule():\r\n" + ex.Message);
            }
        }
        #endregion

        #region + public void PostComputerInfo()
        public void PostComputerInfo()
        {
            try
            {
                var com = ComputerInfoHelp.GetComputerInfo() as IComputerInfo;

                HttpClient_Post(AgentRegistry.PostComputerInfoUrl, com);
            }
            catch (Exception ex)
            {
                throw new Exception("AgentHttpHelp.PostComputerInfo():\r\n" + ex.Message);
            }
        }
        #endregion

        #region + public void PostUsbLog_byDisk(string diskPath)
        public void PostUsbLog_byDisk(string diskPath)
        {
            try
            {
                var comIdentity = ComputerInfoHelp.GetComputerIdentity();
                var usb = new UsbFilter().Find_USBInfo_FromUsbDisk_By_DiskPath(diskPath);

                IUsbLog usbHistory = new UsbLog
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

                HttpClient_Post(AgentRegistry.PostUsbLogUrl, usbHistory);
            }
            catch (Exception ex)
            {
                throw new Exception("AgentHttpHelp.PostUsbLog_byDisk():\r\n" + ex.Message);
            }
        }
        #endregion

        #region + public void PostUsbRequest(UsbRequest usb)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="usb"></param>
        /// <exception cref="throw"></exception>
        public void PostUsbRequest(UsbRequest post)
        {
            try
            {
                HttpClient_Post(AgentRegistry.PostUsbRequestUrl, post);
            }
            catch (Exception ex)
            {
                throw new Exception("AgentHttpHelp.PostUsbRequest():\r\n" + ex.Message);
            }
        }
        #endregion

        #region + public List<IIPPrinterInfo> GetSitePrinterList()
        public List<IPPrinterInfo> GetSitePrinterList()
        {
            try
            {
                var subnetAddr = ComputerInfoHelp.GetSubnetAddr();
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
            catch (Exception ex)
            {
                throw new Exception("AgentHttpHelp.GetSitePrinterList():\r\n" + ex.Message);
            }
        }
        #endregion

        #region + public void PostPrintJobLog(PrintJobLog printJob)
        public void PostPrintJobLog(PrintJobLog printJob)
        {
            try
            {
                HttpClient_Post(AgentRegistry.PostPrintJobLogUrl, printJob);
            }
            catch (Exception ex)
            {
                throw new Exception("AgentHttpHelp.PostPrintJobLog():\r\n" + ex.Message);
            }
        }
        #endregion
    }
}
