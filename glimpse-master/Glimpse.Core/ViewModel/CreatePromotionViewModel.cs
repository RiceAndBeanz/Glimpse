using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Core.ViewModels;

namespace Glimpse.Core.ViewModel
{
    public class CreatePromotionViewModel : BaseViewModel
    {
        private IMvxMessenger messenger;
        public CreatePromotionViewModel(IMvxMessenger messenger) : base(messenger)
        {

        }

        public MvxCommand ContinueCommand
        {
            get
            {


                return new MvxCommand(() =>
                {
                    var desc = new Dictionary<string, string> {
                        {"PromotionTitle", PromotionTitle},{"PromotionDescription", PromotionDescription}, {"Footwear", FootwearIsChecked.ToString()},
                        {"Electronics", ElectronicIsChecked.ToString() },  {"Jewellery", JewlleryIsChecked.ToString() }, {"Restaurants", RestaurantsIsChecked.ToString() },
                        {"Services", ServicesIsChecked.ToString() }, {"Apparel", ApparelIsChecked.ToString()}
                    };

                    if (string.IsNullOrEmpty(PromotionTitle) || string.IsNullOrEmpty(PromotionDescription))
                        ErrorMessage = TextSource.GetText("ErrorMissingField");
                    else if (FootwearIsChecked==false && ElectronicIsChecked == false && JewlleryIsChecked == false && RestaurantsIsChecked == false && ServicesIsChecked == false && ApparelIsChecked == false)
                        ErrorMessage = TextSource.GetText("ErrorMissingField");
                    else
                    {
                        ErrorMessage = "";
                        ShowViewModel<CreatePromotionPart2ViewModel>(desc);
                    }
                });
                
            }
        }

        private string _promotionTitle;
        public string PromotionTitle
        {
            get { return _promotionTitle; }
            set
            {
                _promotionTitle = value;
                RaisePropertyChanged(() => PromotionTitle);
            }
        }
        
        private string _promotionDescription;
        public string PromotionDescription
        {
            get { return _promotionDescription; }
            set
            {
                _promotionDescription = value;
                RaisePropertyChanged(() => PromotionDescription);

            }
        }

        private bool _footwearIsChecked = false;
        public bool FootwearIsChecked
        {
            get { return _footwearIsChecked; }
            set
            {
                _footwearIsChecked = value;
                RaisePropertyChanged(() => FootwearIsChecked);
            }
        }
        private bool _electronicIsChecked = false;
        public bool ElectronicIsChecked
        {
            get { return _electronicIsChecked; }
            set
            {
                _electronicIsChecked = value;
                RaisePropertyChanged(() => ElectronicIsChecked);
            }
        }
        private bool _jewelleryIsChecked = false;
        public bool JewlleryIsChecked
        {
            get { return _jewelleryIsChecked; }
            set
            {
                _jewelleryIsChecked = value;
                RaisePropertyChanged(() => JewlleryIsChecked);
            }
        }
        private bool _restaurantsIsChecked = false;
        public bool RestaurantsIsChecked
        {
            get { return _restaurantsIsChecked; }
            set
            {
                _restaurantsIsChecked = value;
                RaisePropertyChanged(() => RestaurantsIsChecked);
            }
        }
        private bool _servicesIsChecked = false;
        public bool ServicesIsChecked
        {
            get { return _servicesIsChecked; }
            set
            {
                _servicesIsChecked = value;
                RaisePropertyChanged(() => ServicesIsChecked);
            }
        }
        private bool _apparelIsChecked = false;
        public bool ApparelIsChecked
        {
            get { return _apparelIsChecked; }
            set
            {
                _apparelIsChecked = value;
                RaisePropertyChanged(() => ApparelIsChecked);
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                RaisePropertyChanged(() => ErrorMessage);
            }
        }

    }
}
