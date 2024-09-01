using KaloriWebApplication.Models;
using KaloriWebApplication.Models.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using FuzzySharp;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;



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

        private static int ExtractValues(string line, bool isKcal)
        {
            if (isKcal)
            {
                var words = line.Split(' ');
                var bestMatch = words.OrderByDescending(words => Fuzz.PartialRatio(words.ToLower(), "kcal")).FirstOrDefault();
                var index = Array.IndexOf(words, bestMatch);
                var subArray = words.Take(index + 1).ToArray();
                var bestMatchNumber = subArray.LastOrDefault(target => target.Any(char.IsDigit));
                if (bestMatchNumber != null)
                {
                    var cleanedNumber = Int32.Parse(bestMatchNumber.Where(char.IsDigit).ToArray());
                    return cleanedNumber;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                var words = line.Split(' ');
                var bestMatch = words.OrderByDescending(words => Fuzz.PartialRatio(words.ToLower(), "kJ")).FirstOrDefault();
                var index = Array.IndexOf(words, bestMatch);
                var subArray = words.Take(index + 1).ToArray();
                var bestMatchNumber = subArray.LastOrDefault(target => target.Any(char.IsDigit)); //Things break if there's no matches...
                if (bestMatchNumber != null)
                {
                    var cleanedNumber = Int32.Parse(bestMatchNumber.Where(char.IsDigit).ToArray());
                    return cleanedNumber;
                }
                else
                {
                    return 0;
                }
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
                        Console.WriteLine("Image OCR via Azure Computer Vision...");
                        ComputerVisionClient client = Authenticate(_azureComputerVisionEndpoint, _azureComputerVisionKey);
                        p.OCRText = ReadFileUrl(client, location).Result;
                        Console.WriteLine(p.OCRText);
                        int kJScore = 0;
                        int kcalScore = 0;
                        int wordkJScore = 0;
                        int wordkcalScore = 0;
                        string bestkcalLine = "";
                        string bestkJLine = "";

                        foreach (var line in p.OCRText)
                        {
                            foreach (var word in line.Split(' '))
                            {
                                if (Fuzz.PartialRatio(word.ToLower(), "kj") > wordkJScore)
                                {
                                    wordkJScore = Fuzz.PartialRatio(word.ToLower(), "kj");
                                }
                                if (Fuzz.PartialRatio(word.ToLower(), "kcal") > wordkcalScore)
                                {
                                    wordkcalScore = Fuzz.PartialRatio(word.ToLower(), "kcal");
                                }
                            }

                            if (wordkJScore > kJScore)
                            {
                                kJScore = wordkJScore;
                                bestkJLine = line;
                            }

                            if (wordkcalScore > kcalScore)
                            {
                                kcalScore = wordkcalScore;
                                bestkcalLine = line;
                            }

                            wordkcalScore = 0;
                            wordkJScore = 0;
                        }

                        Console.WriteLine("Best KJ line: " + bestkJLine);
                        Console.WriteLine("Best kcal line: " + bestkcalLine);
                        Console.WriteLine("Extracted KJ: " + ExtractValues(bestkJLine, false));
                        Console.WriteLine("Extracted kcal: " + ExtractValues(bestkcalLine, true));
                        p.Calories = ExtractValues(bestkcalLine, true);


                        break;

                    default:
                        //Likely an error, but the form does require a radio button to be selected
                        break;

                }
            }

            ViewBag.SelectedRadio = p.SelectedRadio;
            ViewBag.OCRText = p.OCRText;
            ViewBag.Calories = p.Calories;
            return View(p);
        }

        //Calory updating for image recognition
        [HttpPost]
        public IActionResult SaveUserCalories(int totalCalories)
        {
            Console.WriteLine("Total calories: " + totalCalories);
            var userId = HttpContext.Session.GetInt32("UserID");
            Console.WriteLine("User ID: " + userId);
            if (userId == null)
            {
                TempData["ErrorMessage"] = "You need to be logged in to perform this action.";
                return RedirectToAction("Login", "Account");
            }

            if (totalCalories < 0 || totalCalories > 9999)
            {
                TempData["ErrorMessage"] = "Invalid calorie value.";
                return RedirectToAction("photoUpload");
            }

            var today = DateTime.Today;

            var existingCalory = _context.TotalCalories
                .FirstOrDefault(c => c.UserID == userId && c.CaloryDate == today);
            Console.WriteLine("Existing calory: " + existingCalory);

            if (existingCalory != null)
            {
                existingCalory.TotalCal += totalCalories;
                _context.TotalCalories.Update(existingCalory);
            }
            else
            {
                var newCalory = new TotalCalory
                {
                    UserID = userId.Value,
                    TotalCal = totalCalories,
                    CaloryDate = today
                };
                Console.WriteLine("New calory: " + newCalory);
                _context.TotalCalories.Add(newCalory); //inspect
            }

            var user = _context.Users.Find(userId);
            if (user != null && existingCalory != null)
            {
                int dailyCalorieLimit = user.DailyCalories ?? 0;

                if (existingCalory.TotalCal > dailyCalorieLimit)
                {
                    var notification = new notification
                    {
                        UserID = userId.Value,
                        notificationText = "You've exceeded your daily calorie limit.",
                        notificationDate = DateTime.Now,
                        isRead = 0
                    };
                    _context.notifications.Add(notification);
                    TempData["SuccessMessage"] = "Calories added successfully, but you've exceeded your daily calorie limit.";
                }
                else if (existingCalory.TotalCal > dailyCalorieLimit - 500)
                {
                    var notification = new notification
                    {
                        UserID = userId.Value,
                        notificationText = "You're close to exceeding your daily calorie limit.",
                        notificationDate = DateTime.Now,
                        isRead = 0
                    };
                    _context.notifications.Add(notification);
                    TempData["SuccessMessage"] = "Calories added successfully, but you're close to exceeding your daily calorie limit.";
                }
                else
                {
                    TempData["SuccessMessage"] = "Calories added successfully!";
                }
            }

            _context.SaveChanges();
            return RedirectToAction("photoUpload");
        }

    }
}