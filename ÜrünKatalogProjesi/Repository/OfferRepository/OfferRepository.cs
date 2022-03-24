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

namespace MezuniyetProjesi.Repository.OfferRepository
{
    public class OfferRepository : IOfferRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OfferRepository(ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ApplicationResult<OfferDto>> Create(CreateOfferInput input, ApplicationUser applicationUser)
        {
            try
            {
                Offer map = _mapper.Map<Offer>(input);
                map.CreatedById = applicationUser.Id;
                map.CreatedBy = applicationUser.UserName;
                map.CreatedDate = DateTime.Now;
                map.ModifiedBy = applicationUser.UserName;
                map.ModifiedById = applicationUser.Id;
                map.ModifiedDate = DateTime.Now;
                _context.Offers.Add(map);
                await _context.SaveChangesAsync();
                ApplicationResult<OfferDto> result = new ApplicationResult<OfferDto>
                {
                    Result = _mapper.Map<OfferDto>(map),
                    Succeeded = true
                };

                return result;
            }
            catch (Exception)
            {

                return new ApplicationResult<OfferDto>
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
                var willDelete = _context.Offers.Where(x => x.Id == id).FirstOrDefault();
                if (willDelete != null)
                {
                    _context.Offers.Remove(willDelete);
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

        public async Task<ApplicationResult<OfferDto>> Get(int id)
        {
            try
            {
                Offer offer = _context.Offers.Where(x => x.Id == id).FirstOrDefault();
                OfferDto dto = new()
                {
                    CreatedBy = offer.CreatedBy,
                    CreatedById = offer.CreatedById,
                    Id = offer.Id,
                    ModifiedBy = offer.ModifiedBy,
                    ModifiedById = offer.ModifiedById,
                    ModifiedDate = offer.ModifiedDate,
                    CreatedDate = offer.CreatedDate,
                    IsOfferPercentage = offer.IsOfferPercentage,
                    OfferedPrice = offer.OfferedPrice,
                    ProductId = offer.ProductId
                };
                return new ApplicationResult<OfferDto>
                {
                    Result = dto,
                    Succeeded = true
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<OfferDto>
                {
                    Succeeded = false,
                    ErrorMessage = "Error Occured!"
                };
            }
        }

        public async Task<ApplicationResult<List<OfferDto>>> GetAll()
        {
            try
            {
                List<OfferDto> list = await _context.Offers.Select(offer => new OfferDto
                {
                    CreatedBy = offer.CreatedBy,
                    CreatedById = offer.CreatedById,
                    Id = offer.Id,
                    ModifiedBy = offer.ModifiedBy,
                    ModifiedById = offer.ModifiedById,
                    ModifiedDate = offer.ModifiedDate,
                    CreatedDate = offer.CreatedDate,
                    IsOfferPercentage = offer.IsOfferPercentage,
                    OfferedPrice = offer.OfferedPrice,
                    ProductId = offer.ProductId
                    
                }).ToListAsync();

                return new ApplicationResult<List<OfferDto>>
                {
                    Result = list,
                    Succeeded = true
                };

            }
            catch (Exception)
            {
                return new ApplicationResult<List<OfferDto>>
                {
                    Succeeded = false,
                    ErrorMessage = "Error Occured!"
                };
            }
        }

        public async Task<ApplicationResult<OfferDto>> Update(UpdateOfferInput input, ApplicationUser applicationUser)
        {
            try
            {
                var getExistOffer = await _context.Offers.FindAsync(input.Id);
                if (getExistOffer == null)
                {
                    return new ApplicationResult<OfferDto>
                    {
                        Succeeded = false,
                        ErrorMessage = ErrorCodes.NoRecordFaund,
                        Result = new OfferDto()
                    };
                }

                getExistOffer.Id = input.Id;
                getExistOffer.ProductId = input.ProductId;
                getExistOffer.IsOfferPercentage = input.IsOfferPercentage;
                getExistOffer.OfferedPrice = input.OfferedPrice;
                getExistOffer.ModifiedBy = applicationUser.UserName;
                getExistOffer.ModifiedById = applicationUser.Id;
                getExistOffer.ModifiedDate = DateTime.UtcNow;
                _context.Update(getExistOffer);
                await _context.SaveChangesAsync();

                return new ApplicationResult<OfferDto>
                {
                    Succeeded = true,
                    Result = new OfferDto
                    {
                        CreatedBy = getExistOffer.CreatedBy,
                        CreatedById = getExistOffer.CreatedById,
                        Id = getExistOffer.Id,
                        ModifiedBy = getExistOffer.ModifiedBy,
                        ModifiedById = getExistOffer.ModifiedById,
                        ModifiedDate = getExistOffer.ModifiedDate,
                        CreatedDate = getExistOffer.CreatedDate,
                        ProductId = getExistOffer.ProductId,
                        IsOfferPercentage = getExistOffer.IsOfferPercentage,
                        OfferedPrice = getExistOffer.OfferedPrice

                    }
                };
            }
            catch (Exception)
            {

                return new ApplicationResult<OfferDto>
                {
                    Succeeded = false,
                    ErrorMessage = "Error Occured!"
                };


            }
        }
    }
}
