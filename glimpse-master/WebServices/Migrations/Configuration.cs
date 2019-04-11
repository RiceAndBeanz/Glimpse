namespace WebServices.Migrations
{
    using Glimpse.Core.Services.General;
    using Helpers;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    internal sealed class Configuration : DbMigrationsConfiguration<WebServices.Models.GlimpseDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }       


        protected override void Seed(WebServices.Models.GlimpseDbContext context)
        {

          IList<Vendor> vendors = generateVendors();

         IList<Promotion> promotions = GeneratePromotions(5, context.Vendors.Select(vendor => vendor.VendorId).ToList());

            



      //      foreach (Vendor vendor in vendors)
       //         context.Vendors.Add(vendor);


          foreach (Promotion promotion in promotions)
                context.Promotions.Add(promotion);
            

            base.Seed(context); 
        }

        private  List<Promotion> GeneratePromotions(int numberOfPromotions, List<int> vendorIds)
        {
            List<Promotion> promotions = new List<Promotion>();
            BlobHelper bh = new BlobHelper("storageglimpse", "UTaxV/U+abo8S1ORGCTyAVH4dUoFxl5jonIxMNAK/GUNP5u0IbNxa8WxyJpWbrg2aeUlm6S1NAkph/hW3i69wQ==", "imagestorage");

            for (int i = 0; i < numberOfPromotions; i++)
            {
                //get random vendorid
                int vendorId = vendorIds[random.Next(0, vendorIds.Count)];
                string title = Faker.Lorem.Sentence(1);

                Promotion promo = new Promotion()
                {
                    Title = title,
                    Description = Faker.Lorem.Sentence(3),
                    Category = GetRandomCategory(),
                    PromotionStartDate = DateTime.Now,
                    PromotionEndDate = GetRandomDate(),
                    PromotionImage = GetByteArrayOfRandomImage(),
                    PromotionImageURL = vendorId + "/" + "test".Replace(" ", string.Empty).Replace(".", string.Empty) + "/" + "cover",
                    // need to access the vendors i just created
                    VendorId = vendorId
                };

                promotions.Add(promo);     

            }

            Parallel.ForEach(promotions, (promo) =>
            {

               // bh.UploadFromByteArray(promo.PromotionImage, promo.PromotionImageURL).Wait();


                //close lambda expression and method invocation
            });

            return promotions;
        }

        private byte[] GetByteArrayOfRandomImage()
        {

            //get random picture from the ressources
            Bitmap image;//= WebServices.Properties.Resources.pic1;
            int pictureIndex = random.Next(7);

            switch (pictureIndex)
            {
                case 0:
                    image = WebServices.Properties.Resources.pic0;
                    break;
                case 1:
                    image = WebServices.Properties.Resources.pic1;
                    break;
                case 2:
                    image = WebServices.Properties.Resources.pic2;
                    break;
                case 3:
                    image = WebServices.Properties.Resources.pic3;
                    break;
                case 4:
                    image = WebServices.Properties.Resources.pic4;
                    break;
                case 5:
                    image = WebServices.Properties.Resources.pic5;
                    break;
                case 6:
                    image = WebServices.Properties.Resources.pic6;
                    break;
                default:
                    image = WebServices.Properties.Resources.pic0;
                    break;
            }           

            byte[] data;

            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, ImageFormat.Bmp);

                data = memoryStream.ToArray();
            }

            return data;
        }

        private List<Vendor> generateVendors()
        {
            List<string> defaultVendorNames = new List<string>()
            {
                "sam",
                "eric",
                "joseph",
                "omer",
                "asma"
            };



            List<Vendor> defaultVendors = new List<Vendor>();

            foreach(string name in defaultVendorNames)
            {
                //have an issue with using PCLCrypto, meanwhile must be done manually
                //Tuple<string, string> passwordTuple = Cryptography.EncryptAes(name);

                defaultVendors.Add(new Vendor()
                {
                    Email = name + "@gmail.com",
                    Password = "fakepassword",//passwordTuple.Item1,
                    Salt = "fakesalt",//passwordTuple.Item2,
                    CompanyName = Faker.Company.Name(),
                    Address = Faker.Address.Country() + "," + Faker.Address.UsState() + "," + Faker.Address.City() +
                            "," + Faker.Address.StreetSuffix() + "," + Faker.Address.ZipCode(),
                    Telephone = Faker.Phone.Number(),
                    Location = GetRandomLatLon()                    
                });
            }

            return defaultVendors;

       }


       //     protected override void Seed(WebServices.Models.GlimpseDbContext context)
     //       {
          /*
           * 

                foreach (Promotion promotion in promotions1)
                {
                    context.Promotions.Add(promotion);
                }

                foreach (Promotion promotion in promotions2)
                {
                    context.Promotions.Add(promotion);
                }

                foreach (Promotion promotion in promotions3)
                {
                    context.Promotions.Add(promotion);
                }

                foreach (Promotion promotion in promotions4)
                {
                    context.Promotions.Add(promotion);
                }

                foreach (Promotion promotion in promotions5)
                {
                    context.Promotions.Add(promotion);
                }

                foreach (Promotion promotion in promotions6)
                {
                    context.Promotions.Add(promotion);
                }

                foreach (Promotion promotion in promotions7)
                {
                    context.Promotions.Add(promotion);
                }

                foreach (Promotion promotion in promotions8)
                {
                    context.Promotions.Add(promotion);
                }

                foreach (Promotion promotion in promotions9)
                {
                    context.Promotions.Add(promotion);
                }

                foreach (Promotion promotion in promotions10)
                {
                    context.Promotions.Add(promotion);
                }

                foreach (User user in users)
                {
                    context.Users.Add(user);
                }

                foreach (Vendor vendor in vendors)
                {
                    context.Vendors.Add(vendor); */
    //        }









        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static Location GetRandomLatLon()
        {
            //this is around hall building
            
            //near eric's house
            double centerLat = 45.572223;
            double centerLng = -73.624737;

            //how far from center promotions will be generated
            double radius = 0.02;

            double lat = centerLat + (random.NextDouble() * radius);
            double lon = centerLng + (random.NextDouble() * radius);
            return new Location(lat, lon);
        }

        private static Categories GetRandomCategory()
        {
            Array values = Enum.GetValues(typeof(Categories));
            Categories category = (Categories)values.GetValue(random.Next(values.Length));
            return category;
        }

        private static DateTime GetRandomDate()
        {
            DateTime start = DateTime.Now;
           
            int randomDays = random.Next(30);

            return start.AddDays(randomDays);         
        }

        private static bool GetRandomBoolean()
        {
            return random.NextDouble() >= 0.5;
        }
    
        
    }
}
