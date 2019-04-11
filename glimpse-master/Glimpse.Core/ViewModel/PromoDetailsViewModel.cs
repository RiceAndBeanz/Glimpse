using Glimpse.Core.Contracts.Services;
using Glimpse.Core.Model;
using Glimpse.Core.Services.General;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Core.ViewModel
{
    public class PromoDetailsViewModel : BaseViewModel
    {
        private List<byte[]> _images;
        private readonly IPromotionImageDataService _promotionImageDataService;
        private int _promotionId;
        private string _promotionTitle;
        private string _promotionDuration;
        private string _promotionDescription;
        private byte[] _promotionImage;
        private string _promotionImageURL;
        private string _locationAndDistanceText;
        private string _category;
        private string _startAndEndDate;
        private DateTime _startDate;
        private DateTime _endDate;

        public PromoDetailsViewModel(IMvxMessenger messenger, IPromotionImageDataService promotionImageDataService)
        {
            _promotionImageDataService = promotionImageDataService;
        }

        protected override  void InitFromBundle(IMvxBundle parameters)
        {
            if (parameters.Data.ContainsKey("PromotionID"))
                _promotionId = Convert.ToInt32((parameters.Data["PromotionID"]));

            if (parameters.Data.ContainsKey("ImageURL"))
            {
                _promotionImageURL = (parameters.Data["ImageURL"]);//use blob client im at putting image in the detail
                _promotionImage = BlobClient.BlobClient.GetBlobSynchronous(_promotionImageURL) ;
            }

            if (parameters.Data.ContainsKey("PromotionTitle"))
                _promotionTitle = (parameters.Data["PromotionTitle"]);

            if (parameters.Data.ContainsKey("PromotionDuration"))
                _promotionDuration = (parameters.Data["PromotionDuration"]);

            if (parameters.Data.ContainsKey("PromotionDescription"))
                _promotionDescription = (parameters.Data["PromotionDescription"]);

            if (parameters.Data.ContainsKey("Category"))
                _category = (parameters.Data["Category"]);

            if (parameters.Data.ContainsKey("StartDate"))
                _startDate = new DateTime(Convert.ToInt64(parameters.Data["StartDate"]));

            if (parameters.Data.ContainsKey("EndDate"))
                _endDate = new DateTime(Convert.ToInt64(parameters.Data["EndDate"]));

            LocationAndDistanceText = BuildLocationAndDistanceString();
            StartAndEndDate = BuildStartAndEndDateString();

            base.InitFromBundle(parameters);
        }        

        public string StartAndEndDate
        {
            get { return _startAndEndDate; }
            set
            {
                _startAndEndDate = value;
                RaisePropertyChanged(() => StartAndEndDate);
            }
        }

        public string LocationAndDistanceText
        {
            get { return _locationAndDistanceText; }
            set
            {
                _locationAndDistanceText = value;
                RaisePropertyChanged(() => LocationAndDistanceText);
            }
        }

        public string PromotionImageURL
        {
            get { return _promotionImageURL; }
            set
            {
                _promotionImageURL = value;
                RaisePropertyChanged(() => PromotionImageURL);
            }
        }

        public byte[] PromotionImage
        {
            get { return _promotionImage; }
            set
            {
                _promotionImage = value;
                RaisePropertyChanged(() => PromotionImage);
            }
        }

        public string PromotionTitle
        {
            get
            {
                if (_promotionTitle == null)
                    _promotionTitle = "";

                return _promotionTitle;
            }
        }
        public string PromotionDuration
        {
            get
            {
                if (_promotionDuration == null)
                    _promotionDuration = "";

                return ConvertSecondsToMinutes(_promotionDuration);
            }
        }

        public string PromotionDescription
        {
            get
            {
                if (_promotionDescription == null)
                    _promotionDescription = "";

                return _promotionDescription;
            }
        }

        public List<byte[]> Images
        {
            get
            {
                return _images;
            }
            set
            {
                _images = value;
                RaisePropertyChanged(() => Images);
            }
        }

        public async Task<List<byte[]>> GetImageList()
        {
            //getting images for promotion
            _images = await _promotionImageDataService.GetImageListFromPromotionWithLocationId(_promotionId);
            _images.Insert(0, _promotionImage);
            return _images;
        }

        //builds the string for the text view 
        public string BuildLocationAndDistanceString()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 500);
            string categorie=_category;
            if (Glimpse.Core.Services.General.Settings.Language == "Français")
            {
                switch (_category)
                {
                    case "Footwear":
                        categorie ="Chaussure";
                        break;
                    case "Electronics":
                        categorie = "Électronique";
                        break;
                    case "Jewellery":
                        categorie = "Bijoux";
                        break;
                    case "Restaurants":
                        categorie = "Restaurants";
                        break;
                    case "Services":
                        categorie = "Services";
                        break;
                    case "Apparel":
                        categorie = "Vêtements";
                        break;
                }
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(randomNumber);
            stringBuilder.Append(TextSource.GetText("Bought")).Append(" \u2022 ").Append(categorie).Append(" \u2022 ").Append(ConvertSecondsToMinutes(_promotionDuration));

            return stringBuilder.ToString();            
        }


        private string BuildStartAndEndDateString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(_startDate.ToString("MMM dd, yyyy")).Append(" - ").Append(_endDate.ToString("MMM dd, yyyy"));
            return stringBuilder.ToString();
        }

        public async Task<byte[]> GetCoverImage()
        {
            //getting images for promotion
            _promotionImage = await BlobClient.BlobClient.GetBlob(_promotionImageURL);
            return _promotionImage;
        }
       
        private string ConvertSecondsToMinutes(string value)
        {
            TimeSpan timespan = TimeSpan.FromSeconds(Convert.ToInt32(value));
            int totalMins = (int)timespan.TotalMinutes;
            string displayTime = Convert.ToString(totalMins);

            if (totalMins == 1)
                displayTime = displayTime + " minute";
            else
                displayTime = displayTime + " minutes";

            return displayTime;
        }
    

    /* public override async void Start()
     {
         base.Start();
         await ReloadDataAsync();
     }

     protected override Task InitializeAsync()
     {
         return Task.Run(async () =>
         {

         });
     }*/
}
}