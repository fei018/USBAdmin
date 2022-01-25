using System;

namespace USBAdminWebMVC
{
    public class EmailException : Exception
    {
        public EmailException()
        {

        }

        public EmailException(string error) : base(error)
        {

        }
    }
}
