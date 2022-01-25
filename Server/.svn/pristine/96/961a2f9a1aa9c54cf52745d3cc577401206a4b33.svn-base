using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USBModel;

namespace USBAdminWebMVC
{
    public class EmailHelp
    {
        private readonly string _smtp;
        private readonly int _port;
        private readonly List<string> _fromAdminAdress;
        private readonly string _notifyUrl;
        private readonly string _adminName;
        private readonly Tbl_EmailSetting _emailDb;

        public EmailHelp(USBAdminDatabaseHelp databaseHelp)
        {
            _emailDb = databaseHelp.EmailSetting_Get().Result;

            _smtp = _emailDb.Smtp;
            _port = _emailDb.Port;
            _fromAdminAdress = _emailDb.GetFromAddressList();
            _notifyUrl = _emailDb.NotifyUrl;
            _adminName = _emailDb.FromName;
        }

        #region private async Task SendEmail(string subject, string content, string toAddress = null, List<string> toAddressList = null)
        private async Task SendEmail(string subject, string content, string toAddress = null, List<string> toAddressList = null)
        {
            try
            {
                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress(_adminName, _fromAdminAdress.First());
                message.From.Add(from);

                if (!string.IsNullOrWhiteSpace(toAddress))
                {
                    message.To.Add(new MailboxAddress(toAddress, toAddress));
                }

                if (toAddressList != null && toAddressList.Count > 0)
                {
                    foreach (var to in toAddressList)
                    {
                        message.To.Add(new MailboxAddress(to, to));
                    }
                }

                message.Subject = subject;
                BodyBuilder bodyBuilder = new BodyBuilder { TextBody = content };
                message.Body = bodyBuilder.ToMessageBody();

                using SmtpClient client = new SmtpClient();
                await client.ConnectAsync(_smtp, _port, false);
                //client.Authenticate("", "");

                client.Send(message);
                client.Disconnect(true);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region + public async Task Send_UsbRequest_Notify_Submit_ToUser(Tbl_UsbRequest usb, Tbl_PerComputer com)
        public async Task Send_UsbRequest_Notify_Submit_ToUser(Tbl_UsbRequest usb, Tbl_PerComputer com)
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
                await SendEmail(subject, body.ToString(), usb.RequestUserEmail);

                // send to admin
                body.AppendLine("Approve or Reject Url: " + _notifyUrl + "/" + usb.Id);

                await SendEmail(subject, body.ToString(), null, _fromAdminAdress);
            }
            catch (Exception ex)
            {
                throw new EmailException(ex.Message);
            }    
        }
        #endregion

        #region + public async Task Send_UsbReuqest_Notify_Result_ToUser(Tbl_UsbRequest usb)
        public async Task Send_UsbReuqest_Notify_Result_ToUser(Tbl_UsbRequest usb)
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
                    body.AppendLine(_emailDb.ApproveText);
                }

                if (usb.RequestState == UsbRequestStateType.Reject)
                {
                    body.AppendLine(usb.RejectReason);
                }

                // send to user
                await SendEmail(subject, body.ToString(), usb.RequestUserEmail);
            }
            catch (Exception ex)
            {
                throw new EmailException(ex.Message);
            }
        }
        #endregion
    }
}
