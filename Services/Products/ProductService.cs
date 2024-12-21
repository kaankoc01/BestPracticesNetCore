using App.Repositories;
using App.Repositories.Products;
using App.Services.Products.Create;
using App.Services.Products.Update;
using App.Services.Products.UpdateStock;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace App.Services.Products
{
    public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork,IMapper mapper) : IProductService
    {
        public async Task<ServiceResult<List<ProductDto>>> GetTopPriceProductsAsync(int count)
        {
            var products = await productRepository.GetTopPriceProductsAsync(count);

            
            var productsAsDto = mapper.Map<List<ProductDto>>(products);

            return new ServiceResult<List<ProductDto>>()
            {
                Data = productsAsDto
            };

        }

        public async Task<ServiceResult<List<ProductDto>>> GetAllListAsync()
        {
            // list döndüğümüz yerlerde geriye null dönmeyelim , boş liste dönelim.
            var products = await productRepository.GetAll().ToListAsync();

            // manuel mapping
            // var productsAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();


            var productsAsDto =mapper.Map<List<ProductDto>>(products);

            return ServiceResult<List<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ServiceResult<List<ProductDto>>> GetPagedAllList(int pageNumber , int pageSize)
        {
            // eğer ki pagenumber 1 , page size da 10 gelirse , ilk 10 kayıt demek
            // 1- 10 => ilk 10 kayıt , skip(0).Take(10) deriz.
            // 2- 10 => 11-20 kayıt , skip(10).Take(10) deriz.

            var products = await productRepository.GetAll().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            //var productsAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList(); manuel mapping

            var productsAsDto = mapper.Map<List<ProductDto>>(products);

            return ServiceResult<List<ProductDto>>.Success(productsAsDto);


        }

        public async Task<ServiceResult<ProductDto?>> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product is null)
            {
                return ServiceResult<ProductDto?>.Fail("Product not found", HttpStatusCode.NotFound);
                
            }
            //var productAsDto = new ProductDto(product!.Id, product.Name, product.Price, product.Stock);
            var productsAsDto = mapper.Map<ProductDto>(product);

            return ServiceResult<ProductDto>.Success(productsAsDto)!;

        }

        public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest Request)
        {

            //   throw new CriticalException("kritik seviyede bir hata meydana geldi.");
            

            // async kod . 2. yol async validation
            var anyProduct = await productRepository.Where(x => x.Name == Request.Name).AnyAsync();
             
            if (anyProduct)
            {
                return ServiceResult<CreateProductResponse>.Fail("Ürün ismi veri tabanında bulunmaktadır.", HttpStatusCode.BadRequest);
            }


            var product = mapper.Map<Product>(Request);

            await productRepository.Addasync(product);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult<CreateProductResponse>.SuccessAsCreated(new CreateProductResponse(product.Id),$"api/products/{product.Id}");
        }

        // update ve delete de geriye bir şey dönmeyiz. 204 döneriz
        public async Task<ServiceResult> UpdateAsync(int id, UpdateProductRequest request)
        {
            // fast fail  önce olumsuz durumları dönmek.-> önce başarısız durumları döneriz

            // Guard Clauses - tüm olumsuz durumları if ile yaz ve return et.else yazma.
            // elseler kodun okunabilirliğini düşünüyor.


            var product = await productRepository.GetByIdAsync(id);

            if (product is null)
            {
                return ServiceResult.Fail("Ürün bulunamadı.", HttpStatusCode.NotFound);
            }
            // aynı isimde bir ürün var mı kontrol et. değiştireceğimiz kodun name 'i başka satırda varmı ona bakıyoruz aslında. mesela kalem3 isimli başka bir ürün var mı, varsa hata dönüyoruz.
            var isProductNameExist = await productRepository.Where(x => x.Name == request.Name && x.Id != product.Id).AnyAsync();
            if (isProductNameExist)
            {
                return ServiceResult.Fail("ürün ismi veri tabanında bulunmaktadır.", HttpStatusCode.BadRequest);
            }

            product.Name = request.Name;
            product.Price = request.Price;
            product.Stock = request.Stock;


            product = mapper.Map(request, product);

            productRepository.Update(product);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        // parametre 2 den fazla olduğu an da request  oluşturabiliriz.
        public async Task<ServiceResult> UpdateStockAsync(UpdateProductStockRequest request)
        {
            var product = await productRepository.GetByIdAsync(request.ProductId);

            if(product is null)
            {
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
            }

            product.Stock = request.Quantity;
            productRepository.Update(product);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);

        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product is null)
            {
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
            }
            productRepository.Delete(product);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}


