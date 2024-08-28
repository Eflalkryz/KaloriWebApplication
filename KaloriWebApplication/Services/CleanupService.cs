using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;


namespace KaloriWebApplication.Services
{
    public class CleanupService : IHostedService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string PlaceholderName = "placeholder";

        public CleanupService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting cleanup.");
            EnsureImageUploads();
            //You can't clear up what does not exist
            ClearImageUploads();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Carrying out shutdown tasks.");
            //Hopefully someone will not have deleted the folder externally by then.
            ClearImageUploads();
            return Task.CompletedTask;
        }

        private void EnsureImageUploads()
        {
            var path = Path.Combine(_env.WebRootPath, "image_uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var placeholderPath = Path.Combine(path, PlaceholderName);
            if (!File.Exists(placeholderPath))
            {
                File.WriteAllText(placeholderPath, "This is a placeholder file. Please leave it alone.");
            }
        }

        private void ClearImageUploads()
        {
            var path = Path.Combine(_env.WebRootPath, "image_uploads");
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                if (Path.GetFileName(file) != PlaceholderName)
                {
                    File.Delete(file);
                }
            }
        }
    }
}
