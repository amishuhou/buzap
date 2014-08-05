using Caliburn.Micro;
using UsedParts.AppServices.ViewModels;
using UsedParts.UI.ViewModels;

namespace UsedParts.UI.Tombstoning
{
    public class MakeOfferHandler : ScreenStorageHandler<MakeOfferPageViewModel>
    {
        public override void Configure()
        {
            base.Configure();
            Property(vm => vm.Availability).InPhoneState().RestoreAfterActivation();
            Property(vm => vm.Condition).InPhoneState().RestoreAfterActivation();
            Property(vm => vm.Delivery).InPhoneState().RestoreAfterActivation();
            Property(vm => vm.Warranty).InPhoneState().RestoreAfterActivation();
            Property(vm => vm.Price).InPhoneState();
            Property(vm => vm.Images).InPhoneState();
        }
    }
}
