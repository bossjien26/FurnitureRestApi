namespace Helpers
{
    public class AppSettings
    {
        public DatabaseSettings ConnectionStrings { get; set; }

        public HeaderSetting HeaderSettings { get; set; }

        public JwtSettings JwtSettings { get; set; }
    }
}