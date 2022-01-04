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
    public class CommandService
    {
        private readonly CommanderContext _context;

        public CommandService(CommanderContext context)
        {
            _context = context;
        }

        // Get All the commands with Arguments
        public async Task<List<Command>> All(long? platformId = null, uint page = 0, uint size = 15)
        {
            IQueryable<Command>  results = _context.Commands;

            if (platformId.HasValue)
            {
                results = results.Where(c => c.PlatformId == platformId.Value);
            }

            if (page > 0)
            {
                size = size > 0 ? size : 15;
                results = results.Skip((int)(size * (page - 1))).Take((int)size);
            }

            return await results.Include(c => c.Arguments).Include(c => c.Platform).ToListAsync();
        }

        // Get Command by Id with Arguments
        public async Task<Command> GetById(long id) 
            => await _context.Commands.Include(c => c.Arguments).Include(c => c.Platform).FirstOrDefaultAsync(c => c.Id == id);

        // Create a command with associated Arguments
        public async Task<Command> Create(Command command)
        {
            if (command == null)
                return null;

            _context.Commands.Add(command);
            await _context.SaveChangesAsync();
            return command;
        }

        // Update Command
        public async Task<Command> PartialUpdate(Command command)
        {
            if (command == null)
                return null;

            if (!await _context.Commands.AnyAsync(c => c.Id == command.Id))
                return null;

            _context.Update(command);
            await _context.SaveChangesAsync();
            return command;
        }

        // Delete Command
        public async Task<bool> Delete(long id)
        {
            var command = await GetById(id);
            if (command == null)
                return false;

            _context.Remove(command);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
