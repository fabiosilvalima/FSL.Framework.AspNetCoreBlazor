namespace FSL.Framework.Web.Authentication.Models
{
    public sealed class CookiesConfiguration
    {
        public int ExpirationInSeconds { get; set; }
        public string CryptographicKey { get; set; }
        public string CookieName { get; set; }
    }
}
