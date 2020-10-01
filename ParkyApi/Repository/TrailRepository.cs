using Microsoft.EntityFrameworkCore;
using ParkyApi.Data;
using ParkyApi.Models;
using ParkyApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Repository
{
    public class TrailRepositoryRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _db;

        public TrailRepositoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreateTrail(Trail trail)
        {
            _db.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Trails.Remove(trail);
            return Save();
        }

        public Trail GetTrail(int trailId)
        {
           return  _db.Trails.Include(t => t.NationalPark).FirstOrDefault(t=>t.Id == trailId);
        }

        public ICollection<Trail> GetTrails()
        {
            return _db.Trails.Include(t => t.NationalPark).OrderBy(a => a.Name).ToList();
        }

        public bool TrailExists(string name)
        {
            return _db.Trails.Any(n => n.Name.ToLower().Trim() == name.ToLower().Trim()); 
        }

        public bool TrailExists(int id)
        {
            return _db.Trails.Any(n => n.Id == id);
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false; 
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trails.Update(trail);
            return Save();
        }

        public ICollection<Trail> GetTrailsInNationalPark(int npId)
        {
            return _db.Trails.Include(t => t.NationalPark).Where(t => t.NationalParkId == npId).ToList();
        }
    }
}
