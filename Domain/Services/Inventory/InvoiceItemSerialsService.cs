using Domain.DbContex;
using Domain.Entity.Inventory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.Inventory
{
    public class InvoiceItemSerialsService
    {
        private readonly ApplicationDbContext _context;

        public InvoiceItemSerialsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<InvoiceItemSerials>> GetAllAsync()
        {
            return await _context.InvoiceItemSerials.ToListAsync();
        }
        public async Task<List<InvoiceItemSerials>> GetByInvoiceItemId(long invoiceItemId)
        {
            return await _context.InvoiceItemSerials
                   .Where(w=>w.InvoiceItemId==invoiceItemId)
                   .ToListAsync();
        }

        public async Task<InvoiceItemSerials?> GetByIdAsync(long id)
        {
            return await _context.InvoiceItemSerials.FindAsync(id);
        }

        public async Task AddAsync(InvoiceItemSerials item)
        {
            _context.InvoiceItemSerials.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(InvoiceItemSerials item)
        {
            _context.InvoiceItemSerials.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var item = await _context.InvoiceItemSerials.FindAsync(id);
            if (item != null)
            {
                _context.InvoiceItemSerials.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }

}
