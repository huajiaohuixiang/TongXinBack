
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IdentityCode
{
    public class SendMailServiceImpl : SendMailService
    {
		public SendMailServiceImpl()
        {

        }
        public string sendIdentityCode(string receivername, string receiveraddress,string code)
        {
            try
            {
				this.SendSMTPEMail("smtp.qq.com", "1751415583@qq.com", "iecrmzvwigvbcfcb", receiveraddress, "注册验证码", receivername+",您好，您收到的验证码为"+code+"，5分钟内有效。若非本人操作，请忽略本邮件！");
				return "success";
			}
            catch (Exception e)
            {
				Console.WriteLine(e.Message);
				Console.WriteLine(e.ToString());
				Console.WriteLine(e.GetType());
				Console.WriteLine(e.StackTrace);
				return "error";
            }
		}


		public void SendSMTPEMail(string strSmtpServer, string strFrom, string strFromCode, string strto, string strSubject, string strBody)
		{

			System.Net.Mail.SmtpClient client = new SmtpClient(strSmtpServer);
			client.Port = 587;
			client.UseDefaultCredentials = false;
			client.Credentials = new System.Net.NetworkCredential(strFrom, strFromCode);
			client.DeliveryMethod = SmtpDeliveryMethod.Network;
			System.Net.Mail.MailMessage message = new MailMessage(strFrom, strto, strSubject, strBody);
			message.BodyEncoding = System.Text.Encoding.UTF8;
			message.IsBodyHtml = true;
			client.Send(message);
		}

		
	}
}
