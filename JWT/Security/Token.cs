namespace JWT.Security
{
    public class Token
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; } // ilgili token süresi bitiği zaman logine ynlendirmeden devam etirir bu biraz farklı araştır sonra bunu 
        public DateTime Expiration { get; set; }

    }
}
