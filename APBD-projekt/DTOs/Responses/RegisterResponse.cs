namespace APBD_projekt.DTOs.Responses
{
    public class RegisterResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Login { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}