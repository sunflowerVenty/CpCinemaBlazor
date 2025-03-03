namespace CpCinemaBlazor.ApiRequest.Model
{
    public class User
    {
        public class UserDataShort
        {
            public int id_User { get; set; }
            public bool Admin { get; set; }
            public string Name { get; set; }
            public string AboutMe { get; set; }
            public string Email {  get; set; }
            public string Password { get; set; }
            public bool Edit { get; set; } = false;
        }

        public class UserProd
        {
            public int id_User { get; set; }
            public bool isAdmin { get; set; }
            public string Name { get; set; }
            public string AboutMe { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class AddUser
        {
            public string Name { get; set; }
            public string AboutMe { get; set; }
            public bool Admin { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class AuthUser
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
