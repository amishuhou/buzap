using Caliburn.Micro;
using UsedParts.AppServices.ViewModels;
using UsedParts.UI.ViewModels;

namespace UsedParts.UI.Tombstoning
{
    public class RegistrationHandler : ScreenStorageHandler<RegistrationPageViewModel>
    {
        public override void Configure()
        {
            base.Configure();
            Property(vm => vm.Email).InPhoneState();
            Property(vm => vm.Phone).InPhoneState();
            Property(vm => vm.Password).InPhoneState();
            Property(vm => vm.OrganizationType).InPhoneState();
        }
    }
}
