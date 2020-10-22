using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SlowApp.Dependencies;

namespace SlowApp.Controllers
{
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ItemsProvider _itemsProvider;

        public ItemsController(ItemsProvider itemsProvider)
        {
            _itemsProvider = itemsProvider;
        }

        [HttpGet]
        [Route("/items")]
        public async Task<IActionResult> GetResponse()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var items = await _itemsProvider.GetItemsAsync();

            stopwatch.Stop();

            var response = new
            {
                items,
                meta = new
                {
                    waiting = $"Elapsed time: {stopwatch.ElapsedMilliseconds} ms"
                }
            };

            return Ok(response);
        }
    }
}
