using Android.OS;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.File;
using System;
using System.IO;

namespace Glimpse.Core.Services.General
{
    public static class Log
    {
        public static VerboseLevel Verbose;
        private static readonly StreamWriter StreamWriter;
        private static readonly IMvxFileStore fileStore;

        public enum VerboseLevel
        {
            DEBUG = 0,
            INFO = 1,
            ERROR = 2,
            FATAL = 3,
            OFF = 4
        }

        static Log()
        {
            StreamWriter = new StreamWriter(new MemoryStream());
            fileStore = Mvx.Resolve<IMvxFileStore>();
        }

        public static void Debug(string format, params object[] args)
        {
            LogImpl(format, VerboseLevel.DEBUG, args);
        }

        public static void Info(string format, params object[] args)
        {
            LogImpl(format, VerboseLevel.INFO, args);
        }

        public static void Error(string format, params object[] args)
        {
            LogImpl(format, VerboseLevel.ERROR, args);
        }

        public static void Fatal(string format, params object[] args)
        {
            LogImpl(format, VerboseLevel.FATAL, args);
        }

        private static void LogImpl(string format, VerboseLevel level, params object[] args)
        {
            var message = string.Format("#" + level + " - " + format +"\n", args);
            StreamWriter.WriteLine(message);
            DumpLog();
            //fileStore.WriteFile("log.txt",message);
        }

        //Empties all logs into log.txt in android sd card.
        public static void DumpLog()
        {
            StreamWriter.Flush();
            StreamWriter.BaseStream.Position = 0;
            StreamReader reader = new StreamReader(StreamWriter.BaseStream);
            string text = reader.ReadToEnd();
            var fileStore = Mvx.Resolve<IMvxFileStore>();
            fileStore.WriteFile("log.txt",text);
        }
    }
}
