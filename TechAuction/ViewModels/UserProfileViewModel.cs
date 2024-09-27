namespace TechAuction.ViewModels
{
    public class UserProfileViewModel : ViewModelBase
    {
        public HomeViewModel Parent { get; set; }

        public UserProfileViewModel(HomeViewModel parent)
        {
            Parent = parent;
        }
    }
}