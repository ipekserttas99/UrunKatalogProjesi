using MezuniyetProjesi.Authentication;
using MezuniyetProjesi.Model.DTOs;
using MezuniyetProjesi.Shared;
using MezuniyetProjesi.Uow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public AccountController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

       
        [HttpGet("{Username}")] // KULLANICININ YAPTIĞI TEKLİFLER ENDPOINT'İ, username parametresini almaya gerek yoktu ancak aynı controllerda 2 get çalışmayacağı için böyle yaptım
        public async Task<ActionResult<List<OfferDto>>> OffersThatIMade(string username)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var offercreatedby = _unitOfWork.Offer.GetAll().Result.Result.FindAll(x => x.CreatedBy == user.UserName); // swaggerdan girilen username, teklifi yaratan kişi ise değişkene at
            if(offercreatedby != null) // eğer boş değil ise
            {
                return Ok(offercreatedby); //teklifleri dön
            }
            return BadRequest("Verdiğiniz bir teklif bulunmamaktadır.");
        }

        [HttpGet] // KULLANICININ ALDIĞI TEKLİFLER ENDPOINT'İ
        public async Task<ActionResult<List<OfferDto>>> OffersThatIReceive()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User); // authorize olan user'ı al
            var product = _unitOfWork.Product.GetAll().Result.Result.FindAll(x => x.CreatedBy == user.UserName); // bu kullanıcıya ait productları bul
            if(product != null) // kullanıcının product'ı varsa
            {
                List<int> list2 = new List<int>(); // Yeni liste oluştur
                var sonuc = new List<OfferDto>(); //Yeni OfferDto tipinde liste oluştur
                product.ForEach(n => list2.Add(n.Id)); //product'taki her bir ürünün id'sini list2'ye at
                for (int i = 0; i < list2.Count(); i++) 
                {
                    var offer = _unitOfWork.Offer.GetAll().Result.Result.Find(x => x.ProductId == list2[i]); // ürün idleri ile eşleşen ve kullanıcı tarafından oluşturulmuş ürünleri bul
                    sonuc.Add(offer); // listeye at
                }
                return sonuc; // sonucunda kullanıcının bütün aldığı teklifleri sıralamış olduk
            }
            

            return BadRequest("Teklif bulunmamaktadır.");
        }

        [HttpPut("{id}")] // YAPILAN TEKLİFİ KABUL ETME ENDPOINT'İ
        public async Task<ActionResult<ApplicationResult<ProductDto>>> AcceptTheOffer(int id)
        {
            
            var user = await _userManager.GetUserAsync(HttpContext.User); // authorize olan user'ı al
            var teklif = await _unitOfWork.Offer.Get(id); // girilen id'ye sahip teklifi al
            var kk = teklif.Result.ProductId;       //teklifteki ürün id'sini al   
            var product = _unitOfWork.Product.GetAll().Result.Result.FirstOrDefault(x => x.Id == kk); // offer tablosundaki productid ve producttaki id eşitse producta attı
            if (product.UserName == user.UserName)
            {
                if (teklif.Result.IsOfferPercentage is true) //eğer teklifteki "teklif yüzdelik mi" kısmı true ise
                {
                    int yuzdelik = teklif.Result.OfferedPrice; //girilen değer yüzdeliği temsil eder
                    var map = new UpdateProductInput
                    {
                    
                        Id = product.Id,
                        Brand = product.Brand,
                        Color = product.Color,
                        CategoryId = product.CategoryId,
                        Description = product.Description,
                        UserName = product.UserName,             //yeni update input'u oluşturdu price'ı yüzdelik üzerinden hesaplayarak set etti
                        IsOfferable = true,
                        IsSold = false,
                        Name = product.Name,
                        Image = product.Image,
                        Price = (product.Price * (yuzdelik / 100)),
                        ProductCondition = product.ProductCondition
                    };
                
                    var result = await _unitOfWork.Product.Update(map, user); // ürünü güncelle
                
                    return result;
                }
                else if (teklif.Result.IsOfferPercentage is false) //eğer teklifteki "teklif yüzdelik mi" kısmı false ise
                {
                    var map = new UpdateProductInput
                    {

                        Id = product.Id,
                        Brand = product.Brand,
                        Color = product.Color,
                        CategoryId = product.CategoryId,
                        Description = product.Description,
                        UserName = product.UserName,             //yeni update input'u oluşturdu price'ı verilen teklif fiyatından kabul etti
                        IsOfferable = true,
                        IsSold = false,
                        Name = product.Name,
                        Image = product.Image,
                        Price = teklif.Result.OfferedPrice,
                        ProductCondition = product.ProductCondition
                    };

                    var result = await _unitOfWork.Product.Update(map, user); // ürünü güncelle

                    return result;
                }

            }
            
            return NotFound("Teklif kabul edilemedi.");
        }

        
    }
}
