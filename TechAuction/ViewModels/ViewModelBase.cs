using log4net;
using Logging;
using ReactiveUI;

namespace TechAuction.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public static readonly ILog log = Logger.GetLogger(typeof(SetForSaleViewModel));
    }
}
