using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;

namespace Glimpse.Core.ViewModel
{
    public class ProfilePageViewModel : BaseViewModel
    {
        public ProfilePageViewModel(IMvxMessenger messenger) : base(messenger)
        {
        }
    }
}
