using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SlowApp.Entities;

namespace SlowApp.Dependencies
{
    public class ItemsProvider
    {
        private readonly ItemsVerifier _verifier;
        private readonly SlowAppDbContext _db;

        public ItemsProvider(ItemsVerifier verifier, SlowAppDbContext db)
        {
            _verifier = verifier;
            _db = db;
        }

        public virtual async Task<List<Item>> GetItemsAsync()
        {
            var availableItems = await (from i in _db.Items select i).ToListAsync();
            return await _verifier.VerifyItemsAsync(availableItems);
        }
    }
}
