using MvvmCross.Plugins.Messenger;
using Glimpse.Core.Model;
using Glimpse.Core.Contracts.Services;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System.Collections.Generic;
using System.Linq;
using Glimpse.Core.Helpers;
using System.Collections;
using System;

namespace Glimpse.Core.ViewModel
{
    public class MapViewModel : BaseViewModel

    {
        private readonly int _defaultZoom = 18;
        private readonly int _defaultTilt = 65;
        private readonly int _defaultBearing = 155;
        private Dictionary<Vendor, List<Promotion>> _vendorData = new Dictionary<Vendor, List<Promotion>>();
        private IVendorDataService vendorDataService;
        private IPromotionDataService promotionDataService;
        private IPromotionClickDataService promotionClickDataService;
        private Location _userCurrentLocation;
        private List<PromotionWithLocation> _promotionsStored;
        private IGeolocator locator;
        public delegate void LocationChangedHandler(object sender, LocationChangedHandlerArgs e);
        public event LocationChangedHandler LocationUpdate;


        public MapViewModel(IMvxMessenger messenger, IVendorDataService vendorDataService, IPromotionDataService promotionDataService, IPromotionClickDataService promotionClickDataService) : base(messenger)
        {
            this.vendorDataService = vendorDataService;
            this.promotionDataService = promotionDataService;
            this.promotionClickDataService = promotionClickDataService;
        }

        public override async void Start()
        {
            base.Start();
            await ReloadDataAsync();
        }     

        private Location _location;
        public Location Location
        {
            get
            {               
                return _location;
            }
            set
            {
                _location = value;
                RaisePropertyChanged(() => Location);
            }
        }


        protected override async Task InitializeAsync()
        {
            //Creates the locator
            locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 5;

            //Setting up the event and start listening
            locator.PositionChanged += Locator_PositionChanged;
            await locator.StartListeningAsync(minTime: 1, minDistance: 10);
            Query = "";
        }

        public async Task StorePromotionClick(int promotionId)
        {
            PromotionClick promotionClick = new PromotionClick
            {
                PromotionId = promotionId,
                Time = DateTime.Now
            };

            await promotionClickDataService.StorePromotionClick(promotionClick);
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

        /// <summary>
        /// Event for when position changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Locator_PositionChanged(object sender, PositionEventArgs e)
        {
            UserCurrentLocation = new Location()
            {
                Lat = e.Position.Latitude,
                Lng = e.Position.Longitude
            };           
        }    

        public int DefaulZoom
        {
            get { return _defaultZoom; }
        }


        public int DefaultTilt
        {
            get { return _defaultTilt; }
        }

        public int DefaultBearing
        {
            get { return _defaultBearing; }
        }


        public Location UserCurrentLocation
        {
            get { return _userCurrentLocation; }
            set { _userCurrentLocation = value; RaisePropertyChanged(() => UserCurrentLocation); }
        }

        public Dictionary<Vendor, List<Promotion>> VendorData
        {
            get { return _vendorData; }
            set
            {
                _vendorData = value;
                RaisePropertyChanged(() => VendorData);
            }
        }
        
        public async Task<List<PromotionWithLocation>> GetActivePromotions()
        {
            _promotionsStored = await promotionDataService.GetActivePromotions();
            _filteredpromotions = _promotionsStored;
            return _promotionsStored;
        }

        //Map filtering section

        private List<PromotionWithLocation> _filteredpromotions;
        public List<PromotionWithLocation> FilteredPromotionList
        {
            get { return _filteredpromotions; }
            set
            {
                _filteredpromotions = value;
                RaisePropertyChanged(() => FilteredPromotionList);
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
                FilteredPromotionList = promotionDataService.FilterPromotionWithLocationList(_promotionsStored, _selectedItem, Query);
                RaisePropertyChanged(() => FilteredPromotionList);
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
                FilteredPromotionList = promotionDataService.FilterPromotionWithLocationList(_promotionsStored, SelectedItem, _query);
                RaisePropertyChanged(() => Query);
            }
        }

    }

}

