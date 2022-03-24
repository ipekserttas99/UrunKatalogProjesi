using MezuniyetProjesi.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Repository
{
    public interface IRepository<PK, MainDto, CreateDto, UpdateDto, ApplicationUser>
    {
        Task<ApplicationResult<MainDto>> Get(PK id);
        Task<ApplicationResult<List<MainDto>>> GetAll();
        Task<ApplicationResult<MainDto>> Create(CreateDto input, ApplicationUser applicationUser);
        Task<ApplicationResult<MainDto>> Update(UpdateDto input, ApplicationUser applicationUser);
        Task<ApplicationResult> Delete(PK id);
    }
}
