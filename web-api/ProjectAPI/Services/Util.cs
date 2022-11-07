using System.Net.Mail;
using System.Net;
using ProjectAPI.Models;

namespace ProjectAPI.Services
{
    public class Util
    {
        public static string Now => $"{DateTime.Now.ToUniversalTime():yyyy-MM-ddTHH:mm:ss.000Z}";
        public static string NowPlusYears(int years) => $"{DateTime.Now.AddYears(years).ToUniversalTime():yyyy-MM-ddTHH:mm:ss.000Z}";

        public static void SendActivationConfirmRequestEmailviaGmail(UserAuthModelSignup user)
        {
            String SendMailTo = user.Email; // "sadikcetin@gmail.com";
            String SendMailSubject = "ODU Project Registry signup confirmation";
            String SendMailBody = $"{user.FullName}," +
                $"<br>" +
                $"<br>" +
                $"A user account with your email address has been created at ODU Project Registry. Please use the link below in 10 minutes to activate your account." +
                $"<br>" +
                //$"<a href=\"https://datasense.dev/ProjectForms/signup/confirm/{user.Password}\">ProjectForms/signup/confirm/{user.Password}</a>" +
                $"<a href=\"https://datasense.dev/ProjectForms/register/{user.Password}\">ProjectForms/signup/confirm/{user.Password}</a>" +
                $"<br>" +
                $"<br>" +
                $"Thank you";

            try
            {
                SendEmail(SendMailTo, SendMailSubject, SendMailBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void SendActivationConfirmEmailviaGmail(UserAuthModelSignup user)
        {
            String SendMailTo = user.Email; // "sadikcetin@gmail.com";
            String SendMailSubject = "ODU Project Registry account activated";
            String SendMailBody = $"{user.FullName}," +
                $"<br>" +
                $"<br>" +
                $"A user account with your email address has been activated at ODU Project Registry." +
                $"<br>" +
                $"You will receive communication regarding your project submissions through this email." +
                $"<br>" +
                $"<br>" +
                $"Thank you";

            try
            {
                SendEmail(SendMailTo, SendMailSubject, SendMailBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static void SendEmail(string SendMailTo, string SendMailSubject, string SendMailBody)
        {
            String SendMailFrom = "from-email";
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            MailMessage email = new MailMessage();
            // START
            email.From = new MailAddress(SendMailFrom);
            email.To.Add(SendMailTo);
            email.CC.Add(SendMailFrom);
            email.Subject = SendMailSubject;
            email.Body = SendMailBody;
            email.IsBodyHtml = true;
            //END
            SmtpServer.Timeout = 5000;
            SmtpServer.EnableSsl = true;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new NetworkCredential(SendMailFrom, "network-cred");
            SmtpServer.Send(email);
        }
    }
}
