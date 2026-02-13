using demoapp.Models;
using demoapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace demoapp.Controllers
{
    [Authorize(Policy = "ClientOnly")]
    public class ViewCartController : Controller
    {
        private readonly ItemRepository _itemrepository;

        public ViewCartController(ItemRepository repository)
        {
            _itemrepository = repository;
        }
        public IActionResult Index()
        {
            var cartitem = HttpContext.Session.GetObject<List<ItemModelView>>("cart");
            return View(cartitem);
        }
        public Task<IActionResult> remove(int id)
        {
            var cart = HttpContext.Session.GetObject<List<ItemModelView>>("cart");
            if (cart != null)
            {
                cart.RemoveAll(x => x.Id == id);
                HttpContext.Session.SetObject<List<ItemModelView>>("cart", cart);
            }
            return Task.FromResult<IActionResult>(RedirectToAction("Index"));
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut()
        {
            var cart = HttpContext.Session.GetObject<List<ItemModelView>>("cart");
            bool checkoutresult = false;
            if (cart != null && cart.Count > 0)
            {
                checkoutresult = await _itemrepository.CheckoutProcess(cart);
            }
            HttpContext.Session.Clear();
            return await Task.FromResult<IActionResult>(RedirectToAction("index"));
            //return Ok(checkoutresult);
        }


    }
}
