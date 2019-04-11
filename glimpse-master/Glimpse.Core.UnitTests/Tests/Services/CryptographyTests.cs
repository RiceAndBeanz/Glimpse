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
    public class CryptographyTest
    {

        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void CreateSalt_Returns_Random()
        {
            //arrange
            Byte[] salt1;
            Byte[] salt2;

            //act
            salt1 = Cryptography.CreateSalt();
            salt2 = Cryptography.CreateSalt();

            //assert
            Assert.IsFalse(salt1.SequenceEqual(salt2));            
        }

        [TestMethod]
        public void CreateDerivedKey_CorrectLength()
        {
            //arrange
            string passwordToUse = "password";
            byte[] salt = new byte[8]{65, 43, 34, 23, 45, 34, 64, 53};
            int keyLengthInBytes = 32;

            //act
            byte[] key = Cryptography.CreateDerivedKey(passwordToUse, salt, keyLengthInBytes);

            //assert
            Assert.AreEqual(keyLengthInBytes, key.Length);
        }

        [TestMethod]
        public void HashPassword_SamePasswordIsDifferentHash()
        {
            //arrange
            string passwordToUse = "password";

            //act
            //the same password encrypted twice should not be equal
            var firstActual = Cryptography.HashPassword(passwordToUse);
            var secondActual = Cryptography.HashPassword(passwordToUse);

            //assert
            Assert.AreNotEqual(firstActual.Item1, secondActual.Item1);
            Assert.AreNotEqual(firstActual.Item2, secondActual.Item2);
        }

        [TestMethod]
        public void HashPassword_HashingWithSaltReturnsCorrectHash()
        {
            //arrange
            string passwordToUse = "password";

            //act
            //the same password encrypted twice should not be equal
            var firstActual = Cryptography.HashPassword(passwordToUse);

            string resultWithSalt = Cryptography.HashPassword(passwordToUse, firstActual.Item2);

            //assert
            Assert.AreEqual(firstActual.Item1, resultWithSalt);
           
        }
    }
}
