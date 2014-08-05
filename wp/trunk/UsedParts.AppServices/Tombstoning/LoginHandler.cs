using Caliburn.Micro;
using UsedParts.AppServices.ViewModels;
using UsedParts.UI.ViewModels;

namespace UsedParts.UI.Tombstoning
{
    public class LoginHandler : ScreenStorageHandler<LoginPageViewModel>
    {
        public override void Configure()
        {
            base.Configure();
            Property(vm => vm.Email).InPhoneState();
            Property(vm => vm.Password).InPhoneState();
        }
    }
}
