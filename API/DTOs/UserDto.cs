namespace API.DTOs
{
    public class UserDto
    {
        //we used it to return object when the user loged in or register
        public string Username { get; set; }
        public string Token { get; set; }
    }
}