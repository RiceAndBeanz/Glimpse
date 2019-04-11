using System;
using System.Collections.Generic;
using Glimpse.Core.Model;
using System.IO;
using Glimpse.Core.Contracts.Services;
using MvvmCross.Core.ViewModels;
using Glimpse.Core.Services.General;
using System.Text;

namespace Glimpse.Core.ViewModel
{
    public class CreatePromotionPart2ViewModel : BaseViewModel
    {
        private readonly IPromotionDataService _promotionDataService;
        private IVendorDataService _vendorDataService;
        private readonly IPromotionImageDataService _promotionImageDataService;
        Dictionary<string, string> dataFromCreatePromotionPart1 = new Dictionary<string, string>();
        private Categories selectedCategory;
        private Vendor vendor;
        public CreatePromotionPart2ViewModel(IPromotionDataService promotionDataService, IVendorDataService vendorDataService, IPromotionImageDataService promotionImageDataService)
        {
            _promotionDataService = promotionDataService;
            _vendorDataService = vendorDataService;
            _promotionImageDataService = promotionImageDataService;
            _promotionImageList = new List<byte[]>();
        }

        protected override void InitFromBundle(IMvxBundle parameters)
        {
            var myPara = parameters;

            foreach (string key in parameters.Data.Keys)
            {
                dataFromCreatePromotionPart1.Add(key, parameters.Data[key]);
            }
            base.InitFromBundle(parameters);
        }

        private DateTime _promotionStartDate;
        public DateTime PromotionStartDate
        {
            get { return _promotionStartDate; }
            set { _promotionStartDate = value; RaisePropertyChanged(() => PromotionStartDate); }
        }

        private DateTime _promotionEndDate;
        public DateTime PromotionEndDate
        {
            get { return _promotionEndDate; }
            set { _promotionEndDate = value; RaisePropertyChanged(() => PromotionEndDate); }
        }

        private List<byte[]> _promotionImageList;
        public List<byte[]> PromotionImageList
        {
            get { return _promotionImageList; }
            set
            {
                _promotionImageList = value;
                RaisePropertyChanged(() => PromotionImageList);
            }
        }


        private byte[] _bytes;
        public byte[] Bytes
        {
            get { return _bytes; }
            set { _bytes = value; RaisePropertyChanged(() => Bytes); }
        }

        private void OnPicture(Stream pictureStream)
        {
            var memoryStream = new MemoryStream();
            pictureStream.CopyTo(memoryStream);
            Bytes = memoryStream.ToArray();
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


        private bool _isBusy=false;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }


        public MvxCommand createPromotion
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    IsBusy = true;
                    if (Bytes == null || PromotionEndDate.Year == 0001)
                    {
                        ErrorMessage = TextSource.GetText("ErrorMissingField");
                        IsBusy = false;
                    }
                    else
                    {

                        foreach (string key in dataFromCreatePromotionPart1.Keys)
                        {
                            if (dataFromCreatePromotionPart1[key] == "True")
                                selectedCategory = (Categories)Enum.Parse(typeof(Categories), key, true);
                        }

                        //Calculate DateTime span
                        //TimeSpan promotionLength = _promotionEndDate - _promotionStartDate;

                        if (!string.IsNullOrEmpty(Settings.Email))
                        {
                            vendor = await _vendorDataService.SearchVendorByEmail(Settings.Email);
                        }

                        Promotion promotion = new Promotion()
                        {
                            Title = dataFromCreatePromotionPart1["PromotionTitle"],
                            Description = dataFromCreatePromotionPart1["PromotionDescription"],
                            Category = selectedCategory,
                            PromotionStartDate = _promotionStartDate,
                            PromotionEndDate = _promotionEndDate,
                            PromotionImage = Bytes,
                            PromotionImageURL = vendor.VendorId + "/" + removeSpecialChars(dataFromCreatePromotionPart1["PromotionTitle"]).Replace(" ", string.Empty) + "/" + "cover",
                            VendorId = vendor.VendorId,
                        };

                        vendor.Promotions.Add(promotion);

                        await _promotionDataService.StorePromotion(promotion);

                        //this next line is not actually adding promotions, dont know why, works for all other
                        //await _vendorDataService.EditVendor(vendor.VendorId, vendor);
                        List<Promotion> promotions = await _promotionDataService.GetPromotions();

                        //index for unique naming the promotion image
                        int i = 1;
                        foreach (byte[] promotionImage in PromotionImageList)
                        {
                            PromotionImage promotionImageInstance = new PromotionImage()
                            {
                                Image = promotionImage,
                                PromotionId = promotions[promotions.Count - 1].PromotionId,
                                ImageURL = vendor.VendorId + "/" + removeSpecialChars(dataFromCreatePromotionPart1["PromotionTitle"]).Replace(" ", string.Empty) + "/" + "image" + i
                            };

                            await _promotionImageDataService.StorePromotion(promotionImageInstance);
                            i++;
                        }
                        Bytes = null;
                        PromotionEndDate = new DateTime(0001, 1, 1);
                        IsBusy = false;
                        ShowViewModel<VendorProfilePageViewModel>(new { index = 0 });
                    }
                });
            }
        }


        private readonly static string reservedCharacters = "!*'();:@&=+$,/?%#[]";
        public static string removeSpecialChars(string value)
        {
            if (String.IsNullOrEmpty(value))
                return String.Empty;

            var sb = new StringBuilder();

            foreach (char @char in value)
            {
                if (reservedCharacters.IndexOf(@char) == -1)
                    sb.Append(@char);
                else
                    sb.AppendFormat("", (int)@char);
            }
            return sb.ToString();
        }

    }
}
