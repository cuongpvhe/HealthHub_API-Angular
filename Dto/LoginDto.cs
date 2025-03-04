namespace HealthHub_API.Dto
{
    public class LoginDto
    {
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string VaiTro { get; set; } = "User";
    }
}
