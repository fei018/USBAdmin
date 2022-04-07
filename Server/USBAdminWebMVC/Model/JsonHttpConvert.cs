using Newtonsoft.Json;
using System;
using ToolsCommon;
using USBModel;

namespace USBAdminWebMVC
{
    public class JsonHttpConvert
    {
        #region + public static PostComUsbInfo Deserialize_UsbLog(string json)
        public static Tbl_UsbLog Deserialize_UsbLog(string postJson)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    Converters = {
                        new AbstractJsonConverter<Tbl_UsbLog, IUsbLog>()
                    }
                };

                var info = JsonConvert.DeserializeObject<Tbl_UsbLog>(postJson, settings);
                return info;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public static UserComputer Deserialize_PerComputer(string comJson)
        public static Tbl_ComputerInfo Deserialize_ComputerInfo(string comJson)
        {
            try
            {
                var com = JsonConvert.DeserializeObject<Tbl_ComputerInfo>(comJson);
                return com;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region + public static Tbl_UsbRegisterRequest Deserialize_UsbRequest(string postJson)
        public static Tbl_UsbRequest Deserialize_UsbRequest(string postJson)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    Converters = {
                        new AbstractJsonConverter<Tbl_UsbRequest, IUsbRequest>()
                    }
                };

                var post = JsonConvert.DeserializeObject<Tbl_UsbRequest>(postJson, settings);
                return post;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region + public static Tbl_PerPrintJob Deserialize_IPrintJobInfo(string json)
        public static Tbl_PrintJobLog Deserialize_IPrintJobInfo(string json)
        {
            var settings = new JsonSerializerSettings
            {
                Converters = {
                        new AbstractJsonConverter<Tbl_PrintJobLog, IPrintJobLog>()
                    }
            };

            var temp = JsonConvert.DeserializeObject<Tbl_PrintJobLog>(json, settings);
            return temp;
        }
        #endregion
    }
}
