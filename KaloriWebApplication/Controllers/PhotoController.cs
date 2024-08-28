using KaloriWebApplication.Models;
using KaloriWebApplication.Models.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;



namespace KaloriWebApplication.Controllers
{

    public class PhotoController : Controller
    {
        private readonly Context _context;

        private readonly string _modelPath;

        private readonly string _azureComputerVisionKey;
        private readonly string _azureComputerVisionEndpoint;
        // Use _env.WebRootPath to get the path to the wwwroot folder, do not hardcode it
        private readonly IWebHostEnvironment _env;

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

                var textUrlFileResults = results.AnalyzeResult.ReadResults;
                List<string> lines = new List<string>();
                //Return each line as a list of lines
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

        public PhotoController(Context context, IConfiguration configuration, IWebHostEnvironment env)
        {
            _context = context;
            _azureComputerVisionKey = configuration["AzureComputerVision:SubscriptionKey"];
            _azureComputerVisionEndpoint = configuration["AzureComputerVision:Endpoint"];
            _env = env;
            _modelPath = Path.Combine(_env.WebRootPath, "model.onnx");
        }
        private void ImageRecognition()
        {
            //Image recognition stuff goes here
            Console.WriteLine("Image recognition is done");
        }


        [HttpGet]
        public IActionResult photoUpload()
        {

            return View();
        }

        [HttpPost]
        public IActionResult photoUpload(fotoUpload p)
        {

            if (p.image != null)
            {
                var extension = Path.GetExtension(p.image.FileName);
                var newimagename = Guid.NewGuid() + extension;
                var location = Path.Combine(_env.WebRootPath, "image_uploads", newimagename);
                var stream = new FileStream(location, FileMode.Create);
                p.image.CopyTo(stream);
                stream.Close();
                ViewBag.ImagePath = "/image_uploads/" + newimagename;

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