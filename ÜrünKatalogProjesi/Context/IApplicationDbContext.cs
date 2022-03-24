using MezuniyetProjesi.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Context
{
    public interface IApplicationDbContext
    {
        DbSet<Category> Categories { get; }
        DbSet<Product> Products { get; } 
        DbSet<Offer> Offers { get; }
        DbSet<Mail> Mails { get; }
        int SaveChanges();
    }
}
