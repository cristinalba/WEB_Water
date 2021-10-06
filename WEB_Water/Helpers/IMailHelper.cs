using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_Water.Helpers
{
    public interface IMailHelper
    {
        Response SendEmail(string to, string subject, string body);
    }
}
