using System;

namespace UsedParts.AppServices.ViewModels.Base
{
    public class DisposableSource : IDisposable
    {
        private readonly Action _callback;

        public DisposableSource(Action callback)
        {
            _callback = callback;
        }

        #region IDisposable Members

        public void Dispose()
        {
            _callback();
        }

        #endregion
    }

}
