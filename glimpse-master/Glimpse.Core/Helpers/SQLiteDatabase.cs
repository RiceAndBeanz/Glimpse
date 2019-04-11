using Glimpse.Core.Model;
using Newtonsoft.Json;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Core.Helpers
{
    public class SQLiteDatabase
    {
        public static SQLiteAsyncConnection GetConnection(string path, ISQLitePlatform sqlitePlatform)
        {
            var serializer = new BlobSerializerDelegate(
               obj =>
               {
                   if (obj is Location)
                   {
                       var e = (Location)obj;
                       var serializedString = string.Format("{0},{1}", e.Lat, e.Lng);       // subst your own serializer
                       return Encoding.UTF8.GetBytes(serializedString);
                   }

                   throw new InvalidOperationException(string.Format("Type {0} should not be requested.", obj.GetType()));
               },
               (d, t) =>
               {
                   if (t == typeof(Location))
                   {
                       string serializedString = Encoding.UTF8.GetString(d, 0, d.Length);                     
                       return new Location(serializedString);                       
                   }

                   throw new InvalidOperationException(string.Format("Type {0} should not be requested.", t));
               },
               t => true);


            var connectionFactory = new Func<SQLiteConnectionWithLock>(() => new SQLiteConnectionWithLock(sqlitePlatform, new SQLiteConnectionString(path, false, serializer)));
            return new SQLiteAsyncConnection(connectionFactory);
        }
    }
}
