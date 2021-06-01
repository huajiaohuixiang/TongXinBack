using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityCode
{
    public interface IdentityCodeService
    {
       
       void sendIdCode(string username, string mailaddress);

        bool verifyCode(string username, string code);

        string generatorCode();
    }
}
