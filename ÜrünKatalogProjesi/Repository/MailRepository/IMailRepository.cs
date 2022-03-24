using MezuniyetProjesi.Authentication;
using MezuniyetProjesi.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Repository.MailRepository
{
    public interface IMailRepository : IRepository<int, MailDto, CreateMailInput, UpdateMailInput, ApplicationUser>

    {
    }
}
