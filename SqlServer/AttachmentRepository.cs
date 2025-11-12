using CoreEntities.Model;
using LogicBusiness.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlowBE.Data;

namespace SqlServer
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly ApplicationDbContext _context;

        public AttachmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Attachment>> GetAllAsync()
        {
            return await _context.Attachments.ToListAsync();
        }

        public async Task<IEnumerable<Attachment>> GetByTaskIdAsync(string taskId)
        {
            return await _context.Attachments
                .Where(a => a.TaskId == taskId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Attachment>> GetByCommentIdAsync(string commentId)
        {
            return await _context.Attachments
                .Where(a => a.CommentId == commentId)
                .ToListAsync();
        }

        public async Task<Attachment> GetByIdAsync(string id)
        {
            return await _context.Attachments.FindAsync(id);
        }

        public async Task AddAsync(Attachment attachment)
        {
            await _context.Attachments.AddAsync(attachment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var file = await _context.Attachments.FindAsync(id);
            if (file != null)
            {
                _context.Attachments.Remove(file);
                await _context.SaveChangesAsync();
            }
        }
    }
}
