using AutoMapper;
using MezuniyetProjesi.Authentication;
using MezuniyetProjesi.Context;
using MezuniyetProjesi.Repository.CategoryRepository;
using MezuniyetProjesi.Repository.MailRepository;
using MezuniyetProjesi.Repository.OfferRepository;
using MezuniyetProjesi.Repository.ProductRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Uow
{
    public class UnitOfWork :IUnitOfWork
    {
        private readonly ILogger logger;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext context;

        public ICategoryRepository Category { get; private set; }

        public IProductRepository Product { get; private set; }

        public IOfferRepository Offer { get; private set; }
        public IMailRepository Mail { get; private set; }

        public UnitOfWork(ApplicationDbContext context, ILoggerFactory loggerFactory, IConfiguration configuration, IMapper mapper)
        {
            this.context = context;
            this.logger = loggerFactory.CreateLogger("Project");
            _mapper = mapper;

            Category = new CategoryRepository(context, mapper);
            Product = new ProductRepository(context, mapper);
            Offer = new OfferRepository(context, mapper);
            Mail = new MailRepository(context, mapper);
        }



        public int Complete()
        {
            return context.SaveChanges();
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
