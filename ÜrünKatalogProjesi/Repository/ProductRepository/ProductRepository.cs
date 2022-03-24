using AutoMapper;
using MezuniyetProjesi.Authentication;
using MezuniyetProjesi.Context;
using MezuniyetProjesi.Model;
using MezuniyetProjesi.Model.DTOs;
using MezuniyetProjesi.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Repository.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ProductRepository(ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ApplicationResult<ProductDto>> Create(CreateProductInput input, ApplicationUser applicationUser)
        {
            try
            {
                Product mapCat = _mapper.Map<Product>(input);
                mapCat.CreatedById = applicationUser.Id;
                mapCat.CreatedBy = applicationUser.UserName;
                _context.Products.Add(mapCat);
                await _context.SaveChangesAsync();
                ApplicationResult<ProductDto> result = new ApplicationResult<ProductDto>
                {
                    Result = _mapper.Map<ProductDto>(mapCat),
                    Succeeded = true
                };

                return result;
            }
            catch (Exception)
            {

                return new ApplicationResult<ProductDto>
                {
                    Succeeded = false,
                    ErrorMessage = "Error Occured!"
                };
            }
        }

        public async Task<ApplicationResult> Delete(int id)
        {
            try
            {
                var willDelete = _context.Products.Where(x => x.Id == id).FirstOrDefault();
                if (willDelete != null)
                {
                    _context.Products.Remove(willDelete);
                    await _context.SaveChangesAsync();
                    return new ApplicationResult { Succeeded = true };
                }
                else
                {
                    return new ApplicationResult { Succeeded = false, ErrorMessage = ErrorCodes.NoRecordFaund, };
                }
            }
            catch (Exception ex)
            {
                return new GenericException().GetError(ex);
            }
        }

        public async Task<ApplicationResult<ProductDto>> Get(int id)
        {
            try
            {
                Product product = _context.Products.Where(x => x.Id == id).FirstOrDefault();
                ProductDto dto = new()
                {
                    CreatedBy = product.CreatedBy,
                    CreatedById = product.CreatedById,
                    Id = product.Id,
                    ModifiedBy = product.ModifiedBy,
                    ModifiedById = product.ModifiedById,
                    ModifiedDate = product.ModifiedDate,
                    Name = product.Name,
                    UserName = product.UserName,
                    CreatedDate= product.CreatedDate,
                    Brand = product.Brand,
                    CategoryId = product.CategoryId,
                    Color = product.Color,
                    Description = product.Description,
                    Image = product.Image,
                    IsOfferable = product.IsOfferable,
                    IsSold = product.IsSold,
                    Price = product.Price,
                    ProductCondition = product.ProductCondition
                };
                return new ApplicationResult<ProductDto>
                {
                    Result = dto,
                    Succeeded = true
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<ProductDto>
                {
                    Succeeded = false,
                    ErrorMessage = "Error Occured!"
                };
            }
        }

        public async Task<ApplicationResult<List<ProductDto>>> GetAll()
        {
            try
            {
                List<ProductDto> list = await _context.Products.Select(product => new ProductDto
                {
                    CreatedBy = product.CreatedBy,
                    CreatedById = product.CreatedById,
                    Id = product.Id,
                    ModifiedBy = product.ModifiedBy,
                    ModifiedById = product.ModifiedById,
                    ModifiedDate = product.ModifiedDate,
                    Name = product.Name,
                    UserName = product.UserName,
                    CreatedDate = product.CreatedDate,
                    Brand = product.Brand,
                    CategoryId = product.CategoryId,
                    Color = product.Color,
                    Description = product.Description,
                    Image = product.Image,
                    IsOfferable = product.IsOfferable,
                    IsSold = product.IsSold,
                    Price = product.Price,
                    ProductCondition = product.ProductCondition
                }).ToListAsync();

                return new ApplicationResult<List<ProductDto>>
                {
                    Result = list,
                    Succeeded = true
                };

            }
            catch (Exception)
            {
                return new ApplicationResult<List<ProductDto>>
                {
                    Succeeded = false,
                    ErrorMessage = "Error Occured!"
                };
            }
        }

        public async Task<ApplicationResult<ProductDto>> Update(UpdateProductInput input, ApplicationUser applicationUser)
        {
            try
            {
                
                var getExistProduct = await _context.Products.FindAsync(input.Id);
                if (getExistProduct == null)
                {
                    return new ApplicationResult<ProductDto>
                    {
                        Succeeded = false,
                        ErrorMessage = ErrorCodes.NoRecordFaund,
                        Result = new ProductDto()
                    };
                }

                getExistProduct.Id = input.Id;
                getExistProduct.Name = input.Name;
                getExistProduct.Brand = input.Brand;
                getExistProduct.CategoryId = input.CategoryId;
                getExistProduct.Color = input.Color;
                getExistProduct.Description = input.Description;
                getExistProduct.Image = input.Image;
                getExistProduct.UserName = input.UserName;
                getExistProduct.IsOfferable = input.IsOfferable;
                getExistProduct.IsSold = input.IsSold;
                getExistProduct.Price = input.Price;
                getExistProduct.ProductCondition = input.ProductCondition;
                getExistProduct.ModifiedBy = applicationUser.UserName;
                getExistProduct.ModifiedById = applicationUser.Id;
                getExistProduct.ModifiedDate = DateTime.UtcNow;
                _context.Update(getExistProduct);
                await _context.SaveChangesAsync();

                return new ApplicationResult<ProductDto>
                {
                    Succeeded = true,
                    Result = new ProductDto
                    {
                        CreatedBy = getExistProduct.CreatedBy,
                        CreatedById = getExistProduct.CreatedById,
                        Id = getExistProduct.Id,
                        ModifiedBy = getExistProduct.ModifiedBy,
                        ModifiedById = getExistProduct.ModifiedById,
                        ModifiedDate = getExistProduct.ModifiedDate,
                        Name = getExistProduct.Name,
                        CreatedDate = getExistProduct.CreatedDate,
                        Brand = getExistProduct.Brand,
                        CategoryId = getExistProduct.CategoryId,
                        Color = getExistProduct.Color,
                        Description = getExistProduct.Description,
                        Image = getExistProduct.Image,
                        UserName = getExistProduct.UserName,
                        IsOfferable = getExistProduct.IsOfferable,
                        IsSold = getExistProduct.IsSold,
                        Price = getExistProduct.Price,
                        ProductCondition = getExistProduct.ProductCondition

                    }
                };
            }
            catch (Exception)
            {

                return new ApplicationResult<ProductDto>
                {
                    Succeeded = false,
                    ErrorMessage = "Error Occured!"
                };

            
        }
    }
    }
}
