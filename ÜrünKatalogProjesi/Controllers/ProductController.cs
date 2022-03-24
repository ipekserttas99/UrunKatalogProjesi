using AutoMapper;
using MezuniyetProjesi.Authentication;
using MezuniyetProjesi.Model;
using MezuniyetProjesi.Model.DTOs;
using MezuniyetProjesi.Shared;
using MezuniyetProjesi.Shared.Filters;
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
    public class ProductController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            webHostEnvironment = hostEnvironment;

        }
        private string ImageFile(ProductImage image) // SWAGGERDAN UPLOAD EDİLEN DOSYAYI STRINGE ÇEVİRİP BUNU DÖNEN + İMAGES KLASÖRÜNE RESMİ EKLEYEN METOT
        {
            string uniqueFileName = null;

            if (image.Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.ContentRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + image.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.Image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        [HttpPost] // ÜRÜN EKLEME ENDPOINT'İ
        public async Task<ActionResult<ApplicationResult<ProductDto>>> AddProduct([FromForm] ProductImage image, [FromQuery] CreateProductInput input)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User); // kullanıcıyı al
            string uniqueFileName = ImageFile(image); // swaggerdan girilen dosyayı yukarıdaki metotta çözümle. 
            var n = new CreateProductInput
                {
                    Brand = input.Brand,
                    CategoryId = input.CategoryId,
                    Color = input.Color,
                    Description = input.Description,
                    Image = uniqueFileName,
                    IsOfferable = true,                            //yeni create inputu oluştur
                    IsSold = false,
                    Name = input.Name,
                    Price = input.Price,
                    ProductCondition = input.ProductCondition,
                    UserName = input.UserName
                };
            var result = await _unitOfWork.Product.Create(n, user); // ürünü ekle
            if (result.Succeeded) // ekleme başarılı ise
            {
                return result; //dön
            }
            
            return NotFound("Ürün eklenemedi!");
                
        }


        [HttpGet("{id}")] // SATIN ALMA ENDPOINT'İ
        public async Task<ActionResult<ApplicationResult<ProductDto>>> Purchase(int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User); //kullanıcıyı al
            var result = await _unitOfWork.Product.Get(id); // girilen id ile eşlesen product'ı al
            if (result.Succeeded) // listeleme başarılı ise
            {
                if(result.Result.IsSold is false) // eğer ürün satın alınmamışsa
                {
                    var maplendi =
                    new UpdateProductInput
                    {
                        Id = result.Result.Id,
                        Brand = result.Result.Brand,
                        Color = result.Result.Color,
                        CategoryId = result.Result.CategoryId,
                        Description = result.Result.Description,                   //yeni bir update inputu oluştur ve satıldı mı alanını true, 
                        UserName = result.Result.UserName,                          //teklif yapılabilir mi alanını false setle
                        IsOfferable = false,
                        IsSold = true,
                        Image = result.Result.Image,
                        Name = result.Result.Name,
                        Price = result.Result.Price,
                        ProductCondition = result.Result.ProductCondition
                    };
                
                    var resultt = await _unitOfWork.Product.Update(maplendi, user); // ürünü güncelle
                
                    return resultt;
                }
                else if (result.Result.IsSold is true) //eğer ürün çoktan satın alınmışsa
                {
                    return BadRequest("Bu ürün başka bir kullanıcı tarafından satın alınmıştır.");
                }
            }
   
            return NotFound(result);
        }


        

    }
}
