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
using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.ComponentModel;
using System.Text;

namespace KaloriWebApplication.Controllers
{

    public class PhotoController : Controller
    {
        private readonly Context _context;

        private readonly string _modelPath = Path.Combine("wwwroot", "model.onnx");

        private readonly string _azureComputerVisionKey;
        private readonly string _azureComputerVisionEndpoint;

        public static ComputerVisionClient Authenticate(string endpoint, string key)
        {
            ComputerVisionClient client =
                new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
                { Endpoint = endpoint };
            return client;
        }

        public static async Task<List<string>> ReadFileUrl(ComputerVisionClient client, string filePath)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("READ FILE FROM DISK");
            Console.WriteLine();

            // Read text from file
            using (FileStream stream = System.IO.File.OpenRead(filePath))
            {
                var textHeaders = await client.ReadInStreamAsync(stream);
                // After the request, get the operation location (operation ID)
                string operationLocation = textHeaders.OperationLocation;
                Thread.Sleep(2000);

                // Retrieve the URI where the extracted text will be stored from the Operation-Location header.
                // We only need the ID and not the full URL
                const int numberOfCharsInOperationId = 36;
                string operationId = operationLocation.Substring(operationLocation.Length - numberOfCharsInOperationId);

                // Extract the text
                ReadOperationResult results;
                Console.WriteLine($"Extracting text from URL file {Path.GetFileName(filePath)}...");
                Console.WriteLine();
                do
                {
                    results = await client.GetReadResultAsync(Guid.Parse(operationId));
                }
                while ((results.Status == OperationStatusCodes.Running ||
                    results.Status == OperationStatusCodes.NotStarted));

                // Display the found text.
                Console.WriteLine();
                var textUrlFileResults = results.AnalyzeResult.ReadResults;
                List<string> lines = new List<string>();
                foreach (ReadResult page in textUrlFileResults)
                {
                    foreach (Line line in page.Lines)
                    {
                        lines.Add(line.Text);
                    }
                }
                return lines;
            }
        }

        public PhotoController(Context context, IConfiguration configuration)
        {
            _context = context;
            _azureComputerVisionKey = configuration["AzureComputerVision:SubscriptionKey"];
            _azureComputerVisionEndpoint = configuration["AzureComputerVision:Endpoint"];
        }

        private void ImagePreprocess() {
            Console.WriteLine("Preprocessing image...");
        }
        private void ImageRecognition()
        {
            //Image recognition stuff goes here
            Console.WriteLine("Image recognition is done");
        }
        private void TesseractOCR()
        {
            Console.WriteLine("Image OCR via Tesseract...");
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
                var newimagename = Guid.NewGuid() +extension;
                var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/NewFolder/", newimagename);
                var stream = new FileStream(location, FileMode.Create);
                p.image.CopyTo(stream);
                stream.Close();
                ViewBag.ImagePath = "/NewFolder/" + newimagename;
                
                switch (p.SelectedRadio)
                {
                    case "ImageRecognition":
                        ImageRecognition();
                        break;

                    case "AzureOCR":
                        Console.WriteLine("AzureOCR");
                        Console.WriteLine("Image OCR via Azure Computer Vision...");
                        ComputerVisionClient client = Authenticate(_azureComputerVisionEndpoint, _azureComputerVisionKey);
                        p.OCRText = ReadFileUrl(client, location).Result;
                        Console.WriteLine(p.OCRText);
                        break;

                    case "TesseractOCR":
                        TesseractOCR();
                        break;

                    default:
                        //Likely an error, but the form does require a radio button to be selected
                        break;

                }
            }

            ViewBag.SelectedRadio = p.SelectedRadio;
            ViewBag.OCRText = p.OCRText;
            return View(p);
        }

        

    }
}