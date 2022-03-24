using AutoMapper;
using MezuniyetProjesi.Authentication;
using MezuniyetProjesi.Context;
using MezuniyetProjesi.Model;
using MezuniyetProjesi.Model.DTOs;
using MezuniyetProjesi.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Repository.CategoryRepository
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ApplicationResult<CategoryDto>> Create(CreateCategoryInput input, ApplicationUser applicationUser)
        {
            try
            {
                Category mapCat = _mapper.Map<Category>(input);
                mapCat.CreatedById = applicationUser.Id;
                mapCat.CreatedBy = applicationUser.UserName;
                _context.Categories.Add(mapCat);
                await _context.SaveChangesAsync();
                ApplicationResult<CategoryDto> result = new ApplicationResult<CategoryDto>
                {
                    Result = _mapper.Map<CategoryDto>(mapCat),
                    Succeeded = true
                };

                return result;
            }
            catch (Exception)
            {
                
                return new ApplicationResult<CategoryDto>
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
                var willDelete = _context.Categories.Where(x => x.Id == id).FirstOrDefault();
                if (willDelete != null)
                {
                    _context.Categories.Remove(willDelete);
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

        public async Task<ApplicationResult<CategoryDto>> Get(int id)
        {
            try
            {
                Category category = _context.Categories.Where(x => x.Id == id).FirstOrDefault();
                CategoryDto dto = new()
                {
                    CreatedBy = category.CreatedBy,
                    CreatedById = category.CreatedById,
                    CreatedDate = category.CreatedDate,
                    Id = category.Id,
                    ModifiedBy = category.ModifiedBy,
                    ModifiedById = category.ModifiedById,
                    ModifiedDate = category.ModifiedDate,
                    Name = category.Name
                };
                return new ApplicationResult<CategoryDto>
                {
                    Result = dto,
                    Succeeded = true
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<CategoryDto>
                {
                    Succeeded = false,
                    ErrorMessage = "Error Occured!"
                };
            }
        }

        public async Task<ApplicationResult<List<CategoryDto>>> GetAll()
        {
            try
            {
                List<CategoryDto> list = await _context.Categories.Select(category => new CategoryDto
                {
                    CreatedBy = category.CreatedBy,
                    CreatedById = category.CreatedById,
                    CreatedDate = category.CreatedDate,
                    Id = category.Id,
                    ModifiedBy = category.ModifiedBy,
                    ModifiedById = category.ModifiedById,
                    ModifiedDate = category.ModifiedDate,
                    Name = category.Name
                }).ToListAsync();

                return new ApplicationResult<List<CategoryDto>>
                {
                    Result = list,
                    Succeeded = true
                };

            }
            catch (Exception)
            {
                return new ApplicationResult<List<CategoryDto>>
                {
                    Succeeded = false,
                    ErrorMessage = "Error Occured!"
                };
            }
        }

        public async Task<ApplicationResult<CategoryDto>> Update(UpdateCategoryInput input, ApplicationUser applicationUser)
        {
            try
            {
                var getExistCategory = await _context.Categories.FindAsync(input.Id);
                if (getExistCategory == null)
                {
                    return new ApplicationResult<CategoryDto>
                    {
                        Succeeded = false,
                        ErrorMessage = ErrorCodes.NoRecordFaund,
                        Result = new CategoryDto()
                    };
                }
                if (!getExistCategory.CreatedById.Equals(applicationUser.Id))
                {
                    return new ApplicationResult<CategoryDto>
                    {
                        Succeeded = false,
                        ErrorMessage = ErrorCodes.NoRecordFaund,
                        Result = new CategoryDto()
                    };
                }

                getExistCategory.Name = input.Name;
                getExistCategory.ModifiedBy = applicationUser.UserName;
                getExistCategory.ModifiedById = applicationUser.Id;
                getExistCategory.ModifiedDate = DateTime.UtcNow;
                _context.Update(getExistCategory);
                await _context.SaveChangesAsync();

                return new ApplicationResult<CategoryDto>
                {
                    Succeeded = true,
                    Result = new CategoryDto
                    {
                        CreatedBy = getExistCategory.CreatedBy,
                        CreatedById = getExistCategory.CreatedById,
                        CreatedDate = getExistCategory.CreatedDate,
                        Id = getExistCategory.Id,
                        ModifiedBy = getExistCategory.ModifiedBy,
                        ModifiedById = getExistCategory.ModifiedById,
                        ModifiedDate = getExistCategory.ModifiedDate,
                        Name = getExistCategory.Name
                    }
                };
            }
            catch (Exception)
            {
                 
                return new ApplicationResult<CategoryDto>
                {
                    Succeeded = false,
                    ErrorMessage = "Error Occured!"
                };

            }
        }
    }
}
