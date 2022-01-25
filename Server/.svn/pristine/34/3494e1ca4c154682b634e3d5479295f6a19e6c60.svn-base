using Newtonsoft.Json;
using System;
using USBCommon;
using USBModel;

namespace USBAdminWebMVC
{
    public class JsonHttpConvert
    {
        #region + public static PostComUsbInfo Deserialize_PerUsbHistory(string json)
        public static Tbl_PerUsbHistory Deserialize_PerUsbHistory(string postJson)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    Converters = {
                        new AbstractJsonConverter<Tbl_PerUsbHistory, IPerUsbHistory>()
                    }
                };

                var info = JsonConvert.DeserializeObject<Tbl_PerUsbHistory>(postJson, settings);
                return info;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public static UserComputer Deserialize_PerComputer(string comJson)
        public static Tbl_PerComputer Deserialize_PerComputer(string comJson)
        {
            try
            {
                var com = JsonConvert.DeserializeObject<Tbl_PerComputer>(comJson);
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

        #region + public static Tbl_PrintTemplate Deserialize_PrintTemplate(string json)
        public static Tbl_PrintTemplate Deserialize_PrintTemplate(string json)
        {
            var settings = new JsonSerializerSettings
            {
                Converters = {
                        new AbstractJsonConverter<Tbl_PrintTemplate, IPrintTemplate>()
                    }
            };

            var temp = JsonConvert.DeserializeObject<Tbl_PrintTemplate>(json, settings);
            return temp;
        }
        #endregion
    }
}
