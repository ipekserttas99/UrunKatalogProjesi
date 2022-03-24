using MezuniyetProjesi.Repository.CategoryRepository;
using MezuniyetProjesi.Repository.MailRepository;
using MezuniyetProjesi.Repository.OfferRepository;
using MezuniyetProjesi.Repository.ProductRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Uow
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IOfferRepository Offer { get; }
        IMailRepository Mail { get; }
        int Complete();
    }
}
