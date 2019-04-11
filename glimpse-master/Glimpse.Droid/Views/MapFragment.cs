using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Google.Maps.Android.Clustering;
using Com.Google.Maps.Android.Clustering.Algo;
using Glimpse.Core.Contracts.Services;
using Glimpse.Core.Helpers;
using Glimpse.Core.Model;
using Glimpse.Core.ViewModel;
using Glimpse.Droid.Activities;
using Glimpse.Droid.Helpers;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platform;
using static Android.Gms.Maps.GoogleMap;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Droid.WeakSubscription;
using MvvmCross.Platform.WeakSubscription;
using Glimpse.Droid.Controls.Listener;

namespace Glimpse.Droid.Views
{
    [MvxFragment(typeof(MainViewModel), Resource.Id.viewPager, true)]
    [Register("glimpse.droid.views.MapFragment")]
    public class MapFragment : MvxFragment<MapViewModel>, IOnMapReadyCallback,
        ClusterManager.IOnClusterItemClickListener, ClusterManager.IOnClusterClickListener, IOnCameraIdleListener, RadioGroup.IOnCheckedChangeListener 
    {
        private MapView _mapView;
        private GoogleMap map;
        private Marker currentUserLocation;
        private Context globalContext;
        private LatLng location;
        private ClusterManager clusterManager;
        private List<PromotionItem> clusterList;
        private List<PromotionWithLocation> activePromotions;
        private IAlgorithm clusterAlgorithm;
        private RadioGroup _radioGroup;
        private SearchView _searchView;


        private Dictionary<int, PromotionItem> visibleMarkers = new Dictionary<int, PromotionItem>();

        private List<PromotionItem> itemsList = new List<PromotionItem>();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(Resource.Layout.MapView, null);
            _mapView = view.FindViewById<MapView>(Resource.Id.map);
            _mapView.OnCreate(savedInstanceState);

           
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {

            _searchView = (SearchView)View.FindViewById(Resource.Id.map_searchview);
            _radioGroup = (RadioGroup)View.FindViewById(Resource.Id.mapfilter_radiogroup);

            _searchView.SearchClick += delegate
            {
                _radioGroup.Visibility = ViewStates.Visible;
            };

            //done this weird way because of issue clearing the focus of the search view
            var listener = new MySearchViewOnCloseListener();
            listener.view = _radioGroup;
            _searchView.SetOnCloseListener(listener);
        }

        public override void OnActivityCreated(Bundle p0)
        {
            base.OnActivityCreated(p0);
            // (this.Activity as MainActivity).SetCustomTitle("MapView");
            MapsInitializer.Initialize(Activity);
            IMvxNotifyPropertyChanged viewModel = ViewModel as IMvxNotifyPropertyChanged;
            viewModel.WeakSubscribe(PropertyChanged);
        }

        private void PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Query" /*|| e.PropertyName == "SelectedItem"*/)
            {
                clusterManager.ClearItems();
                clusterList.Clear();
                ShowPromotionsOnMap();               
            }
            else if(e.PropertyName == "Location")
            {
                ChangeMapFocus(ViewModel.Location.Lat, ViewModel.Location.Lng);
            }
        }


        public override void OnStart()
        {
            base.OnStart();
        }


        public override void OnDestroyView()
        {
            base.OnDestroyView();
            _mapView.OnDestroy();
        }

        /*   public override void OnSaveInstanceState(Bundle outState)
           {
               base.OnSaveInstanceState(outState);
               _mapView.OnSaveInstanceState(outState);
           } */

        public override async void OnResume()
        {
            base.OnResume();

            globalContext = Context;
            //if location services are not enabled do not go further
            if (!CheckLocationServices())
            {
                var alert = new AlertDialog.Builder(globalContext);
                alert.SetTitle("Location services are turned off");
                alert.SetMessage("Please enable Location Services!");
                alert.SetPositiveButton("OK", (senderAlert, args) => { });
                var ad = alert.Create();

                ad.Show();
            }
            //location services are on so we can continue
            else
            {
                //Create a progress dialog for loading
                var pr = new ProgressDialog(globalContext);
                pr.SetMessage("Loading Current Position");
                pr.SetCancelable(false);

                var viewModel = ViewModel;
                pr.Show();
                //Get the location
                var locationAsModel = await viewModel.GetUserLocation();

                location = new LatLng(locationAsModel.Lat, locationAsModel.Lng);
                pr.Hide();
                SetUpMap();
            }

            _mapView.OnResume();
        }

