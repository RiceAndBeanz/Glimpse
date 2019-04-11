using Glimpse.Core.Contracts.Services;
using Glimpse.Core.Helpers;
using Glimpse.Core.Model;
using Glimpse.Core.Model.CustomModels;
using Glimpse.Core.Services.General;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Glimpse.Core.ViewModel
{
    public class LikedPromotionsViewModel : BaseViewModel
    {

        private ILocalPromotionDataService _localPromotionDataService;
        private IPromotionDataService _promotionDataService;

       

        private List<LikedItemWrap> _promotions;

        private List<PromotionWithLocation> _promotionsStored;

        private IGeolocator locator;
        private Location _userLocation;

        private GoogleWebService _gwb;

        public delegate void LocationChangedHandler(object sender, LocationChangedHandlerArgs e);
        public event LocationChangedHandler LocationUpdate;


        public LikedPromotionsViewModel(IMvxMessenger messenger, ILocalPromotionDataService localPromotionDataService, IPromotionDataService promotionDataService) : base(messenger)
        {
            _localPromotionDataService = localPromotionDataService;
            _promotionDataService = promotionDataService;
        }     

        private Categories? _selectedItem;
        public Categories? SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                List<PromotionWithLocation> filteredPromos = _promotionDataService.FilterPromotionWithLocationList(_promotionsStored, _selectedItem, Query);
                PromotionList = PromotionWithLocationToLikedItemWrap(filteredPromos);
                RaisePropertyChanged(() => PromotionList);
            }
        }

        private string _query=" ";
        public string Query
        {
            get
            {
                return _query;
            }
            set
            {
                _query = value;
                List<PromotionWithLocation> filteredPromos = _promotionDataService.FilterPromotionWithLocationList(_promotionsStored, SelectedItem, _query);
                PromotionList = PromotionWithLocationToLikedItemWrap(filteredPromos);
                RaisePropertyChanged(() => Query);
            }
        }


        private List<string> _categories;
        public List<string> Categories
        {
            get
            {
                List<string> allCategories = new List<string>();
                if (Glimpse.Core.Services.General.Settings.Language == "Français")
                    allCategories.Add("Tout");
                else
                    allCategories.Add("All");
                foreach (string name in Enum.GetNames(typeof(Categories)))
                {
                    if (Glimpse.Core.Services.General.Settings.Language == "Français")
                    {
                        switch (name)
                        {
                            case "Footwear":
                                allCategories.Add("Chaussure");
                                break;
                            case "Electronics":
                                allCategories.Add("Électronique");
                                break;
                            case "Jewellery":
                                allCategories.Add("Bijoux");
                                break;
                            case "Restaurants":
                                allCategories.Add("Restaurants");
                                break;
                            case "Services":
                                allCategories.Add("Services");
                                break;
                            case "Apparel":
                                allCategories.Add("Vêtements");
                                break;
                        }
                    }

                    else
                        allCategories.Add(name);
                };
                return allCategories;
            }
            set
            {
                _categories = value;
                RaisePropertyChanged(() => Categories);
            }
        }

        public List<PromotionWithLocation> PromotionsStored
        {
            get { return _promotionsStored; }
            set
            {
                _promotionsStored = value;
                RaisePropertyChanged(() => PromotionsStored);
            }
        }

        public List<LikedItemWrap> PromotionList
        {
            get { return _promotions; }
            set
            {
                _promotions = value;
                RaisePropertyChanged(() => PromotionList);
            }
        }

        private bool _isRefreshing;
        public virtual bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                RaisePropertyChanged(() => IsRefreshing);
            }
        }

        public override async void Start()
        {
            base.Start();
            await ReloadDataAsync();
        }

        public async Task ReloadAsync()
        {
            await ReloadDataAsync();
        }

        protected override async Task InitializeAsync()
        {
            //Creates the locator
            locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 5;

            //creates the google web service wrapper
            _gwb = new GoogleWebService();

            //get initial user location
            _userLocation = await GetUserLocation();

            Query = "";
            //PromotionList = await _localPromotionDataService.GetPromotions();

            //_promotionsStored = PromotionList;
        }


        public async Task<Location> GetUserLocation()
        {
            //Get the current location            
            var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);


            return new Location()
            {
                Lat = position.Latitude,
                Lng = position.Longitude
            };
        }


        public async Task<List<PromotionWithLocation>> GetPromotionsWithLocation()
        {
            var mapPromotions = LikedItemWrapToPromotionWithLocation(PromotionList);

            List<Location> promotionLocations = mapPromotions.Select(promotionWithLocation => promotionWithLocation.Location).ToList();

            List<IEnumerable<Location>> splitPromotionLocation = promotionLocations.Chunk(10).ToList();


            //index to plug result into mapPromotions list
            int j = 0;
            foreach (IEnumerable<Location> subList in splitPromotionLocation)
            {
                List<Location> subListAsList = subList.ToList();
                DistanceMatrix distanceMatrix = await _gwb.GetMultipleDurationResponse(_userLocation, subListAsList);

                for (int i = 0; i < subListAsList.Count; i++)
                {
                    if (distanceMatrix.rows[0].elements[i].status.Equals("OK"))
                    {
                        mapPromotions[j].Duration = distanceMatrix.rows[0].elements[i].duration.value;
                    }
                    j++;

                }

            }

            List<PromotionWithLocation> final = mapPromotions.OrderBy(promotion => promotion.Duration).ToList().FindAll(p => p.Duration != 9999);

            final = await _promotionDataService.PopulatePromotionWithLocationBlobs(final);

            return final;
        }

        public void btnClick(PromotionWithLocation promo)
        {
            OnLocationUpdate(promo.Location);
        }

        private void OnLocationUpdate(Location location)
        {
            if (LocationUpdate != null)
            {
                LocationChangedHandlerArgs args = new LocationChangedHandlerArgs(location);
                LocationUpdate.Invoke(this, args);
            }
        }
       

        public List<LikedItemWrap> PromotionWithLocationToLikedItemWrap(List<PromotionWithLocation> promoList)
        {
            List<LikedItemWrap> result = new List<LikedItemWrap>();

            foreach(PromotionWithLocation promo in promoList)
            {
                result.Add(new LikedItemWrap(promo, this));
            }

            return result;
        }

        public List<PromotionWithLocation> LikedItemWrapToPromotionWithLocation(List<LikedItemWrap> promoList)
        {
            List<PromotionWithLocation> result = new List<PromotionWithLocation>();

            foreach (LikedItemWrap promo in promoList)
            {
                result.Add(promo.Item);
            }

            return result;
        }       

        public MvxCommand ReloadCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    IsRefreshing = true;

                    await ReloadAsync();

                    IsRefreshing = false;
                });
            }
        }

        public ICommand ViewTileDetails
        {
            get
            {
                return new MvxCommand<LikedItemWrap>(item =>
                {
                    var desc = new Dictionary<string, string> {
                        {"PromotionID", Convert.ToString(item.Item.PromotionId)},
                        {"PromotionTitle", item.Item.Title},
                        {"PromotionDuration", Convert.ToString(item.Item.Duration)},
                        {"PromotionDescription", item.Item.Description},
                        {"ImageURL", item.Item.ImageURL },
                        {"Category", item.Item.Category.ToString()},
                        {"StartDate", item.Item.PromotionStartDate.Ticks.ToString()},
                        {"EndDate", item.Item.PromotionEndDate.Ticks.ToString() }
                    };

                    ShowViewModel<PromoDetailsViewModel>(desc);

                });
            }
        }
    }
}