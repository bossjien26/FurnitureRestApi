namespace Helpers
{
    public class AppSettings
    {
        public DatabaseSettings ConnectionStrings { get; set; }

        public HeaderSetting HeaderSettings { get; set; }

        public JwtSettings JwtSettings { get; set; }

        public SmtpMailConfig SmtpMailConfig { get; set; }

        public RedisSettings RedisSettings {get;set;}
    }
}