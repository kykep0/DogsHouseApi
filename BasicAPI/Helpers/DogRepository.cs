using BasicAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicAPI.Helpers
{
    public class DogRepository
    {
        private readonly ApiContext _context;

        public DogRepository(ApiContext context)
        {
            _context = context;
        }

        public async Task<List<Dog>> GetAllDogs()
        {
            return await _context.Dogs.ToListAsync();
        }

        public async Task<List<Dog>> GetDogs(string attribute, string order, int pageNumber, int limit)
        {
            var query = _context.Dogs.AsQueryable();

            switch (attribute)
            {
                case "name":
                    query = query.OrderBy(d => d.Name);
                    break;
                case "color":
                    query = query.OrderBy(d => d.Color);
                    break;
                case "tail_length":
                    query = query.OrderBy(d => d.TailLength);
                    break;
                case "weight":
                    query = query.OrderBy(d => d.Weight);
                    break;
                default:
                    break;
            }

            if (order == "desc")
            {
                query = query.Reverse();
            }

            return await query.Skip((pageNumber - 1) * limit).Take(limit).ToListAsync();
        }

        public async Task<List<Dog>> GetDogsByPage(int pageNumber, int limit)
        {
            return await _context.Dogs.Skip((pageNumber - 1) * limit).Take(limit).ToListAsync();
        }

        public async Task<List<Dog>> GetDogsSorted(string attribute, string order)
        {
            var query = _context.Dogs.AsQueryable();

            switch (attribute)
            {
                case "name":
                    query = query.OrderBy(d => d.Name);
                    break;
                case "color":
                    query = query.OrderBy(d => d.Color);
                    break;
                case "tail_length":
                    query = query.OrderBy(d => d.TailLength);
                    break;
                case "weight":
                    query = query.OrderBy(d => d.Weight);
                    break;
                default:
                    break;
            }

            if (order == "desc")
            {
                query = query.Reverse();
            }

            return await query.ToListAsync();
        }

        public async Task CreateDog(Dog dog)
        {
            _context.Dogs.Add(dog);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DoesDogExist(string name)
        {
            return await _context.Dogs.AnyAsync(d => d.Name == name);
        }
    }
}
