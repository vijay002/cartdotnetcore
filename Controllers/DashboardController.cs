using demoapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace demoapp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ItemRepository _itemRepository;
        public DashboardController(ItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<IActionResult> Index()
        {
            string path = Path.Combine("images");
            var items = await _itemRepository.GetItemList(path);
            return View(items.ToList());
            
        }
    }
}
