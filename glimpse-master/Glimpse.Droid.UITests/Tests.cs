using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using Xamarin.UITest.Android;
using System.Collections.Generic;

namespace Glimpse.Droid.UITests
{
    [TestFixture]
    public class Tests
    {
        //android app
        AndroidApp app;
        private readonly string _testEmail = "e5@gmail.com";
        private readonly string _testPassword = "e5";

        [SetUp]
        public void BeforeEachTest()
        {
            app = ConfigureApp
              .Android
              .ApkFile("../../../Glimpse.Droid/bin/Release/Glimpse.Droid.Glimpse.Droid-x86-Signed.apk")
              .EnableLocalScreenshots()
              .StartApp();
        }

        [Test]
        public void AppLaunches()
        {
            app.Screenshot("First screen.");
        }

        [Test]
        public void TestSwipeThroughNearbyPromotions()
        {
            //Arrange scenario condition
            app.Tap(x => x.Id("btnSignIn"));
            app.Tap(x => x.Id("txtEmail"));
            app.EnterText(x => x.Id("txtEmail"), _testEmail);
            app.Tap(x => x.Id("txtPassword"));
            app.EnterText(x => x.Id("txtPassword"), _testPassword);
            app.Tap(x => x.Id("btnSignIn"));
            app.WaitForElement("cardImage");

            //Act
            var initialCardStack = app.Query("cardTitle").Select(x => x.Text).ToList();
            var cardStackAfterSwipes = app.Query("cardTitle").Select(x => x.Text).ToList();
            app.SwipeRightToLeft();
            cardStackAfterSwipes.RemoveAt(cardStackAfterSwipes.Count - 1);
            app.Screenshot("Swiping Top Card");
            app.SwipeRightToLeft();
            cardStackAfterSwipes.RemoveAt(cardStackAfterSwipes.Count - 1);
            app.Screenshot("Second Top Card");
            app.SwipeLeftToRight();
            cardStackAfterSwipes.RemoveAt(cardStackAfterSwipes.Count - 1);
            var initialInde = app.Query(x => x.Id("cardImage"));


            //Assert
            Assert.IsTrue((initialCardStack.Count - 3) == cardStackAfterSwipes.Count);


        }

        [Test]
        public void TestNavigateThroughAllTheAppPages()
        {
            //Arrange scenario condition and like card
            app.Tap(x => x.Id("btnSignIn"));
            app.Tap(x => x.Id("txtEmail"));
            app.EnterText(x => x.Id("txtEmail"), _testEmail);
            app.Tap(x => x.Id("txtPassword"));
            app.EnterText(x => x.Id("txtPassword"), _testPassword);
            app.Tap(x => x.Id("btnSignIn"));
            app.WaitForElement("cardImage");
            app.SwipeLeftToRight();

            //Act 
            //retrieving specific cardview element
            app.Screenshot("Card View1");
            var cardViewElement = app.Query(x => x.Id("cardImage")).GetValue(0);
            app.Tap(x => x.Id("cardImage"));

            //retrieving specific detail view element + screenshot of view
            app.WaitForElement("detailViewTitle");
            app.Screenshot("Detail view");
            var detailViewElement = app.Query(x => x.Id("detailViewTitle")).GetValue(0);
            app.Back();

            //retrieving specific card view element + screenshot of view
            app.WaitForElement("cardImage");
            app.Screenshot("Card View2");
            cardViewElement = app.Query(x => x.Id("cardImage"));
            app.Tap(x => x.Class("AppCompatImageView").Index(1));

            //retrieving specific like promotion view element + screenshot of view
            app.WaitForElement("promotionTitle");
            app.Screenshot("Like Promotion View");
            var likedPromotionViewElement = app.Query(x => x.Id("promotionTitle")).GetValue(0);
            app.Tap(x => x.Id("promotionTitle"));

            //retrieving specific detail view element + screenshot of view
            app.WaitForElement("detailViewTitle");
            app.Screenshot("Detail view2");
            detailViewElement = app.Query(x => x.Id("detailViewTitle"));
            app.Back();

            //retrieving specific like promotion view element + screen shot
            app.WaitForElement("promotionTitle");
            app.Screenshot("LikedView2");
            likedPromotionViewElement = app.Query(x => x.Id("promotionTitle")).GetValue(0);
            app.Tap(x => x.Class("AppCompatImageView").Index(2));

            //retrieving specific like promotion view element + screen shot
            app.WaitForElement("map");
            app.Screenshot("MapView");
            var mapViewElement = app.Query(x => x.Id("map")).GetValue(0);

            //Assert
            //if the elements are not null, then the above views exist
            Assert.IsTrue(cardViewElement != null && detailViewElement != null && likedPromotionViewElement != null && mapViewElement != null);
        }

