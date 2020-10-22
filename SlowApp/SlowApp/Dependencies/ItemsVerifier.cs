using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SlowApp.Entities;

namespace SlowApp.Dependencies
{
    public class ItemsVerifier
    {
        public virtual async Task<List<Item>> VerifyItemsAsync(List<Item> items)
        {
            var delay = TimeSpan.FromSeconds(new Random().Next(1, 8));
            await Task.Delay(delay);

            return items;
        }
    }
}
