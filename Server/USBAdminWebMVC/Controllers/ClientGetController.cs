using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ToolsCommon;
using USBModel;

namespace USBAdminWebMVC.Controllers
{
    [AgentHttpKeyFilter]
    public class ClientGetController : Controller
    {
        private readonly USBDBHelp _usbDb;

        public ClientGetController(USBDBHelp usbDb)
        {
            _usbDb = usbDb;
        }

        // Agent Rule
        #region AgentRule(string computerIdentity)
        public async Task<IActionResult> AgentRule(string computerIdentity)
        {
            try
            {
                var rule = await _usbDb.AgentRule_Get_By_ComputerIdentity(computerIdentity) as IAgentRule;
                var result = new AgentHttpResponseResult() { Succeed = true, AgentRule = rule };

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new AgentHttpResponseResult(false, ex.Message));
            }
        }
        #endregion

        // Usb Whitelist

        #region UsbWhitelist()
        public async Task<IActionResult> UsbWhitelist()
        {
            try
            {
                var query = await _usbDb.UsbWhitelist_Get();

                var agent = new AgentHttpResponseResult { Succeed = true, UsbWhitelist = query };

                return Json(agent);
            }
            catch (Exception ex)
            {
                return Json(new AgentHttpResponseResult(false, ex.Message));
            }
        }
        #endregion

        // AgentDownload

        #region AgentDownload()
        public async Task<IActionResult> AgentDownload()
        {
            try
            {
                var fileInfo = new FileInfo(USBAdminHelp.AgentClientApp);
                if (!fileInfo.Exists)
                {
                    throw new Exception("Update File not exist.");
                }

                using FileStream fs = fileInfo.OpenRead();
                byte[] buff = new byte[fileInfo.Length];

                if (await fs.ReadAsync(buff, 0, buff.Length) <= 0)
                {
                    throw new Exception("Update File read buff fail.");
                }

                var result = new AgentHttpResponseResult(true);

                await Task.Run(() =>
                {
                    result.DownloadFileBase64 = Convert.ToBase64String(buff);
                });

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new AgentHttpResponseResult(false, ex.Message));
            }
        }
        #endregion

        // Site Printer

        #region SitePrinterList(string subnetAddr)
        public async Task<IActionResult> SitePrinterList(string subnetAddr)
        {
            try
            {
                var list = await _usbDb.IPPrinterInfo_Get_IList_BySiteSubnetAddr(subnetAddr);
                var result = new AgentHttpResponseResult(true) { SitePrinterList = list };
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new AgentHttpResponseResult(false, ex.Message));
            }
        }
        #endregion

    }
}
