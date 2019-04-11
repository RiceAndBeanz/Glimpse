using Glimpse.Core.Model;
using Glimpse.Core.Model.CustomModels;
using Glimpse.Core.Services.General;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Core.UnitTests.Tests.Services
{
    [TestClass]
    public class GoogleWebServiceTest
    {

        private GoogleWebService gwb;

        [TestInitialize]
        public void Initialize()
        {
            gwb = new GoogleWebService();
        }

        [TestMethod]
        public async Task GetIndividualResponse_Returns_Valid()
        {
            //arrange
            Location origin = new Location(41.43206, -81.38992);
            Location destination = new Location(41.43217, -81.38985);

            string expected = "OK";

            //act
            DistanceMatrix actual = await gwb.GetIndividualDurationResponse(origin, destination);

            //assert
            

            Assert.IsTrue(expected.Equals(actual.status));

        }

        [TestMethod]
        public async Task GetMultipleResponse_Returns_Valid()
        {
            //arrange
            string expected = "OK";

            Location origin = new Location(41.43206, -81.38992);

            List<Location> listDestinations = new List<Location>();

            Location destination;
            for (int i = 0; i < 9; i++)
            {
                destination = new Location(41.43217, -81.38985);
                listDestinations.Add(destination);
            }
            destination = new Location(41.43600, -81.38985);
            listDestinations.Add(destination);

            //act
            DistanceMatrix actual = await gwb.GetMultipleDurationResponse(origin, listDestinations);

            //assert
            Assert.IsTrue(expected.Equals(actual.status));

        }



    }
}
