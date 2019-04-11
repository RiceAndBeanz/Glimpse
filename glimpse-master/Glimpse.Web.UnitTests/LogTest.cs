using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebServices;
using Serilog;
using System.IO;

namespace Glimpse.Web.UnitTests
{
    [TestClass]
    public class LogTest
    {
        [TestMethod]
        public void Logger_Is_Created()
        {
            Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Debug()
              .WriteTo.File(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\logs--.log", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}", shared: true)
              .CreateLogger();

            Assert.AreEqual(File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\logs--.log"), true);
        }
    }
}
