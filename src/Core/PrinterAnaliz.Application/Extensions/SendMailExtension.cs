using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text; 


namespace PrinterAnaliz.Application.Extensions
{
    public static class  SendMailExtension
    {
       
        public  static bool SendEmail(string From, List<string> To, string Subject, string Body, string Username, string Password,string Host = "",  bool EnableSsl = false,  int SmtpPort = 587)
        {

            try
            {
                InternetAddressList toList = new InternetAddressList();
                InternetAddressList ccList = new InternetAddressList();

                var startTsl = EnableSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.None;

                if (To!=null)
                    foreach (var itemTo in To) { toList.Add(MailboxAddress.Parse(itemTo)); }
             


                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(From));
                if (toList != null)
                    email.To.AddRange(toList);
 

                email.Subject = Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = Body };

                using var smtp = new SmtpClient();

                smtp.Connect(Host, SmtpPort, startTsl);
                smtp.Authenticate(Username, Password);
                smtp.Send(email);
                smtp.Disconnect(true);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
