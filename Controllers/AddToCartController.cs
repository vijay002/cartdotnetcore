using demoapp.Models;
using demoapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace demoapp.Controllers
{
    [Authorize(Policy = "ClientOnly")]
    public class AddToCartController : Controller
    {
        private readonly ItemRepository _itemRepository;
        public AddToCartController(ItemRepository itemRepository)
        {
            _itemRepository = itemRepository;

        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            //HttpContext.Session.GetObject<List<ItemModelView>>
            var cart = HttpContext.Session.GetObject<List<ItemModelView>>("cart") ?? new List<ItemModelView>();

            var path = Path.Combine("/images");
            string filePath = Path.Combine("wwwroot/images");
            var products = await _itemRepository.GetItemById(id);
            var item = cart.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                //var selectedid =  item.Where(x => x.Id == id);
                cart.Add(new ItemModelView
                {
                    Id = products.Id,
                    Name = products.Name,
                    Price = products.Price,
                    Quantity = 1,
                    ImagePath = path + @"/" + products.FileName
                });
                //var a = new { selectedid };
            }
            else
            {
                item.Quantity++;
            }
            HttpContext.Session.SetObject("cart", cart);
            return RedirectToAction("Index", "Dashboard");
            //return new EmptyResult();
            //return Json(new { sucess = true });
        }

        public async Task<IActionResult> RemovefromCart(int id)
        {
            var cart = HttpContext.Session.GetObject<List<ItemModelView>>("cart");
            cart.RemoveAll(x => x.Id == id);
            HttpContext.Session.SetObject("cart", cart);
            return RedirectToAction("Index", "Dashboard");
        }

        public async Task<IActionResult> ViewCart()
        {
            var cart = HttpContext.Session.GetObject<List<ItemModelView>>("cart");
            return View(cart);
        }
    }
}
