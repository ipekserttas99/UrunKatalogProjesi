using Hangfire;
using MezuniyetProjesi.Authentication;
using MezuniyetProjesi.Jobs;
using MezuniyetProjesi.Model;
using MezuniyetProjesi.Model.DTOs;
using MezuniyetProjesi.Shared.Services;
using MezuniyetProjesi.Uow;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISendEmailJob sendEmailJob;

        public LoginController(UserManager<ApplicationUser> userManager, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, IUnitOfWork unitOfWork, ISendEmailJob sendEmailJob)
        {
            this.userManager = userManager;
            _configuration = configuration;
            this.signInManager = signInManager;
            _unitOfWork = unitOfWork;
            this.sendEmailJob = sendEmailJob;
        }

        [HttpPost] // LOGIN OLMA ENDPOINT'İ
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            
            if (ModelState.IsValid)
            {
                var loginResult = await signInManager.PasswordSignInAsync(model.UserName, model.Password, true, false); // username ve password ile giriş yap
                if (!loginResult.Succeeded) // giriş başarısız olduysa
                {
                    
                        var user = await userManager.FindByNameAsync(model.UserName); // username'e ait kullanıcıyı bul user değişkenine at
                        await userManager.AccessFailedAsync(user); // ve user'ı access failed et
                        var failedCount = userManager.GetAccessFailedCountAsync(user); // access failed sayısını failedCount değişkenine at
                        if (failedCount.Result.Equals(3)) // eğer 3 kez failed count yaptıysa
                        {
                            BackgroundJob.Enqueue(() => sendEmailJob.DoLogInJob(model.Email, model.UserName)); // mail gönderme background servisini çalıştır
                            
                            CreateMailInput input = new()
                            {
                                 CreatedBy = user.UserName,         // Statüsü "mail gönderildi" olan yeni bir mail kaydı ekle database'e
                                 CreatedById = user.Id,
                                 CreatedDate = DateTime.Now,
                                 Status = "Sent"

                            };
                            await userManager.DeleteAsync(user); // 3 kez fail olduğu için blokeledik
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
                            return BadRequest("Hesabınız bloke oldu. Mail atılmıştır."); // tüm işlemleri yaptıktan sonra dön
                        }

                    return BadRequest("Giriş yapılamadı. Bilgilerinizi kontrol ediniz."); // giriş başarısızsa
                }

                return Ok("Giriş başarılı."); // model state valid ise
            }
            return BadRequest(ModelState); // model state valid değil ise
        }

        
        
    }
}
