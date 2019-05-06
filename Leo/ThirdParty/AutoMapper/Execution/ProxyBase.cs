using System.ComponentModel;

namespace Leo.ThirdParty.AutoMapper.Execution
{
    public abstract class ProxyBase
    {
        protected void NotifyPropertyChanged(PropertyChangedEventHandler handler, string method)
        {
            handler?.Invoke(this, new PropertyChangedEventArgs(method));
        }
    }
}