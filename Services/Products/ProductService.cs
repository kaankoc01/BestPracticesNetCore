﻿using App.Repositories;
using App.Repositories.Products;
using System.Net;

namespace App.Services.Products
{
    public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork) : IProductService
    {
        public async Task<ServiceResult<List<ProductDto>>> GetTopPriceProductsAsync(int count)
        {
            var products = await productRepository.GetTopPriceProductsAsync(count);

            // manuel mapping
            var productsAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();

            return new ServiceResult<List<ProductDto>>()
            {
                Data = productsAsDto
            };

        }

        public async Task<ServiceResult<ProductDto>> GetProductByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product is null)
            {
                ServiceResult<ProductDto>.Fail("Product not found", HttpStatusCode.NotFound);
            }
            var productAsDto = new ProductDto(product!.Id, product.Name, product.Price, product.Stock);

            return ServiceResult<ProductDto>.Success(productAsDto!);

        }

        public async Task<ServiceResult<CreateProductResponse>> CreateProductAsync(CreateProductRequest Request)
        {
            var product = new Product()
            {
                Name = Request.Name,
                Price = Request.Price,
                Stock = Request.Stock
            };

            await productRepository.Addasync(product);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult<CreateProductResponse>.Success(new CreateProductResponse(product.Id));
        }
        // update ve delete de geriye bir şey dönmeyiz. 204 döneriz

        public async Task<ServiceResult> UpdateProductAsync(int id, UpdateProductRequest request)
        {
            // fast fail  önce olumsuz durumları dönmek.-> önce başarısız durumları döneriz

            // Guard Clauses - tüm olumsuz durumları if ile yaz ve return et.else yazma.
            // elseler kodun okunabilirliğini düşünüyor.


            var product = await productRepository.GetByIdAsync(id);

            if (product is null)
            {
                return ServiceResult.Fail("Product not found", HttpStatusCode.NotFound);
            }
            
            product.Name = request.Name;
            product.Price = request.Price;
            product.Stock = request.Stock;

            productRepository.Update(product);
            await unitOfWork.SaveChangesAsync();

            return ServiceResult.Success();
        }
    }
}


