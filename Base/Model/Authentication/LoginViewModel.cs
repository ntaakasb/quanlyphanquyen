namespace Base.Model.Authentication
{
    public class LoginViewModel
    {
        public string UserAccount { get; set; }
        public string UserSecret { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }

        public string IdToken { get; set; }
    }
}