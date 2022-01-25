using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using USBCommon;
using Microsoft.AspNetCore.Mvc;

namespace USBAdminWebMVC.Controllers
{
    public class JsonResultHelp
    {
        public static JsonResult Ok(string msg = null)
        {
            return new JsonResult(new { code = 200, msg = msg });
        }

        public static JsonResult Ok(object data, string msg = null)
        {
            return new JsonResult(new { code = 200, data = data, msg = msg });
        }

        public static JsonResult Error(string msg)
        {
            return new JsonResult(new { code = 400, msg = msg });
        }


        public static JsonResult LayuiTableData(int totalCount, object data)
        {
            return new JsonResult(new { code = 0, msg = "", count = totalCount, data = data });
        }

        public static JsonResult LayuiTableData(string error)
        {
            return new JsonResult(new { code = 400, msg = error });
        }
    }
}
