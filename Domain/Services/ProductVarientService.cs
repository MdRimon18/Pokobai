using Domain.DbContex;
using Domain.Entity;
using Domain.Entity.Settings;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class ProductVarientService
    {
        private readonly ApplicationDbContext _context;

        public ProductVarientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductVariants>> ProductVarients()
        {
            try
            {
                return await _context.ProductVariants
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // TODO: Log exception
                // Console.WriteLine($"Error in GetAttributeValues: {ex.Message}");
                return null;
            }
        }
        public async Task<List<ProductVariants>> ProductVarientsByProductId(long productId)
        {
            try
            {
                return await _context.ProductVariants.Where(w=>w.ProductId== productId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // TODO: Log exception
                // Console.WriteLine($"Error in GetAttributeValues: {ex.Message}");
                return null;
            }
        }
        public List<AttributteViewModel> GetAttributes()
        {
            try
            {
                return _context.Attributtes
                    .Select(a => new AttributteViewModel
                    {
                        AttributteId = a.AttributteId,
                        AttributteKey = a.AttributteKey,
                        AttributeName = a.AttributeName,
                        Status = a.Status,
                        LastModified = a.LastModified
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                // TODO: Log exception (e.g., using ILogger)
                // Console.WriteLine($"Error in GetAttributes: {ex.Message}");
                return null;
            }
        }
        public List<AttributteValue> GetAttributeValues()
        {
            try
            {
                return _context.AttributteValues
                    .ToList();
            }
            catch (Exception ex)
            {
                // TODO: Log exception
                // Console.WriteLine($"Error in GetAttributeValues: {ex.Message}");
                return null;
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
        public bool SaveAttributteWithDetails(AttributteWithDetailsViewModel model)
        {
            try
            {
                var attribute = model.AttributteId == 0
                    ? new Attributte { AttributeName = model.AttributeName, Status = model.Status ?? "Active", LastModified = DateTime.UtcNow }
                    : _context.Attributtes.Find(model.AttributteId);

                if (attribute == null)
                    return false;

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

                // Process provided details
                if (model.AttributteDetails != null && model.AttributteDetails.Any())
                {
                    var incomingDetails = model.AttributteDetails
                        .Select(d => d?.Trim().ToLowerInvariant())
                        .Where(d => !string.IsNullOrEmpty(d))
                        .Distinct()
                        .ToList();

                    // Update existing values or add new ones
                    foreach (var detail in incomingDetails)
                    {
                        var existingValue = existingValues
                            .FirstOrDefault(v => v.AttrbtValue.Trim().ToLowerInvariant() == detail);

                        if (existingValue != null)
                        {
                            // Update existing value
                            existingValue.Status = "Active";
                            existingValue.LastModified = DateTime.UtcNow;
                        }
                        else
                        {
                            // Add new value
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
                }

                _context.SaveChanges();
                model.AttributteId = attribute.AttributteId;
                return true;
            }
            catch (Exception ex)
            {
                // TODO: Log exception
                // Console.WriteLine($"Error in SaveAttributteWithDetails: {ex.Message}");
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
        // Products
        //public async Task<List<Products>> GetAllProductsAsync()
        //{
        //    try
        //    {
        //        return await _context.Products
        //            .Include(p => p.Variants)
        //            .ThenInclude(v => v.ProductVariantAttributes)
        //            .ThenInclude(va => va.AttributeValue)
        //            .ThenInclude(av => av.Attributte)
        //            .Where(p => p.Status == "Active")
        //            .ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Failed to retrieve products: " + ex.Message, ex);
        //    }
        //}

        //public async Task<Products> GetProductByIdAsync(long id)
        //{
        //    try
        //    {
        //        var product = await _context.Products
        //            .Include(p => p.Variants)
        //            .ThenInclude(v => v.ProductVariantAttributes)
        //            .ThenInclude(va => va.AttributeValue)
        //            .ThenInclude(av => av.Attributte)
        //            .FirstOrDefaultAsync(p => p.ProductId == id && p.Status == "Active");

        //        if (product == null)
        //            throw new Exception($"Product with ID {id} not found or inactive.");

        //        return product;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Failed to retrieve product with ID {id}: " + ex.Message, ex);
        //    }
        //}

        //public async Task AddProductAsync(Products product)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(product.Name) || product.BasePrice < 0)
        //            throw new ArgumentException("Invalid product data: Name is required and BasePrice must be non-negative.");

        //        product.Status = "Active";
        //        product.LastModified = DateTime.UtcNow;
        //        _context.Products.Add(product);
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Failed to add product: " + ex.Message, ex);
        //    }
        //}

        // Product Variants
        public async Task<ProductVariants> GetVariantByIdAsync(long variantId)
        {
            try
            {
                var variant = await _context.ProductVariants
                    .Include(v => v.ProductVariantAttributes)
                    .ThenInclude(va => va.AttributeValue)
                    .ThenInclude(av => av.Attributte)
                    .FirstOrDefaultAsync(v => v.ProductVariantId == variantId && v.Status == "Active");

                if (variant == null)
                    throw new Exception($"Variant with ID {variantId} not found or inactive.");

                return variant;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve variant with ID {variantId}: " + ex.Message, ex);
            }
        }

        //public async Task AddVariantAsync(long productId, ProductVariants variant, List<int> attributeValueIds)
        //{
        //    try
        //    {
        //        var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId && p.Status == "Active");
        //        if (product == null)
        //            throw new Exception($"Product with ID {productId} not found or inactive.");

        //        if (variant.PriceAdjustment == null || variant.Position < 0 || string.IsNullOrEmpty(variant.Status))
        //            throw new ArgumentException("Invalid variant data: PriceAdjustment, Position, and Status are required.");

        //        variant.ProductId = productId;
        //        variant.Status = "Active";
        //        variant.LastModified = DateTime.UtcNow;
        //        _context.ProductVariants.Add(variant);
        //        await _context.SaveChangesAsync();

        //        foreach (var attributeValueId in attributeValueIds)
        //        {
        //            var attributeValue = await _context.AttributteValues
        //                .Include(av => av.Attributte)
        //                .FirstOrDefaultAsync(av => av.AttributeValueId == attributeValueId && av.Status == "Active");
        //            if (attributeValue != null)
        //            {
        //                _context.ProductVariantAttributes.Add(new ProductVariantAttribute
        //                {
        //                    ProductVariantId = variant.ProductVariantId,
        //                    AttributeValueId = attributeValueId,
        //                    AttributeId = attributeValue.AttributeId,
        //                    Status = "Active",
        //                    LastModified = DateTime.UtcNow
        //                });
        //            }
        //        }
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Failed to add variant: " + ex.Message, ex);
        //    }
        //}

        // Attributes
        public async Task<List<Attributte>> GetAllAttributesAsync()
        {
            try
            {
                return await _context.Attributtes
                    .Include(a => a.AttributeValues)
                    .Where(a => a.Status == "Active")
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve attributes: " + ex.Message, ex);
            }
        }

        public async Task AddAttributeAsync(Attributte attribute)
        {
            try
            {
                if (string.IsNullOrEmpty(attribute.AttributeName))
                    throw new ArgumentException("Attribute name is required.");

                attribute.Status = "Active";
                attribute.LastModified = DateTime.UtcNow;
                _context.Attributtes.Add(attribute);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add attribute: " + ex.Message, ex);
            }
        }

        public async Task AddAttributeValueAsync(int attributeId, AttributteValue attributeValue)
        {
            try
            {
                var attribute = await _context.Attributtes.FirstOrDefaultAsync(a => a.AttributteId == attributeId && a.Status == "Active");
                if (attribute == null)
                    throw new Exception($"Attribute with ID {attributeId} not found or inactive.");

                if (string.IsNullOrEmpty(attributeValue.AttrbtValue))
                    throw new ArgumentException("Attribute value is required.");

                attributeValue.AttributeId = attributeId;
                attributeValue.Status = "Active";
                attributeValue.LastModified = DateTime.UtcNow;
                _context.AttributteValues.Add(attributeValue);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add attribute value: " + ex.Message, ex);
            }
        }
    }
}
