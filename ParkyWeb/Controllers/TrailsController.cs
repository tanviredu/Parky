using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    public class TrailsController : Controller
    {
        private readonly INationalparkRepository _npRepo;
        private readonly ITrailRepository _trailRepo;

        public TrailsController(INationalparkRepository npRepo, ITrailRepository trailRepo)
        {
            _npRepo = npRepo;
            _trailRepo = trailRepo;
        }
        public IActionResult Index()
        {
            return View(new Trail() { });
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(SD.NationalParkApiPath, HttpContext.Session.GetString("JWT"));
            TrailVM objVM = new TrailVM();
            objVM.nationalParkList = npList;
            objVM.Trail = new Trail();


            if (id == null)
            {
                return View(objVM);
            }

            objVM.Trail = await _trailRepo.GetAsync(SD.TrailsApiPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWT"));
            if (objVM.Trail == null)
            {
                return NotFound();
            }

            return View(objVM);

        }

        [HttpPost]
        public async Task<IActionResult> Upsert(TrailVM objVM)
        {
            if (ModelState.IsValid)
            {
                if (objVM.Trail.Id == 0)
                {
                    await _trailRepo.CreateAsync(SD.TrailsApiPath, objVM.Trail, HttpContext.Session.GetString("JWT"));
                }
                else
                {
                    await _trailRepo.UpdateAsync(SD.TrailsApiPath + objVM.Trail.Id, objVM.Trail, HttpContext.Session.GetString("JWT"));
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                objVM.nationalParkList = await _npRepo.GetAllAsync(SD.NationalParkApiPath, HttpContext.Session.GetString("JWT"));               
                return View(objVM);
            }
        }

        public async Task<IActionResult> GetAllTrail()
        {
            return Json(new { data = await _trailRepo.GetAllAsync(SD.TrailsApiPath, HttpContext.Session.GetString("JWT")) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepo.DeleteAsync(SD.TrailsApiPath, id, HttpContext.Session.GetString("JWT"));
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete not Successful" });
        }
    }
}
