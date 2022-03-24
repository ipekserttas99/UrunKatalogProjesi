using AutoMapper;
using MezuniyetProjesi.Model;
using MezuniyetProjesi.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Shared
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            // Category
            CreateMap<CategoryDto, Category>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryInput, Category>()
                .ForMember(x => x.ModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.ModifiedById, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(s => DateTime.UtcNow))
                .ForMember(x => x.ModifiedDate, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<Category, CreateCategoryInput>();

            // Product
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<UpdateProductInput, ProductDto>().ReverseMap();
            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductInput, Product>()
                .ForMember(x => x.ModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.ModifiedById, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(s => DateTime.UtcNow))
                .ForMember(x => x.ModifiedDate, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<Product, CreateProductInput>();
            CreateMap<ProductDto, UpdateProductInput>()
                .ForMember(x=>x.Image,opt => opt.Ignore());
            CreateMap<CreateProductInput, UpdateProductInput>();

            // Offer
            CreateMap<OfferDto, Offer>();
            CreateMap<UpdateOfferInput, OfferDto>().ReverseMap();
            CreateMap<Offer, OfferDto>();
            CreateMap<CreateOfferInput, Offer>()
                .ForMember(x => x.ModifiedBy, opt => opt.Ignore())
                .ForMember(x => x.ModifiedById, opt => opt.Ignore())
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(s => DateTime.UtcNow))
                .ForMember(x => x.ModifiedDate, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<Offer, CreateOfferInput>();

            //Mail
            CreateMap<MailDto, Mail>();
            CreateMap<Mail, MailDto>();
            CreateMap<CreateMailInput, Mail>()
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(s => DateTime.UtcNow))
                .ForMember(x => x.Id, opt => opt.Ignore());
            CreateMap<Mail, CreateMailInput>();
        }
    }
}
