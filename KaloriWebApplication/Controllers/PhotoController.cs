using Microsoft.AspNetCore.Mvc;
using KaloriWebApplication.Models.Concrete;
using System.Linq;
using KaloriWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace KaloriWebApplication.Controllers
{
    public class PhotoController : Controller
    {
        private readonly Context _context;

        public PhotoController(Context context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult photoUpload()
        {



            return View();
        }

        [HttpPost]
        public IActionResult photoUpload(fotoUpload p)
        {

            if(p.image != null)
            {
                var extension = Path.GetExtension(p.image.FileName);
                var newimagename =Guid.NewGuid() +extension;
                var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/NewFolder/", newimagename);
                var stream = new FileStream(location, FileMode.Create);
                p.image.CopyTo(stream);
                ViewBag.ImagePath = "/NewFolder/" + newimagename;
            }
            
            return View(p);
        }


    }
}