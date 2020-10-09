using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalparkRepository _npRepo;
        private readonly ITrailRepository _trailRepo;
        private readonly IAccountRepository _accRepo;

        public HomeController(ILogger<HomeController> logger, INationalparkRepository npRepo, ITrailRepository trailRepo, IAccountRepository accRepo)
        {
            _logger = logger;
            _npRepo = npRepo;
            _trailRepo = trailRepo;
            _accRepo = accRepo;
        }

        public async Task<IActionResult> Index()
        {
            IndexVM listOfParksAndTrails = new IndexVM()
            {
                NationalParkList = await _npRepo.GetAllAsync(SD.NationalParkApiPath, HttpContext.Session.GetString("JWT")),
                TrailList = await _trailRepo.GetAllAsync(SD.TrailsApiPath, HttpContext.Session.GetString("JWT"))
            };
            return View(listOfParksAndTrails);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Login()
        {
            User user = new User();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            User obj = await _accRepo.LoginAsync(SD.AccountApiPath + "authenticate", user);

            if(obj.Token == null)
            {
                return View();
            }
            HttpContext.Session.SetString("JWT", obj.Token);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            bool result = await _accRepo.RegisterAsync(SD.AccountApiPath + "register/", user);

            if (result == false)
            {
                return View();
            }
            

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {

            HttpContext.Session.SetString("JWT", "");

            return RedirectToAction("Index");
        }
    }
}
