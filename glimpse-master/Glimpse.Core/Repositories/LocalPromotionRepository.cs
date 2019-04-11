using Glimpse.Core.Contracts.Repository;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glimpse.Core.Model;
using SQLite.Net.Interop;
using SQLite.Net.Async;
using Glimpse.Core.Helpers;

namespace Glimpse.Core.Repositories
{
    public class LocalPromotionRepository : ILocalPromotionRepository
    {
        private SQLiteAsyncConnection _connection;

        public LocalPromotionRepository()
		{
        }

        public async Task InitializeAsync(string path, ISQLitePlatform sqlitePlatform)
        {
            _connection = SQLiteDatabase.GetConnection(path, sqlitePlatform);
            // Create MyEntity table if need be
            await _connection.CreateTableAsync<PromotionWithLocation>();
        }

        public async Task<PromotionWithLocation> Insert(PromotionWithLocation promotionWithLocation)
        {
            var count = await _connection.InsertAsync(promotionWithLocation);
            return (count == 1) ? promotionWithLocation : null;
        }


        public async Task<PromotionWithLocation> Delete(PromotionWithLocation promotionWithLocation)
        {
            var count = await _connection.DeleteAsync(promotionWithLocation);
            return (count == 1) ? promotionWithLocation : null;
        }

        public async Task<List<PromotionWithLocation>> GetPromotions()
        {
            var entities = await _connection.Table<PromotionWithLocation>().ToListAsync();
            return entities;
        }

        public async Task<List<PromotionWithLocation>> GetActivePromotions()
        {
           // var entities = await _connection.Table<PromotionWithLocation>().ToListAsync();
            var entities = await GetPromotions();
            return entities.Where(e => e.PromotionStartDate.CompareTo(DateTime.Now) <= 0 && e.PromotionEndDate.CompareTo(DateTime.Now) >= 0).GroupBy(p => p.PromotionId).Select(p =>  p.First()).Distinct().ToList();
        }

        public async Task<List<PromotionWithLocation>> SearchActivePromotions(string keyword)
        {
            // var entities = await _connection.Table<PromotionWithLocation>().ToListAsync();
            var entities = await GetActivePromotions();
            return entities.Where(promo => promo.Title.Contains(keyword) || promo.Description.Contains(keyword)).ToList();
        }

    }
}