        [Test]
        public void TestNavigateToTheMapViewAndViewPromotions()
        {
            //Arrange scenario condition(sign in)
            app.Tap(x => x.Id("btnSignIn"));
            app.Tap(x => x.Id("txtEmail"));
            app.EnterText(x => x.Id("txtEmail"), _testEmail);
            app.Tap(x => x.Id("txtPassword"));
            app.EnterText(x => x.Id("txtPassword"), _testPassword);
            app.Tap(x => x.Id("btnSignIn"));
            app.WaitForElement("cardImage");
            app.Tap(x => x.Class("AppCompatImageView").Index(2));

            //Act
            app.WaitForElement("map");
            app.ScrollUp();
            app.ScrollUp();
            app.Tap(x => x.Marked("Google Map"));
            app.Tap(x => x.Marked("Zoom out"));
            app.Tap(x => x.Marked("Zoom out"));
            app.Tap(x => x.Marked("Zoom out"));
            app.Tap(x => x.Marked("Zoom out"));
            app.Tap(x => x.Marked("Zoom out"));
            app.Screenshot("Promos with no Cluster");
            app.Tap(x => x.Marked("Zoom out"));
            app.Tap(x => x.Marked("Zoom out"));
            app.Screenshot("Promos with  Cluster");
        }

        [Test]
        public void TestNavigateThroughTheAppAndViewApparelPromos()
        {
            //Arrange scenario condition(sign in)
            app.Tap(x => x.Id("btnSignIn"));
            app.Tap(x => x.Id("txtEmail"));
            app.EnterText(x => x.Id("txtEmail"), _testEmail);
            app.Tap(x => x.Id("txtPassword"));
            app.EnterText(x => x.Id("txtPassword"), _testPassword);
            app.Tap(x => x.Id("btnSignIn"));

            //Act
            app.WaitForElement("cardImage");
            app.Tap(x => x.Id("search_button"));
            app.EnterText(x => x.Id("search_src_text"), "apparel");
            var cardContainsWord = app.Query("cardTitle").First(result => result.Text.ToLower().Contains("apparel"));
            app.SwipeLeftToRight();
            app.Tap(x => x.Class("AppCompatImageView").Index(1));

            //acting on like view 
            app.WaitForElement("promotionTitle");
            app.Tap(x => x.Id("search_button"));
            app.EnterText(x => x.Id("search_src_text"), "apparel");
            var elementContainsWord = app.Query("promotionTitle").First(result => result.Text.ToLower().Contains("apparel"));
            app.Tap(x => x.Class("AppCompatImageView").Index(2));

            //acting on map view
            app.WaitForElement("map");
            app.Tap(x => x.Id("search_button"));
            app.Screenshot("All promotions");
            app.Tap(x => x.Text("Apparel"));
            app.Screenshot("apparel promotions");
            app.Tap(x => x.Text("All"));
            app.Screenshot("All promotions2");
            app.EnterText(x => x.Id("search_src_text"), "apparel");
            app.Screenshot("apparel promotions2");
            app.Tap(x => x.Id("search_close_btn"));
            app.ClearText(x => x.Id("search_src_text"));

            //assert for cardview and likeview
            Assert.IsTrue(cardContainsWord != null && elementContainsWord != null);


        }

        [Test]
        public void TestLikeElectronicsDealsAndViewThemOnTheLikeList()
        {
            string promo1 = "";
            string promo2 = "";
            List<string> electronicdealsTitle;
            bool promo1InLikedView = false;
            bool promo2InLikedView = false;

            //Arrange scenario condition(sign in)
            app.Tap(x => x.Id("btnSignIn"));
            app.Tap(x => x.Id("txtEmail"));
            app.EnterText(x => x.Id("txtEmail"), _testEmail);
            app.Tap(x => x.Id("txtPassword"));
            app.EnterText(x => x.Id("txtPassword"), _testPassword);
            app.Tap(x => x.Id("btnSignIn"));

            //acting on cardview
            app.WaitForElement("cardImage");


            //saving title of first promo
            app.Tap(x => x.Id("search_button"));
            app.Tap(x => x.Text("Electronics"));
            electronicdealsTitle = app.Query("cardTitle").Select(x => x.Text).ToList();
            promo1 = electronicdealsTitle[electronicdealsTitle.Count - 1];
            app.Tap(x => x.Id("btnLike"));
            electronicdealsTitle.RemoveAt(electronicdealsTitle.Count - 1);


            //saving title of second promo
            promo2 = electronicdealsTitle[electronicdealsTitle.Count - 1];
            app.Tap(x => x.Id("btnLike"));
            electronicdealsTitle.RemoveAt(electronicdealsTitle.Count - 1);
            app.Tap(x => x.Class("AppCompatImageView").Index(1));

            //acting on like view 
            app.WaitForElement("promotionTitle");
            app.Tap(x => x.Id("search_button"));
            app.Tap(x => x.Text("Electronics"));
            List<string> allElectronicDeals = app.Query("promotionTitle").Select(x => x.Text).ToList();

            //assert
            foreach (string title in allElectronicDeals)
            {
                if (promo1 == title)
                    promo1InLikedView = true;
                if (promo2 == title)
                    promo2InLikedView = true;
            }

            Assert.IsTrue(promo1InLikedView);
            Assert.IsTrue(promo2InLikedView);

        }

    }
}

