using Glimpse.Core.Model;
using Glimpse.Core.Services.General;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace WebServices.DataGenerator
{
    public static class DataGenerator
    {
        private static Random random = new Random();

        public static byte[] GetByteArrayOfRandomImage()
        {
            //get random picture from the ressources
            Bitmap image; //= WebServices.Properties.Resources.pic1;
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

        /**
         * Generate random number of promotions with random list of vendor Ids
         **/
        public static List<Promotion> GeneratePromotions(int numberOfPromotions, List<int> vendorIds)
        {
            List<Promotion> promotions = new List<Promotion>();

            for (int i = 0; i < numberOfPromotions; i++)
            {
                //get random vendorid
                int vendorId = vendorIds[random.Next(0, vendorIds.Count)];

                promotions.Add(new Promotion()
                {
                    Title = Faker.Lorem.Sentence(1),
                    Description = Faker.Lorem.Sentence(3),
                    Category = GetRandomCategory(),
                    PromotionStartDate = DateTime.Now,
                    PromotionEndDate = GetRandomDate(),
                    PromotionImage = GetByteArrayOfRandomImage(),
                    // need to access the vendors i just created
                    VendorId = vendorId
                });
            }

            return promotions;
        }

        public static List<Vendor> GenerateVendors()
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

            foreach (string name in defaultVendorNames)
            {
                //have an issue with using PCLCrypto, meanwhile must be done manually
                //Tuple<string, string> passwordTuple = Cryptography.EncryptAes(name);

                defaultVendors.Add(new Vendor()
                {
                    Email = name + "@gmail.com",
                    Password = "fakepassword", //passwordTuple.Item1,
                    Salt = "fakesalt", //passwordTuple.Item2,
                    CompanyName = Faker.Company.Name(),
                    Address = Faker.Address.Country() + "," + Faker.Address.UsState() + "," + Faker.Address.City() +
                              "," + Faker.Address.StreetSuffix() + "," + Faker.Address.ZipCode(),
                    Telephone = Faker.Phone.Number(),
                    Location = GetRandomLatLon()
                });
            }

            return defaultVendors;

        }

        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static Location GetRandomLatLon()
        {
            //this is around hall building
            double centerLat = 45.495393;
            double centerLng = -73.578862;

            //how far from center promotions will be generated
            double radius = 0.02;

            double lat = centerLat + (random.NextDouble() * radius);
            double lon = centerLng + (random.NextDouble() * radius);
            return new Location(lat, lon);
        }

        public static Categories GetRandomCategory()
        {
            Array values = Enum.GetValues(typeof(Categories));
            Categories category = (Categories) values.GetValue(random.Next(values.Length));
            return category;
        }

        public static DateTime GetRandomDate()
        {
            DateTime start = DateTime.Now;

            int randomDays = random.Next(30);

            return start.AddDays(randomDays);
        }

        public static bool GetRandomBoolean()
        {
            return random.NextDouble() >= 0.5;
        }

    }
}
