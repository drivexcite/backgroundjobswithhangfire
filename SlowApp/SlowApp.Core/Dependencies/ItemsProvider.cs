using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SlowApp.Core.Entities;

namespace SlowApp.Core.Dependencies
{
    public class ItemsProvider
    {
        private readonly SlowAppDbContext _db;
        private readonly ItemsLogger _logger;

        public ItemsProvider(SlowAppDbContext db, ItemsLogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public virtual async Task<List<Item>> GetItemsAsync()
        {
            var availableItems = await (from i in _db.Items select i).ToListAsync();
            await _logger.ProcessItemsAsync(availableItems);

            return availableItems;
        }
    }
}
