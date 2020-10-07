using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
            IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(SD.NationalParkApiPath);
            TrailVM objVM = new TrailVM();
            objVM.nationalParkList = (IEnumerable<NationalPark>)npList.Select(n => new SelectListItem
            {
                Text = n.Name,
                Value = n.Id.ToString()
            });


            if (id == null)
            {
                return View(objVM);
            }

            objVM.Trail = await _trailRepo.GetAsync(SD.TrailsApiPath, id.GetValueOrDefault());
            if (objVM == null)
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
                    await _trailRepo.CreateAsync(SD.TrailsApiPath, objVM.Trail);
                }
                else
                {
                    await _trailRepo.UpdateAsync(SD.TrailsApiPath + objVM.Trail.Id, objVM.Trail);
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(objVM);
            }
        }

        public async Task<IActionResult> GetAllTrail()
        {
            return Json(new { data = await _trailRepo.GetAllAsync(SD.TrailsApiPath) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepo.DeleteAsync(SD.TrailsApiPath, id);
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete not Successful" });
        }
    }
}
