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

namespace MezuniyetProjesi.Repository.MailRepository
{
    public class MailRepository : IMailRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MailRepository(ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<ApplicationResult<MailDto>> Create(CreateMailInput input, ApplicationUser applicationUser)
        {
            try
            {
                Mail mail = _mapper.Map<Mail>(input);
                mail.CreatedBy = applicationUser.UserName;
                mail.CreatedById = applicationUser.Id;
                _context.Mails.Add(mail);
                await _context.SaveChangesAsync();
                ApplicationResult<MailDto> result = new ApplicationResult<MailDto>
                {
                    Result = _mapper.Map<MailDto>(mail),
                    Succeeded = true
                };
                return result;
            }
            catch (Exception)
            {

                return new ApplicationResult<MailDto>
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
                var willDelete = _context.Mails.Where(x => x.Id == id).FirstOrDefault();
                if (willDelete != null)
                {
                    _context.Mails.Remove(willDelete);
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

        public async Task<ApplicationResult<MailDto>> Get(int id)
        {
            try
            {
                Mail mail = _context.Mails.Where(x => x.Id == id).FirstOrDefault();
                MailDto dto = new()
                {
                    CreatedBy = mail.CreatedBy,
                    CreatedById = mail.CreatedById,
                    Id = mail.Id,
                    CreatedDate = mail.CreatedDate,
                    Status = mail.Status
                };
                return new ApplicationResult<MailDto>
                {
                    Result = dto,
                    Succeeded = true
                };
            }
            catch (Exception)
            {
                return new ApplicationResult<MailDto>
                {
                    Succeeded = false,
                    ErrorMessage = "Error Occured!"
                };
            }
        }

        public async Task<ApplicationResult<List<MailDto>>> GetAll()
        {
            try
            {
                List<MailDto> list = await _context.Mails.Select(mail => new MailDto
                {
                    CreatedBy = mail.CreatedBy,
                    CreatedById = mail.CreatedById,
                    Id = mail.Id,
                    CreatedDate = mail.CreatedDate,
                    Status = mail.Status

                }).ToListAsync();

                return new ApplicationResult<List<MailDto>>
                {
                    Result = list,
                    Succeeded = true
                };

            }
            catch (Exception)
            {
                return new ApplicationResult<List<MailDto>>
                {
                    Succeeded = false,
                    ErrorMessage = "Error Occured!"
                };
            }
        }

        public async Task<ApplicationResult<MailDto>> Update(UpdateMailInput input, ApplicationUser applicationUser)
        {
            try
            {
                var getExistMail = await _context.Mails.FindAsync(input.Id);
                if (getExistMail == null)
                {
                    return new ApplicationResult<MailDto>
                    {
                        Succeeded = false,
                        ErrorMessage = ErrorCodes.NoRecordFaund,
                        Result = new MailDto()
                    };
                }

                getExistMail.Id = input.Id;
                _context.Update(getExistMail);
                await _context.SaveChangesAsync();

                return new ApplicationResult<MailDto>
                {
                    Succeeded = true,
                    Result = new MailDto
                    {
                        CreatedBy = getExistMail.CreatedBy,
                        CreatedById = getExistMail.CreatedById,
                        Id = getExistMail.Id,
                        CreatedDate = getExistMail.CreatedDate,
                        Status = getExistMail.Status

                    }
                };
            }
            catch (Exception)
            {

                return new ApplicationResult<MailDto>
                {
                    Succeeded = false,
                    ErrorMessage = "Error Occured!"
                };


            }
        
    }
    }
}
