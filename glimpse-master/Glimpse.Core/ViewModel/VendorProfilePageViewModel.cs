using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Core.ViewModels;
using Glimpse.Core.Contracts.Services;
using Glimpse.Core.Model;
using Newtonsoft.Json;
using Glimpse.Core.Services.General;

namespace Glimpse.Core.ViewModel
{
    public class VendorProfilePageViewModel : BaseViewModel
    {

        private IPromotionDataService _promotionDataService;
        private IVendorDataService _vendorDataService;
        public List<Promotion> _myPromotionList;
        private Vendor vendor;

        public VendorProfilePageViewModel(IMvxMessenger messenger, IPromotionDataService promotionDataService, IVendorDataService vendorDataService) : base(messenger)
        {
            _vendorDataService = vendorDataService;
            _promotionDataService = promotionDataService;
            getPromotions.Execute();
        }

        public List<Promotion> PromotionList
        {
            get { return _myPromotionList; }
            set
            {
                _myPromotionList = value;
                RaisePropertyChanged(() => PromotionList);
            }
        }

        private string _companyName;
        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                _companyName = value;
                RaisePropertyChanged(() => CompanyName);

            }
        }

        private async Task<List<Promotion>> GetPromotionsForLoggedInVendor()
        {
            List<Promotion> promotionForVendor = await _promotionDataService.GetPromotions();

            if (!string.IsNullOrEmpty(Settings.Email))
            {
                vendor = await _vendorDataService.SearchVendorByEmail(Settings.Email);
            }

            CompanyName = vendor.CompanyName;
            return promotionForVendor.Where(c => c.VendorId == vendor.VendorId).ToList();
        }

        /// <summary>
        /// Init method so that list is refreshed when show view model is called
        /// The parameter is required to be able to get this method called since none exist with empty argument...
        /// </summary>
        /// <param name="index"></param>
        public async void Init(int index)
        {
            PromotionList = await GetPromotionsForLoggedInVendor();
        }

        public MvxCommand getPromotions
        {
            get
            {
                return new MvxCommand( async() =>
                {
                    PromotionList = await GetPromotionsForLoggedInVendor();
                });
            }
        }
        
        public IMvxCommand ShowCreatePromotionView { get { return ShowCommand<CreatePromotionViewModel>(); } }

        private MvxCommand ShowCommand<TViewModel>()
            where TViewModel : IMvxViewModel
        {
            return new MvxCommand(() => ShowViewModel<TViewModel>());
        }
    }
}
