using Domain.CommonServices;
using Domain.DbContex;
using Domain.Entity;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class ProductAttributeService
    {
        private readonly ApplicationDbContext _context;

        public ProductAttributeService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<AttributteViewModel> GetAttributes(long companyId)
        {
            try
            {
                return _context.Attributtes.Where(w=>w.CompanyId==companyId)
                    .Select(a => new AttributteViewModel
                    {
                        AttributteId = a.AttributteId,
                        AttributteKey = a.AttributteKey,
                        AttributeName = a.AttributeName,
                        Status = a.Status,
                        LastModified = a.LastModified,
                        AttributeValues = _context.AttributteValues
                            .Where(v => v.AttributeId == a.AttributteId)
                            .Select(v => new AttributteValueViewModel
                            {
                                AttributeValueId = v.AttributeValueId,
                                AttributeId = v.AttributeId,
                                AttrbtValue = v.AttrbtValue,
                                Status = v.Status,
                                LastModified = v.LastModified
                            }).ToList()
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                // TODO: Log exception (e.g., ILogger)
                // Console.WriteLine($"Error in GetAttributes: {ex.Message}");
                return new List<AttributteViewModel>();
            }
        }

        public List<AttributteValueViewModel> GetAttributeValuesByAttributteId(int attributteId)
        {
            try
            {
                return _context.AttributteValues
                    .Where(v => v.AttributeId == attributteId)
                    .Select(v => new AttributteValueViewModel
                    {
                        AttributeValueId = v.AttributeValueId,
                        AttributeId = v.AttributeId,
                        AttrbtValue = v.AttrbtValue,
                        Status = v.Status,
                        LastModified = v.LastModified
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                // TODO: Log exception
                // Console.WriteLine($"Error in GetAttributeValuesByAttributteId: {ex.Message}");
                return null;
            }
        }
        public bool SaveAttributteWithDetails(AttributteWithDetailsViewModel model,long CompanyId)
        {
            try
            {
                var attribute = model.AttributteId == 0
                    ? new Attributte { AttributeName = model.AttributeName, Status = model.Status ?? "Active", LastModified = DateTime.UtcNow }
                    : _context.Attributtes.Find(model.AttributteId);

                if (attribute == null)
                    return false;
                attribute.CompanyId =CompanyId;

                if (model.AttributteId != 0)
                {
                    attribute.AttributeName = model.AttributeName;
                    attribute.Status = model.Status ?? "Active";
                    attribute.LastModified = DateTime.UtcNow;
                }
                else
                {
                    attribute.AttributteKey = Guid.NewGuid();
                    _context.Attributtes.Add(attribute);
                }

                _context.SaveChanges();

                // Load existing attribute values
                var existingValues = _context.AttributteValues
                    .Where(v => v.AttributeId == attribute.AttributteId)
                    .ToList();

                var incomingDetails = model.AttributteDetails?
                    .Select(d => d?.Trim())
                    .Where(d => !string.IsNullOrWhiteSpace(d))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList() ?? new List<string>();

                // 1. Add or update incoming values
                foreach (var detail in incomingDetails)
                {
                    var existingValue = existingValues
                        .FirstOrDefault(v => v.AttrbtValue.Trim().Equals(detail, StringComparison.OrdinalIgnoreCase));

                    if (existingValue != null)
                    {
                        existingValue.Status = "Active";
                        existingValue.LastModified = DateTime.UtcNow;
                    }
                    else
                    {
                        _context.AttributteValues.Add(new AttributteValue
                        {
                            AttributeId = attribute.AttributteId,
                            AttrbtValue = detail,
                            AttributeValueKey = Guid.NewGuid(),
                            Status = "Active",
                            LastModified = DateTime.UtcNow
                        });
                    }
                }

                // 2. Delete values not in the new list
                foreach (var existing in existingValues)
                {
                    if (!incomingDetails.Any(d => d.Equals(existing.AttrbtValue.Trim(), StringComparison.OrdinalIgnoreCase)))
                    {
                        _context.AttributteValues.Remove(existing); // Or use: existing.Status = "Deleted";
                    }
                }

                _context.SaveChanges();
                model.AttributteId = attribute.AttributteId;
                return true;
            }
            catch (Exception ex)
            {
                // TODO: Log exception
                return false;
            }
        }

        public bool DeleteAttribute(int id)
        {
            try
            {
                var attribute = _context.Attributtes.Find(id);
                if (attribute == null)
                    return false;

                var values = _context.AttributteValues.Where(v => v.AttributeId == id);
                _context.AttributteValues.RemoveRange(values);
                _context.Attributtes.Remove(attribute);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // TODO: Log exception
                // Console.WriteLine($"Error in DeleteAttribute: {ex.Message}");
                return false;
            }
        }
    }
}
