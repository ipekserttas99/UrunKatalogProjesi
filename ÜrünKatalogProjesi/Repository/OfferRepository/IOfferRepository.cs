using MezuniyetProjesi.Authentication;
using MezuniyetProjesi.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Repository.OfferRepository
{
    public interface IOfferRepository : IRepository<int, OfferDto, CreateOfferInput, UpdateOfferInput, ApplicationUser>
    {
    }
}
