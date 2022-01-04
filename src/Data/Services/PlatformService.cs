using Data.Context;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class PlatformService
    {
        private readonly CommanderContext _context;

        public PlatformService(CommanderContext context)
        {
            _context = context; 
        }

        // Add Platform to the database
        public async Task<Platform> Create(Platform platform)
        {
            if (platform == null)
                return null;

            await _context.AddAsync(platform);
            await _context.SaveChangesAsync();
            return platform;
        }

        // Get All Platforms from databse
        public async Task<List<Platform>> All(uint page = 0, uint size = 15)
        {
            IQueryable<Platform> results = _context.Platforms;

            if (page > 0)
            {
                size = size > 0 ? size : 15;
                results = results.Skip((int)(size * (page - 1))).Take((int)size);
            }

            return await results.ToListAsync();
        }

        // Get Platform by Id
        // For Testing Purposes
        public async Task<Platform> GetById(long id) 
            => await _context.Platforms.FirstOrDefaultAsync(p => p.Id == id);

        // Update a Platform
        public async Task<Platform> PartialUpdate(Platform platform)
        {
            if (platform == null)
                return null;

            if (!await _context.Platforms.AnyAsync(p => p.Id == platform.Id))
                return null;

            _context.Update(platform);
            await _context.SaveChangesAsync();
            return platform;
        }

        // Delete a Platform
        public async Task<bool> Delete(long id)
        {
            var platform = await GetById(id);
            if (platform == null)
                return false;

            _context.Remove(platform);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
