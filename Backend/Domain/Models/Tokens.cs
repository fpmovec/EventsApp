namespace Domain.Models
{
    public class RefreshToken
    {
       public int Id {  get; set; }
       public string UserId { get; set; }
       public string Token { get; set; }
       public string JwtId { get; set; }
       public bool IsUsed { get; set; }
       public bool IsRevoked { get; set; }
       public DateTime CreatedAt { get; set; }
       public DateTime ExpiryDate { get; set; }
    }

    public class AuthTokens
    {
        public string MainToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
