using FluentValidation;
using Hangfire;
using MezuniyetProjesi.Authentication;
using MezuniyetProjesi.Jobs;
using MezuniyetProjesi.Model;
using MezuniyetProjesi.Model.DTOs;
using MezuniyetProjesi.Responses;
using MezuniyetProjesi.Shared;
using MezuniyetProjesi.Uow;
using MezuniyetProjesi.Validator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISendEmailJob sendEmailJob;

        public RegisterController(UserManager<ApplicationUser> userManager, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, IUnitOfWork unitOfWork, ISendEmailJob sendEmailJob)
        {
            this.userManager = userManager;
            _configuration = configuration;
            this.signInManager = signInManager;
            _unitOfWork = unitOfWork;
            this.sendEmailJob = sendEmailJob;
        }

        [HttpPost] // REGISTER OLMA ENDPOINT'İ
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid) //model validse
            {
                var newUser = new ApplicationUser
                {
                    UserName = model.Username,                 //yeni application user oluştur
                    Email = model.Email


                };

                var registerUser = await userManager.CreateAsync(newUser, model.Password); // usermanager ile kullanıcıyı oluştur
                if (registerUser.Succeeded) //kullanıcı oluşturma başarılıysa
                {
                    await signInManager.SignInAsync(newUser, isPersistent: false); //signin
                    var user = await userManager.FindByNameAsync(newUser.UserName);

                    //hoşgeldiniz emaili
                    BackgroundJob.Enqueue(() => sendEmailJob.DoRegisterJob(model.Email, model.Username)); // mail gönderme background servisini çalıştır

                    CreateMailInput input = new()
                    {
                        CreatedBy = user.UserName,         // Statüsü "mail gönderildi" olan yeni bir mail kaydı ekle database'e
                        CreatedById = user.Id,
                        CreatedDate = DateTime.Now,
                        Status = "Sent"

                    };
                    
                    await _unitOfWork.Mail.Create(input, user); //Statüsü "mail gönderildi" olan yeni bir mail kaydı ekle database'e
                    var failJob = Hangfire.SqlServer.SqlServerStorage.Current.GetMonitoringApi().FailedCount(); // hangfiredaki failed olan jobları count et değişkene at
                    if (failJob is 1) // background serviste başarısız olan mailleri 5 kez göndermeyi dene yine başarısızsa failed tablosuna at diye config yaptık. Eğer 5 kez denedi ve failed tablosuna attıysa burası 1 olacaktır. Eğer 1 ise
                    {
                        var kk = _unitOfWork.Mail.GetAll().Result.Result.Find(x => x.CreatedById == user.Id); //mail tablosundaki ilgili maili bul
                        UpdateMailInput updateMail = new()
                        {
                            Id = kk.Id,
                            CreatedBy = kk.CreatedBy,
                            CreatedById = kk.CreatedById, // statüsünü failed'e çek
                            CreatedDate = kk.CreatedDate,
                            Status = "Failed"

                        };
                        await _unitOfWork.Mail.Update(updateMail, user); // güncelle
                        Hangfire.SqlServer.SqlServerStorage.Current.GetMonitoringApi().FailedJobs(0, int.MaxValue).Clear(); // failed jobs 'ı temizle
                    }
                return Ok(GetTokenResponse(user));
                    
                }
                AddErrors(registerUser);
            }
        return BadRequest("İşlem başarısız!");
        }
        private void AddErrors(IdentityResult result) //ERROR METODU
        {
            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("error", err.Description);
            }
        }
        private JwtTokenResult GetTokenResponse(ApplicationUser user) // TOKEN RESPONSE'UNU DÖNEN METOT
        {
            var token = GetToken(user);
            JwtTokenResult result = new()
            {
                AccessToken = token,
                ExpireInSeconds = _configuration.GetValue<int>("Tokens:Lifetime"),
                UserId = user.Id
            };
            return result;
        }

        private string GetToken(ApplicationUser user) //TOKEN ÜRETEN METOT
        {
            var utcNow = DateTime.UtcNow;

            var claims = new Claim[]
            {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString())
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                claims: claims,
                notBefore: utcNow,
                expires: DateTime.Now.AddDays(30),
                audience: _configuration["JWT:ValidAudience"],
                issuer: _configuration["JWT:ValidIssuer"],
                signingCredentials: signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
