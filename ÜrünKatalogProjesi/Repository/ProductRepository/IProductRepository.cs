using MezuniyetProjesi.Authentication;
using MezuniyetProjesi.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Repository.ProductRepository
{
    public interface IProductRepository:IRepository<int, ProductDto, CreateProductInput, UpdateProductInput, ApplicationUser>
    {

    }
}
