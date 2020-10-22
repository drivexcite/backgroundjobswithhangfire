using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SlowApp.Core.Entities;

namespace SlowApp.Core.Dependencies
{
    public class ItemsLogger
    {
        public virtual async Task VerifyItemsAsync(List<Item> items)
        {
            var delay = TimeSpan.FromSeconds(new Random().Next(1, 8));
            await Task.Delay(delay);
        }
    }
}
