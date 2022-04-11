using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USBModel;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace USBAdminWebMVC
{
    public class EmailHelp
    {
        private string _smtp;
        private int _port;
        private string _adminAddr;
        private List<string> _forwardEmailAdressList;
        private string _notifyUrl;
        private string _adminName;
        private Tbl_EmailSetting _emailSetting;

        private readonly HttpContext _httpContext;
        private readonly USBDBHelp _usbDb;

        public EmailHelp(IHttpContextAccessor httpContextAccessor, USBDBHelp databaseHelp)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _usbDb = databaseHelp;
        }

        #region private void SetEmailSet()
        private void SetEmailSet()
        {
            try
            {
                _emailSetting = _usbDb.EmailSetting_Get().Result;
                _smtp = _emailSetting.Smtp;
                _port = _emailSetting.Port;
                _adminAddr = _emailSetting.AdminEmailAddr;
                _forwardEmailAdressList = _emailSetting.GetForwardEmailAddrList();
                _notifyUrl = _emailSetting.NotifyUrl;
                _adminName = _emailSetting.AdminName;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region private async Task SendEmail(MimeMessage mimeMessage)
        private async Task SendEmail(MimeMessage mimeMessage)
        {
            try
            {
                using SmtpClient client = new SmtpClient();
                await client.ConnectAsync(_smtp, _port, false);
                //client.Authenticate("", "");

                client.Send(mimeMessage);
                client.Disconnect(true);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + private async Task SendEmail_ByAdmin(string subject, string body, string toAddress)
        private async Task SendEmail_ByAdmin(string subject, string body, string toAddress)
        {
            try
            {
                SetEmailSet();

                using MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress(_adminName, _adminAddr));
                message.To.Add(new MailboxAddress(toAddress, toAddress));
                message.Subject = subject;
                BodyBuilder bodyBuilder = new BodyBuilder { TextBody = body };
                message.Body = bodyBuilder.ToMessageBody();

                await SendEmail(message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task SendEmail_ByAdmin(string subject, string body, List<string> toAddressList)
        {
            try
            {
                SetEmailSet();

                using MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress(_adminName, _adminAddr));

                if (toAddressList.Any())
                {
                    foreach (var to in toAddressList)
                    {
                        message.To.Add(new MailboxAddress(to, to));
                    }
                }
                
                message.Subject = subject;
                BodyBuilder bodyBuilder = new BodyBuilder { TextBody = body };
                message.Body = bodyBuilder.ToMessageBody();

                await SendEmail(message);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task SendToUser_UsbRequest_SubmitForm_Received(Tbl_UsbRequest usb, Tbl_PerComputer com)
        public async Task SendToUser_UsbRequest_SubmitForm_Received(Tbl_UsbRequest usb, Tbl_ComputerInfo com)
        {
            try
            {
                var subject = "USB registration request notification";

                StringBuilder body = new StringBuilder();
                body.AppendLine("USB Detail:")
                    .AppendLine("Manufacturer: " + usb.Manufacturer)
                    .AppendLine("Product: " + usb.Product)
                    .AppendLine("SerialNumber: " + usb.SerialNumber)
                    .AppendLine("IP: " + com.IPAddress)
                    .AppendLine("ComputerName: " + com.HostName)
                    .AppendLine()
                    .AppendLine("Request reason:")
                    .AppendLine(usb.RequestReason)
                    .AppendLine();

                // send to user
                await SendEmail_ByAdmin(subject, body.ToString(), usb.RequestUserEmail);

                // send to forward
                body.AppendLine("Approve or Reject Url: " + _notifyUrl + "/" + usb.Id);

                await SendEmail_ByAdmin(subject, body.ToString(), _forwardEmailAdressList);
            }
            catch (Exception ex)
            {
                throw new EmailException(ex.Message);
            }
        }
        #endregion

        #region + public async Task SendToUser_UsbReuqest_Result(Tbl_UsbRequest usb)
        public async Task SendToUser_UsbReuqest_Result(Tbl_UsbRequest usb)
        {
            try
            {
                var subject = "USB registration request result";

                StringBuilder body = new StringBuilder();
                body.AppendLine("USB Detail:")
                    .AppendLine("Manufacturer: " + usb.Manufacturer)
                    .AppendLine("Product: " + usb.Product)
                    .AppendLine("SerialNumber: " + usb.SerialNumber)
                    .AppendLine()
                    .AppendLine("Request reason:")
                    .AppendLine(usb.RequestReason)
                    .AppendLine()
                    .AppendLine("-----------")
                    .AppendLine("Request result:");

                if (usb.RequestState == UsbRequestStateType.Approve)
                {
                    body.AppendLine(_emailSetting.ApproveText);
                }

                if (usb.RequestState == UsbRequestStateType.Reject)
                {
                    body.AppendLine(usb.RejectReason);
                }

                // send to user
                await SendEmail_ByAdmin(subject, body.ToString(), usb.RequestUserEmail);
            }
            catch (Exception ex)
            {
                throw new EmailException(ex.Message);
            }
        }
        #endregion
    }
}
