using System;
using System.ComponentModel;

namespace UsedParts.AppServices.ViewModels.Base
{
    public interface IBusy : INotifyPropertyChanged
    {
        IDisposable StartJob();
        void EndJob();

        bool IsBusy { get; set; }
    }
}
