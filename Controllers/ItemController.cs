using demoapp.Models;
using demoapp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace demoapp.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class ItemController : Controller
    {
        private readonly ItemRepository _itemrepository;
        private readonly IHostEnvironment _host;
        public ItemController(ItemRepository itemrepository, IHostEnvironment host)
        {
            _itemrepository = itemrepository;
            _host = host;
        }

        public async Task<IActionResult> Index()
        {
            string path = _host.ContentRootPath;
            //string filePath = Path.Combine("wwwroot/images", "uploads") +@"/";
            string filePath = Path.Combine("/images");
            var items = await _itemrepository.GetItemList(filePath);
            return View(items);
        }

        [HttpGet]
        public IActionResult CreateItem(int? id)
        {
            ItemCreateModelView model = new ItemCreateModelView();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateItem(ItemCreateModelView model)
        {
            try
            {
                string path = _host.ContentRootPath;
                string filePath = Path.Combine("wwwroot/images");
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (model.Id >  0 && model.UploadedFile == null)
                {
                    ModelState.Remove("UploadedFile");
                }
                

                if (ModelState.IsValid)
                {
                    string newfilename = model.FileName??"";
                    if (model.UploadedFile != null && model.UploadedFile.Length > 0)
                    {
                        //using (var filestream = model.UploadedFile.OpenReadStream())
                        //{
                        //    MemoryStream memoryStream = filestream as MemoryStream;
                        //    if(memoryStream == null)
                        //        filestream.CopyTo(memoryStream);

                        //    byte[] bytes = memoryStream.ToArray();
                        //    System.IO.File.WriteAllBytes(filePath + @"/" +Guid.NewGuid().ToString(), bytes);
                        //}

                        var sfilename = model.UploadedFile.FileName;
                        var extenstion = Path.GetExtension(sfilename);
                        newfilename = Guid.NewGuid().ToString() + Path.GetExtension(sfilename);

                        using (FileStream fs = new FileStream(filePath + @"/" + newfilename, FileMode.Create))
                        {
                            await model.UploadedFile.CopyToAsync(fs);
                        }
                    }
                    var entity = new ItemCreateModelView()
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Description = model.Description,
                        Category = model.Category,
                        FileName = newfilename,
                        Quantity = model.Quantity,
                        Price = model.Price,
                        createdby = userId
                    };

                    _itemrepository.InsertItem(entity);
                    return RedirectToAction("Index");//successfull item listed
                    
                }
                else 
                    return View("CreateItem", model);


            }
            catch (Exception ex)
            {
                //throw ex;
                return View("CreateItem", model);
            }
        }


        public async Task<IActionResult> edit(int id)
        {
            var item = await _itemrepository.GetItemById(id);
            return View("CreateItem", item);

        }
        public async Task<IActionResult> delete(int id)
        {
            var affected = await _itemrepository.DeletebyId(id);

            return RedirectToAction("index");
        }



    }
}
