namespace Helpers
{
    public class SmtpMailConfig
    {
        public string Account { get; set; }

        public string Password { get; set; }

        public int Port { get; set; }

        public string Server { get; set; }

        public string FromAddress { get; set; }

        public string FromDisplayName { get; set; }

        public bool EnableSsl { get; set; }

        public string TextPart { get; set; }
    }
}