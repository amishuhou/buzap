using System.ComponentModel;

namespace UsedParts.Localization
{
    public class AppResourcesManager : INotifyPropertyChanged
    {
        private static readonly Resources Items = new Resources();

        public Resources Resources
        {
            get { return Items; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(/*[CallerMemberName]*/ string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Update()
        {
            OnPropertyChanged("Resources");
        }
    }
}
