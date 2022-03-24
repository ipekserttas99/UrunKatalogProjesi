using MezuniyetProjesi.Authentication;
using MezuniyetProjesi.Model.DTOs;
using MezuniyetProjesi.Shared;
using MezuniyetProjesi.Uow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Controllers
{
    [Route("[controller]s")]
    [ApiController]
    [Authorize]
    public class OfferController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public OfferController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }


        [HttpPost] //TEKLİF YAP ENDPOINT'İ
        public async Task<ActionResult<ApplicationResult<OfferDto>>> MakeOffer([FromBody] CreateOfferInput input)
        {
            var rr = _unitOfWork.Product.GetAll().Result.Result.FirstOrDefault(x => x.Id == input.ProductId); //girilen inputtaki productid product tablosundaki id ise rr'ye at
            var user = await _userManager.GetUserAsync(HttpContext.User); //kullanıcıyı al
           
            if (rr.IsOfferable == true) //eğer ürün teklife açıksa
            {
                if (input.IsOfferPercentage) // ürünün teklifi yüzdelik ise
                {
                    input.OfferedPrice = rr.Price - rr.Price * input.OfferedPrice / 100; // inputta girilen yüzdelik olarak alınır ve fiyat üzerinden hesaplanır
                    var result = await _unitOfWork.Offer.Create(input, user); // teklif create edilir
                    if (result.Succeeded)
                    {
                        return result; //create başarılı ise döner
                    }
                    
                }
                else if (input.IsOfferPercentage ==false) // eğer ürünün teklifi yüzdelik değil ise
                {
                    var resultt = await _unitOfWork.Offer.Create(input, user); //inputtaki değerlerle offer create et
                    if (resultt.Succeeded)
                    {
                        return resultt; //create başarılı ise döner
                    }
                }
            }
            return NotFound("Ürün teklife açık değildir!"); //eğer ürün teklife açık değilse döner

        }

        [HttpDelete("{id}")] // YAPTIĞI TEKLİFİ GERİ ÇEKME ENDPOINT'İ
        public async Task<ActionResult<ApplicationResult<OfferDto>>> CancelOffer(int id)
        {
            var omu = await _unitOfWork.Offer.Get(id); //girilen id'yle teklif tablosunda eşleşen teklifi bul 
            var username = _userManager.GetUserName(HttpContext.User); //kullanıcının username'ini al
            if (omu.Result.CreatedBy == username) //teklif kullanıcının username'i ile oluşturulduysa
            {
                var result = await _unitOfWork.Offer.Delete(id); // teklifi sil
                if (result.Succeeded)
                {
                    return Ok("Teklif silindi."); //silme işlemi tamamlandıysa döner
                } 
            }
            else if (omu.Result.CreatedBy != username)
            {
                return BadRequest("Size ait olmayan teklifi silemezsiniz!");
            }
            return BadRequest("Böyle bir teklifiniz yok."); 
        }
    }
}
