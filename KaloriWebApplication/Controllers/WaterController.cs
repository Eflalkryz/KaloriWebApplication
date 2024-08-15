using Microsoft.AspNetCore.Mvc;
using KaloriWebApplication.Models;
using KaloriWebApplication.Models.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Security.Claims;

namespace KaloriWebApplication.Controllers
{
    public class WaterController: Controller
    {
        private readonly Context _context;

        public WaterController(Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}