using Azure;
using Domain.DbContex;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class ProductCategoryTypeService
    {
        private readonly ApplicationDbContext _context;

        public ProductCategoryTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddOrUpdateAsync(long companyId, long productId, List<string> ProductCategoryTypeIds)
        {
            try
            {
                if (ProductCategoryTypeIds?.Any() == true)
                {
                    var incomingCodes = ProductCategoryTypeIds;
                    var existingEntries = await _context.ProductCategoryTypes
                        .Where(x => x.ProductId == productId && x.CompanyId == companyId)
                        .ToListAsync();

                    // Add or update
                    foreach (var code in incomingCodes)
                    {
                        var existing = existingEntries.FirstOrDefault(x => x.CategoryTypeCode == code);
                        if (existing == null)
                        {
                            _context.ProductCategoryTypes.Add(new ProductCategoryTypes
                            {
                                CategoryTypeCode = code,
                                ProductId = productId,
                                CompanyId = companyId
                            });
                        }
                        // Optionally update existing fields if needed
                    }

                    // Delete items not in incoming list
                    var toDelete = existingEntries
                        .Where(x => !incomingCodes.Contains(x.CategoryTypeCode))
                        .ToList();

                    if (toDelete.Any())
                        _context.ProductCategoryTypes.RemoveRange(toDelete);

                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                // Optionally log the exception: ex.Message
                return false;
            }
        }


        public async Task<ProductCategoryTypes?> GetByIdAsync(int id)
        {
            return await _context.ProductCategoryTypes.FindAsync(id);
        }

        public async Task<List<ProductCategoryTypes>> GetAllAsync()
        {
            return await _context.ProductCategoryTypes.ToListAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.ProductCategoryTypes.FindAsync(id);
            if (entity == null) return false;

            _context.ProductCategoryTypes.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
