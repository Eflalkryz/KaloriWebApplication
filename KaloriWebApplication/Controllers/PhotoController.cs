using Microsoft.AspNetCore.Mvc;
using KaloriWebApplication.Models.Concrete;
using System.Linq;
using KaloriWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace KaloriWebApplication.Controllers
{
    public class PhotoController : Controller
    {
        private readonly Context _context;

        private readonly string _modelPath = Path.Combine("wwwroot", "model.onnx");


        public PhotoController(Context context)
        {
            _context = context;
        }
        private void ImageRecognition()
        {
            //Image recognition stuff goes here
            Console.WriteLine("Image recognition is done");
        }

        private void ImageOCR()
        {
            Console.WriteLine("Image OCR is done");
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

            switch (p.SelectedRadio)
            {
                case "ImageRecognition":
                    ImageRecognition();
                    break;

                case "OCR":
                    ImageOCR();
                    break;

                default:
                    //Likely an error, but the form does require a radio button to be selected
                    break;

            }

            ViewBag.SelectedRadio = p.SelectedRadio;
            return View(p);
        }

        

    }
}