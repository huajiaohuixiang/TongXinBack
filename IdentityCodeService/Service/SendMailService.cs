using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityCode
{
    public  interface SendMailService
    {

        string sendIdentityCode(string receivername, string receiveraddress, string code);
        void SendSMTPEMail(string strSmtpServer, string strFrom, string strFromCode, string strto, string strSubject, string strBody);
    }
}
