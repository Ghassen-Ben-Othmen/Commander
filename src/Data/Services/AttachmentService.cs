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
    public class AttachmentService
    {
        private readonly CommanderContext _context;

        public AttachmentService(CommanderContext context)
        {
            _context = context;
        }

        // Get By CommandId
        public async Task<List<Attachment>> GetByCommandId(long commandId)
            => await _context.Attachments.Where(att => att.CommandId == commandId).ToListAsync();

        // Create Attachment
        public async Task<Attachment> Create(Attachment attachment)
        {
            if (attachment == null)
                return null;

            await _context.Attachments.AddAsync(attachment);
            await _context.SaveChangesAsync();
            return attachment;
        }

        // Delete Attachment
        public async Task<bool> Delete(long id)
        {
            var attachment = await _context.Attachments.FirstOrDefaultAsync(att => att.Id == id);
            if (attachment == null)
                return false;

            _context.Attachments.Remove(attachment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
