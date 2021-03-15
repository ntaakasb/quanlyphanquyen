namespace Base.Model.Authentication
{
    public class LoggedUserDetailResponseModel
    {
        public string Id { get; set; }
        public string Avatar { get; set; }
        public string CurrentLane { get; set; }
        public string FullName { get; set; }
        public string DefaultSecurityProfile { get; set; }
        public LoggedUserDetailResponseModel()
        {
            Avatar = "../assets/img/no-avatar.png";
        }
    }
}