namespace HealthHub_API.Dto
{
    public class TokenApiDto
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken {  get; set; } = string.Empty;
        public string  VaiTro { get; set; }
        public DateTime RefreshTokenExpiry { get; set; } 
    }
}
