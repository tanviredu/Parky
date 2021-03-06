﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    [Authorize]
    public class NationalParkController : Controller
    {
        private readonly INationalparkRepository _npRepo;

        public NationalParkController(INationalparkRepository npRepo)
        {
            _npRepo = npRepo;
        }
        public IActionResult Index()
        {
            return View(new NationalPark() { });
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Upsert(int? id)
        {
            NationalPark obj = new NationalPark();
            if(id == null)
            {
                return View(obj);
            }

            obj = await _npRepo.GetAsync(SD.NationalParkApiPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWT"));
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
      
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upsert(NationalPark obj)
        {
            if (ModelState.IsValid)
            {

          
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                byte[] p1 = null;
                using var fs1 = files[0].OpenReadStream();
                using var ms1 = new MemoryStream();
                fs1.CopyTo(ms1);
                p1 = ms1.ToArray();

                obj.Picture = p1;
            }
            else
            {
                var objFromDb = await _npRepo.GetAsync(SD.NationalParkApiPath, obj.Id, HttpContext.Session.GetString("JWT"));
                    if(obj.Picture != null)
                    {
                        obj.Picture = objFromDb.Picture;
                    }
                
            }
            if(obj.Id == 0)
            {
                await _npRepo.CreateAsync(SD.NationalParkApiPath, obj, HttpContext.Session.GetString("JWT"));
            }
            else
            {
                await _npRepo.UpdateAsync(SD.NationalParkApiPath+obj.Id, obj, HttpContext.Session.GetString("JWT"));
            }

            return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(obj);
            }
        }

        public async Task<IActionResult> GetAllNationalPark()
        {
            return Json(new { data = await _npRepo.GetAllAsync(SD.NationalParkApiPath, HttpContext.Session.GetString("JWT")) });
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _npRepo.DeleteAsync(SD.NationalParkApiPath, id, HttpContext.Session.GetString("JWT"));
            if (status)
            {
                return Json(new { success = true, message="Delete Successful" });
            }
            return Json(new { success = false, message = "Delete not Successful" });
        }
    }
}
