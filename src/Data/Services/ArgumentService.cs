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
    public class ArgumentService
    {
        private readonly CommanderContext _context;

        public ArgumentService(CommanderContext context)
        {
            _context = context;
        }

        // Get argument by id
        public async Task<Argument> GetById(long id)
            => await _context.Arguments.FirstOrDefaultAsync(arg => arg.Id == id);

        // Create an Argument
        public async Task<Argument> Create(Argument argument)
        {
            if (argument == null)
                return null;

            await _context.Arguments.AddAsync(argument);
            await _context.SaveChangesAsync();
            return argument;
        }

        // Update an Argument
        public async Task<Argument> PartialUpdate(Argument argument)
        {
            if (argument == null)
                return null;

            if (!await _context.Arguments.AnyAsync(arg => arg.Id == argument.Id))
                return null;

            _context.Update(argument);
            await _context.SaveChangesAsync();
            return argument;
        }

        // Delete Argument
        public async Task<bool> Delete(long id)
        {
            var argument = await _context.Arguments.FirstOrDefaultAsync(arg => arg.Id == id);
            if (argument == null)
                return false;

            _context.Remove(argument);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
