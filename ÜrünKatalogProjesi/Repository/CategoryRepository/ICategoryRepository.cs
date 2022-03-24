using MezuniyetProjesi.Authentication;
using MezuniyetProjesi.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Repository.CategoryRepository
{
    public interface ICategoryRepository: IRepository<int, CategoryDto, CreateCategoryInput, UpdateCategoryInput, ApplicationUser>
    {

    }
}