        public override void OnPause()

        {
            base.OnPause();
            _mapView.OnPause();
        }

        public override void OnLowMemory()

        {
            base.OnLowMemory();
            _mapView.OnLowMemory();
        }

        private void SetUpMap()
        {
            View.FindViewById<MapView>(Resource.Id.map).GetMapAsync(this);
        }

        private bool CheckLocationServices()
        {
            var locMgr =
                (LocationManager)(Activity as MainActivity).GetSystemService(Context.LocationService);


            var gps_enabled = false;
            var network_enabled = false;

            gps_enabled = locMgr.IsProviderEnabled(LocationManager.GpsProvider);

            network_enabled = locMgr.IsProviderEnabled(LocationManager.NetworkProvider);

            return gps_enabled || network_enabled;
        }    

        private void ChangeMapFocus(double lat, double lng)
        {
            var latLng = new LatLng(lat, lng);
            var cameraUpdate = CameraUpdateFactory.NewLatLng(latLng);
            map.AnimateCamera(cameraUpdate);
        }


        public void OnMapReady(GoogleMap googleMap)
        {
            map = googleMap;
            try
            {
                var success =
                    googleMap.SetMapStyle(MapStyleOptions.LoadRawResourceStyle(Context, Resource.Raw.style_json));
            }
            catch (Exception e)
            {
            }

            var viewModel = ViewModel;


            //current user marker
            var options = new MarkerOptions();
            options.SetPosition(location);
            options.SetTitle("My Position");
            options.SetAlpha(0.7f);
            options.SetIcon(BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueMagenta));
            options.InfoWindowAnchor(0.7f, 0.7f);
            options.SetSnippet("My position");

            currentUserLocation = map.AddMarker(options);

            //camera initialized on the user            
            var builder = CameraPosition.InvokeBuilder();
            builder.Target(location);
            builder.Zoom(viewModel.DefaulZoom);
            builder.Bearing(viewModel.DefaultBearing);
            builder.Tilt(viewModel.DefaultTilt);
            var cameraPosition = builder.Build();
            var cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
            map.MoveCamera(cameraUpdate);

            var set = this.CreateBindingSet<MapFragment, MapViewModel>();
            set.Bind(currentUserLocation)
                .For(m => m.Position)
                .To(vm => vm.UserCurrentLocation)
                .WithConversion(new LatLngValueConverter(), null).TwoWay();
            set.Apply();

            //map settings
            map.UiSettings.MapToolbarEnabled = true;
            map.UiSettings.ZoomControlsEnabled = true;
            map.UiSettings.CompassEnabled = true;
            map.UiSettings.MyLocationButtonEnabled = true;
            map.UiSettings.RotateGesturesEnabled = true;
            map.UiSettings.ZoomGesturesEnabled = true;
            map.BuildingsEnabled = true;


            //TEST
            clusterManager = new ClusterManager(Context, map);
            clusterAlgorithm = new PreCachingAlgorithmDecorator(new NonHierarchicalDistanceBasedAlgorithm());
            clusterManager.Algorithm = clusterAlgorithm;

            clusterManager.SetOnClusterClickListener(this);
            clusterManager.SetOnClusterItemClickListener(this);
            map.SetOnCameraIdleListener(clusterManager);
            map.SetOnMarkerClickListener(clusterManager);

            clusterList = new List<PromotionItem>();

            //Clear Map Promotions
            clusterManager.ClearItems();
            clusterList.Clear();

            //Show promotions from vendors
            ShowPromotionsOnMap();

            //setup radiogroup
            _radioGroup = (RadioGroup)View.FindViewById(Resource.Id.mapfilter_radiogroup);

            //TODO Set the Radiobutton to All Categories by default

            //View allCategoriesFilter = _radioGroup.GetChildAt(0);
            //_radioGroup.Check(allCategoriesFilter);

