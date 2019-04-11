using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Core.ViewModels;

namespace Glimpse.Core.ViewModel
{
    public class PromotionDialogViewModel : BaseViewModel
    {
        private IMvxMessenger messenger;
        public PromotionDialogViewModel(IMvxMessenger messenger) : base(messenger)
        {

        }

        public MvxCommand ContinueCommand
        {
            get
            {

                return new MvxCommand(() =>
                {
                 
                });
                
            }
        }

        private string dialogTitle;
        public string PromotionTitle
        {
            get { return dialogTitle; }
            set
            {
                dialogTitle = value;
                RaisePropertyChanged(() => dialogTitle);
            }
        }

        private string dialogDescription;
        public string PromotionDescription
        {
            get { return dialogDescription; }
            set
            {
                dialogDescription = value;
                RaisePropertyChanged(() => dialogDescription);
            }
        }

        private string dialogExpirationDate;
        public string PromotionExpirationDate
        {
            get { return dialogExpirationDate; }
            set
            {
                dialogExpirationDate = value;
                RaisePropertyChanged(() => dialogExpirationDate);
            }
        }

        private string dialogCompanyName;
        public string PromotionCompanyName
        {
            get { return dialogCompanyName; }
            set
            {
                dialogCompanyName = value;
                RaisePropertyChanged(() => dialogCompanyName);
            }
        }

        private byte[] _bytes;
        public byte[] Bytes
        {
            get { return _bytes; }
            set { _bytes = value; RaisePropertyChanged(() => Bytes); }
        }


    }
}
