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
        public async Task<List<ProductVariantViewModel>> ProductVarientsByProductId(long productId)
        {
            try
            {
                var attributes = await (from pa in _context.ProductVariants.Where(w => w.ProductId == productId&&w.Status== "Active")
                                        join pad in _context.ProductVariantAttributes
                                            on pa.ProductVariantId equals pad.ProductVariantId into detailsGroup
                                        from pad in detailsGroup.DefaultIfEmpty()
                                        join av in _context.AttributteValues
                                            on (pad != null ? pad.AttributeValueId : 0) equals av.AttributeValueId into valueGroup
                                        from av in valueGroup.DefaultIfEmpty()
                                        join attr in _context.Attributtes
                                            on (pad != null ? pad.AttributeId : 0) equals attr.AttributteId into attrGroup
                                        from attr in attrGroup.DefaultIfEmpty()
                                        select new
                                        {
                                            ProductAttribute = pa,
                                            AttributeName = attr != null ? attr.AttributeName : null,
                                            AttributeValue = av != null ? av.AttrbtValue : null
                                        })
                                        .ToListAsync();

                var result = attributes
                    .GroupBy(x => x.ProductAttribute)
                    .Select(g => new ProductVariantViewModel
                    {
                        ProductVariantId = g.Key.ProductVariantId,
                        ProductId = g.Key.ProductId,
                        SkuNumber=g.Key.SkuNumber,
                        PriceAdjustment = g.Key.PriceAdjustment,
                        StockQuantity = g.Key.StockQuantity,
                        SupplierId = g.Key.SupplierId,
                        Status = g.Key.Status,
                        LastModified = g.Key.LastModified,
                        AttributeDetailsText = g.Any(x => x.AttributeName != null && x.AttributeValue != null)
                            ? string.Join(", ",
                                g.Where(x => x.AttributeName != null && x.AttributeValue != null)
                                 .GroupBy(x => x.AttributeName)
                                 .Select(ag => $"{ag.Key}: {string.Join(", ", ag.Select(x => x.AttributeValue ?? "None"))}"))
                            : string.Empty
                    })
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return new List<ProductVariantViewModel>();
            }
        }
        public async Task<ProductVariantViewModel> ProductVarientsById(long Id)
        {
            try
            {
                var attributes = await (from pa in _context.ProductVariants.Where(w => w.ProductVariantId == Id && w.Status == "Active")
                                        join pad in _context.ProductVariantAttributes
                                            on pa.ProductVariantId equals pad.ProductVariantId into detailsGroup
                                        from pad in detailsGroup.DefaultIfEmpty()
                                        join av in _context.AttributteValues
                                            on (pad != null ? pad.AttributeValueId : 0) equals av.AttributeValueId into valueGroup
                                        from av in valueGroup.DefaultIfEmpty()
                                        join attr in _context.Attributtes
                                            on (pad != null ? pad.AttributeId : 0) equals attr.AttributteId into attrGroup
                                        from attr in attrGroup.DefaultIfEmpty()
                                        select new
                                        {
                                            ProductAttribute = pa,
                                            AttributeName = attr != null ? attr.AttributeName : null,
                                            AttributeValue = av != null ? av.AttrbtValue : null
                                        })
                                        .ToListAsync();

                var result = attributes
                    .GroupBy(x => x.ProductAttribute)
                    .Select(g => new ProductVariantViewModel
                    {
                        ProductVariantId = g.Key.ProductVariantId,
                        ProductId = g.Key.ProductId,
                        SkuNumber = g.Key.SkuNumber,
                        PriceAdjustment = g.Key.PriceAdjustment,
                        StockQuantity = g.Key.StockQuantity,
                        SupplierId = g.Key.SupplierId,
                        Status = g.Key.Status,
                        LastModified = g.Key.LastModified,
                        AttributeDetailsText = g.Any(x => x.AttributeName != null && x.AttributeValue != null)
                            ? string.Join(", ",
                                g.Where(x => x.AttributeName != null && x.AttributeValue != null)
                                 .GroupBy(x => x.AttributeName)
                                 .Select(ag => $"{ag.Key}: {string.Join(", ", ag.Select(x => x.AttributeValue ?? "None"))}"))
                            : string.Empty
                    })
                    .FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return new ProductVariantViewModel();
            }
        }
        public async Task<ProductVariantViewModel?> GetVariantForEditAsync(long productVariantId)
        {
            // Step 1: Get selected AttributeValueIds
            var selectedAttributeValueIds = await _context.ProductVariantAttributes
                .Where(pva => pva.ProductVariantId == productVariantId)
                .Select(pva => pva.AttributeValueId)
                .ToListAsync();

            // Step 2: Load the variant (and attributes) using Include
            var variantEntity = await _context.ProductVariants
                .FirstOrDefaultAsync(v => v.ProductVariantId == productVariantId);

            if (variantEntity == null)
                return null;

            // Step 3: Map to view model
            var variant = new ProductVariantViewModel
            {
                ProductVariantId = variantEntity.ProductVariantId,
                ProductId = variantEntity.ProductId,
                SkuNumber = variantEntity.SkuNumber,
                PriceAdjustment = variantEntity.PriceAdjustment,
                StockQuantity = variantEntity.StockQuantity,
                ImageUrl = variantEntity.ImageUrl,
                Position = variantEntity.Position,
                SupplierId = variantEntity.SupplierId,
                Status = variantEntity.Status,
                LastModified = variantEntity.LastModified,
                AttributeValuesId = selectedAttributeValueIds,
                //Attributes = variantEntity.ProductVariantAttributes.Select(attr => new ProductAttributeDetailViewModel
                //{
                //    AttributteId = attr.AttributeId,
                //    AttributteName = attr.Attributte.AttributeName,
                //    AttributteValueId = attr.AttributeValueId,
                //    AttributteValue = attr.AttributeValue.AttrbtValue
                //}).ToList()
            };

            // Step 4: Load all available attribute values
            variant.AllAttributeValues = await _context.AttributteValues
             .ToListAsync();

            return variant;
        }


        //public async Task<List<ProductVariants>> ProductVarientsByProductId(long productId)
        //{
        //    try
        //    {
        //        return await _context.ProductVariants.Where(w=>w.ProductId== productId).Include(i=>i.ProductVariantAttributes)
        //            .ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        // TODO: Log exception
        //        // Console.WriteLine($"Error in GetAttributeValues: {ex.Message}");
        //        return null;
        //    }
        //}
        public async Task<(bool,long)> SaveProductVariantWithProductVariantAttribute(ProductVariants model)
        {
            try
            {
                var productVariant = model.ProductVariantId == 0
                    ? new ProductVariants
                    {
                        ProductId = model.ProductId,
                        SkuNumber = model.SkuNumber,
                        PriceAdjustment = model.PriceAdjustment,
                        StockQuantity = model.StockQuantity,
                        SupplierId = model.SupplierId,
                        ImageUrl = model.ImageUrl,
                        Position = model.Position,
                        Status = model.Status ?? "Active",
                        LastModified = DateTime.UtcNow
                    }
                    : await _context.ProductVariants.FindAsync(model.ProductVariantId);

                if (productVariant == null)
                    return (false, 0);

                if (model.ProductVariantId > 0)
                {
                    productVariant.ProductId = model.ProductId;
                    productVariant.SkuNumber = model.SkuNumber;
                    productVariant.PriceAdjustment = model.PriceAdjustment;
                    productVariant.StockQuantity = model.StockQuantity;
                    productVariant.SupplierId = model.SupplierId;
                    productVariant.ImageUrl = model.ImageUrl;
                    productVariant.Position = model.Position;
                    productVariant.Status = model.Status ?? "Active";
                    productVariant.LastModified = DateTime.UtcNow;
                }
                else
                {
                    _context.ProductVariants.Add(productVariant);
                }

                await _context.SaveChangesAsync();

                // Get existing attributes
                var existingDetails = await _context.ProductVariantAttributes
                    .Where(d => d.ProductVariantId == productVariant.ProductVariantId)
                    .ToListAsync();

                // Handle attribute values
                if (model.AttributeValuesId != null && model.AttributeValuesId.Any())
                {
                    // Create a list of new/existing attribute value IDs to keep
                    var incomingAttributeValueIds = model.AttributeValuesId.ToList();

                    // Remove attributes that are not in the incoming list
                    foreach (var existing in existingDetails)
                    {
                        if (!incomingAttributeValueIds.Contains(existing.AttributeValueId))
                        {
                            _context.ProductVariantAttributes.Remove(existing);
                        }
                    }

                    // Add or update attributes
                    foreach (var valueId in incomingAttributeValueIds)
                    {
                        var value = await _context.AttributteValues.FindAsync(valueId);
                        if (value != null)
                        {
                            var existingAttribute = existingDetails
                                .FirstOrDefault(d => d.AttributeValueId == valueId);

                            if (existingAttribute == null)
                            {
                                // Add new attribute
                                _context.ProductVariantAttributes.Add(new ProductVariantAttribute
                                {
                                    ProductVariantId = productVariant.ProductVariantId,
                                    AttributeId = value.AttributeId,
                                    AttributeValueId = valueId
                                });
                            }
                        }
                    }
                }
                else
                {
                    // If no attribute values provided, remove all existing attributes
                    foreach (var existing in existingDetails)
                    {
                        _context.ProductVariantAttributes.Remove(existing);
                    }
                }

                await _context.SaveChangesAsync();
                model.ProductVariantId = productVariant.ProductVariantId;
                return (true, productVariant.ProductVariantId);
            }
            catch
            {
                return (false,0);
            }
        }

        public bool DeleteProductAttriVariants(long id)
        {
            try
            {
                var productVariant = _context.ProductVariants.Find(id);
                if (productVariant == null)
                    return false;

                var details = _context.ProductVariantAttributes.Where(d => d.ProductVariantId == id);
                _context.ProductVariantAttributes.RemoveRange(details);
                
                productVariant.Status = "Delete";
                productVariant.LastModified =DateTime.UtcNow; // Or use DubaiTime if needed
                _context.Entry(productVariant).State = EntityState.Modified;

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // TODO: Log exception
                // Console.WriteLine($"Error in DeleteProductAttribute: {ex.Message}");
                return false;
            }
        }

        public async Task<List<ProductVariantDto>> GetProductVariantsAsync(long productId)
        {
            try
            {
                var productVariants = await _context.ProductVariants
               .Where(pv => pv.ProductId == productId && pv.Status == "Active")
               .Include(pv => pv.ProductVariantAttributes)
                   .ThenInclude(pva => pva.Attributte)
               .Include(pv => pv.ProductVariantAttributes)
                   .ThenInclude(pva => pva.AttributeValue)
               .Select(pv => new ProductVariantDto
               {
                   ProductVariantId = pv.ProductVariantId,
                   ProductId = pv.ProductId,
                   SkuNumber = pv.SkuNumber,
                   PriceAdjustment = pv.PriceAdjustment,
                   StockQuantity = pv.StockQuantity,
                   ImageUrl = pv.ImageUrl,
                   Position = pv.Position,
                   SupplierId = pv.SupplierId,
                   Status = pv.Status,
                   LastModified = pv.LastModified,
                   Attributes = pv.ProductVariantAttributes
                       .Where(pva => pva.Status == "Active")
                       .GroupBy(pva => new { pva.AttributeId, pva.Attributte.AttributeName })
                       .Select(g => new ProductVariantAttributeDto
                       {
                           AttributeId = g.Key.AttributeId,
                           AttributeName = g.Key.AttributeName,
                           Values = g.Select(pva => new AttributeValueDto
                           {
                               AttributeValueId = pva.AttributeValueId,
                               AttributeValue = pva.AttributeValue.AttrbtValue,
                               ProductVariantAttributeId = pva.ProductVariantAttributeId,
                              // Status = pva.Status,
                             //  LastModified = pva.LastModified
                           }).ToList()
                       }).ToList()
               })
               //.OrderBy(pv => pv.Position)
               .ToListAsync();

                return   productVariants;
            }
            catch (Exception ex)
            {
               
                return null;
            }
        }

        public async Task<ProductVariantsResponseDto> GetProductVariantsForEcommerceAsync(long productId,string baseUrl)
        {
            try
            {
                var productVariants = await _context.ProductVariants
                    .Where(pv => pv.ProductId == productId && pv.Status == "Active")
                    .Include(pv => pv.ProductVariantAttributes)
                        .ThenInclude(pva => pva.Attributte)
                    .Include(pv => pv.ProductVariantAttributes)
                        .ThenInclude(pva => pva.AttributeValue)
                    .ToListAsync();

                var productVariantDtos = productVariants.Select(pv => new ProductVariantDto
                {
                    ProductVariantId = pv.ProductVariantId,
                    ProductId = pv.ProductId,
                    SkuNumber = pv.SkuNumber,
                    PriceAdjustment = pv.PriceAdjustment,
                    StockQuantity = pv.StockQuantity,
                    ImageUrl = baseUrl + pv.ImageUrl,
                    Position = pv.Position,
                    SupplierId = pv.SupplierId,
                    Status = pv.Status,
                    LastModified = pv.LastModified,
                    //AttributeDetailsText = string.Join(", ", pv.ProductVariantAttributes
                    //    .Where(pva => pva.Status == "Active")
                    //    .Select(pva => $"{pva.Attributte.AttributeName}: {pva.AttributeValue.AttrbtValue}")),
                    AttributeGroupDtos = pv.ProductVariantAttributes
                        .Where(pva => pva.Status == "Active")
                        .GroupBy(pva => new { pva.AttributeId, pva.Attributte.AttributeName })
                        .Select(g => new AttributeGroupDto
                        {
                            Attribute = g.Key.AttributeName,
                            Values = g.Select(pva => new AttributeValueDto
                            {
                                AttributeValue = pva.AttributeValue.AttrbtValue,
                                ProductVariantId = pv.ProductVariantId,
                                Price = pv.PriceAdjustment,
                                StockQuantity = pv.StockQuantity
                            })
                            .DistinctBy(v => v.AttributeValue)
                            .OrderBy(v => v.AttributeValue)
                            .ToList()
                        })
                        .OrderBy(a => a.Attribute)
                        .ToList()
                }).OrderBy(pv => pv.Position).ToList();

                var attributes = productVariants
                    .SelectMany(pv => pv.ProductVariantAttributes.Select(pva => new { pva, pv }))
                    .Where(x => x.pva.Status == "Active")
                    .GroupBy(x => new { x.pva.AttributeId, x.pva.Attributte.AttributeName })
                    .Select(g => new AttributeGroupDto
                    {
                        AttributeId=g.Key.AttributeId,
                        Attribute = g.Key.AttributeName,
                        Values = g.Select(x => new AttributeValueDto
                        {
                            AttributeValueId=x.pva.AttributeValueId,
                            AttributeValue = x.pva.AttributeValue.AttrbtValue,
                            ProductVariantId = x.pv.ProductVariantId,
                            Price = x.pv.PriceAdjustment,
                            StockQuantity = x.pv.StockQuantity,
                            ImageUrl=x.pv.ImageUrl != null ? baseUrl + x.pv.ImageUrl : null
                        })
                        .DistinctBy(v => v.AttributeValue)
                        .OrderBy(v => v.AttributeValue)
                        .ToList()
                    })
                    .OrderBy(a => a.Attribute)
                    .ToList();

                 
                return new ProductVariantsResponseDto
                {

                    //  ProductVariants = productVariantDtos,
                    AttributeSets = attributes,
                    DefaultOrFirstVariants = productVariantDtos.FirstOrDefault()
                };
            }
            catch (Exception ex)
            {
                return new ProductVariantsResponseDto
                {

                    // ProductVariants = new List<ProductVariantDto>(),
                    AttributeSets = new List<AttributeGroupDto>(),
                   
                };
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
        public List<AttributteValue> GetAttributeValues(long companyId)
        {
            try
            {
                return _context.AttributteValues
                    .Include(av => av.Attributte)
                    .Where(av => av.Attributte.CompanyId == companyId&&av.Status== "Active")
                    .ToList();
            }
            catch (Exception ex)
            {
                // TODO: Log exception
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