            _radioGroup.SetOnCheckedChangeListener(this);
            
        }

        public bool OnClusterClick(ICluster cluster)
        {
            Toast.MakeText(Context, "Cluster clicked", ToastLength.Short).Show();
            return false;
        }

        public bool OnClusterItemClick(Java.Lang.Object vendor)
        {
            PromotionItem item = (PromotionItem)vendor;

            //Get the last created promotion by the vendor
            PromotionWithLocation currentPromotion = item.CurrentPromotion;

            //Store for analytics
            StoreItemClick(currentPromotion.PromotionId);

            PromotionDialogFragment promotionDialog = new PromotionDialogFragment(item);
            promotionDialog.SetStyle(DialogFragmentStyle.NoFrame, Resource.Style.Theme_AppCompat_Light_Dialog);

            promotionDialog.Show(Activity.FragmentManager, "put a tag here");

            return false;
        }

        private async void StoreItemClick(int promotionId)
        {
            await ViewModel.StorePromotionClick(promotionId);
        }

        public async void OnCameraIdle()
        {
        }

        private void CreateClusterItem(List<PromotionWithLocation> promotionItems, double lat, double lng)
        {
            clusterList.Add(new PromotionItem(promotionItems, lat, lng));
        }

        private void GenerateCluster()
        {
            clusterManager.AddItems(clusterList);
            clusterManager.Cluster();
        }


        private async void ShowPromotionsOnMap()
        {
            var viewModel = ViewModel;
            var vendorService = Mvx.Resolve<IVendorDataService>();

            if (ViewModel.FilteredPromotionList == null)
                activePromotions = await ViewModel.GetActivePromotions();
            else
                activePromotions = ViewModel.FilteredPromotionList;

            List<Vendor> allVendors = await vendorService.GetVendors();

            var uniqueVendors = allVendors.GroupBy(x => new { x.Location.Lat, x.Location.Lng }).Select(g => g.First()).ToList();

            //Print out the pins
            foreach (var v in uniqueVendors)
            {
                //Get promotions for each vendor
                var promotionsList = activePromotions.Where(e => e.VendorId == v.VendorId).ToList();

                //If there are is no current promotion for vendor
                if (promotionsList.Count != 0)
                {
                    CreateClusterItem(promotionsList, v.Location.Lat, v.Location.Lng);
                }
            }
            GenerateCluster();
        }

        public void OnCheckedChanged(RadioGroup group, int checkedId)
        {
            //radio group index is based on 1, making base 0
            checkedId = checkedId - 1;
            
            //radio group index seem to be incremented by 7 randomly, might be issue with MvxRadioGroup
            checkedId = checkedId % 7;
            if (checkedId == 0)
            {
                ViewModel.SelectedItem = null;
                //GetAllActivePromotions();         
            }
            else
            {
                int checkedId0BasedIndex = checkedId - 1;
                Categories category = (Categories)checkedId0BasedIndex;
                ViewModel.SelectedItem = category;
            }

            clusterManager.ClearItems();
            clusterList.Clear();
            ShowPromotionsOnMap();
        }


        private async void GetAllActivePromotions()
        {
            await ViewModel.GetActivePromotions();
        }
        /*
                //Note that the type "Items" will be whatever type of object you're adding markers for so you'll
                //likely want to create a List of whatever type of items you're trying to add to the map and edit this appropriately
                //Your "Item" class will need at least a unique id, latitude and longitude.
                private void addItemsToMap(List<PromotionItem> items)
                {
                    if (map != null)
                    {
                        //This is the current user-viewable region of the map
                        LatLngBounds bounds = map.Projection.VisibleRegion.LatLngBounds;
        
                        //Loop through all the items that are available to be placed on the map
                        foreach(PromotionItem item in items)
                        {
                            //If the item is within the the bounds of the screen
                            if (bounds.Contains(new LatLng(item.Position.Latitude, item.Position.Longitude)))
                            {
                                //If the item isn't already being displayed
                                if (!clusterManager.containsKey(item.getId()))
                                {
                                    //Add the Marker to the Map and keep track of it with the HashMap
                                    //getMarkerForItem just returns a MarkerOptions object
                                    this.courseMarkers.put(item.getId(), this.mMap.addMarker(getMarkerForItem(item)));
                                }
                            }
        
                            //If the marker is off screen
                            else
                            {
                                //If the course was previously on screen
                                if (courseMarkers.containsKey(item.getId()))
                                {
                                    //1. Remove the Marker from the GoogleMap
                                    courseMarkers.get(item.getId()).remove();
        
                                    //2. Remove the reference to the Marker from the HashMap
                                    courseMarkers.remove(item.getId());
                                }
                            }
                        }
                    }
                }
                */
    }
}



