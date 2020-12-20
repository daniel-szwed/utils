using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace ArbreSoft.Utils.Loggers
{
    public class EmailLogger : Logger
    {
        private string host;
        private int port;
        private string username;
        private string password;

        public EmailLogger(IOptions<EmailLoggerConfig> config)
        {
            this.host = config.Value.Host;
            this.port = config.Value.Port;
            this.username = config.Value.Username;
            this.password = config.Value.Password;
        }

        public override bool IsComposite()
        {
            return false;
        }

        public override void Log(string message)
        {
            Send(username, "Log", message, false);
        }

        private void Send(string recipients, string subject, string body, bool htmlBody)
        {
            var credentials = new NetworkCredential(username, password);
            var message = new MailMessage(username, recipients, subject, body);
            message.IsBodyHtml = htmlBody;

            using (var client = new SmtpClient(host, port))
            {
                client.Credentials = credentials;
                client.EnableSsl = true;
                client.Send(message);
            }
        }
    }
}