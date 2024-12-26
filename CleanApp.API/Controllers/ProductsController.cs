using App.Application.Features.Products;
using App.Application.Features.Products.Create;
using App.Application.Features.Products.Update;
using App.Application.Features.Products.UpdateStock;
using App.Domain.Entities;
using CleanApp.API.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CleanApp.API.Controllers
{
    public class ProductsController(IProductService productService) : CustomBaseController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => CreateActionResult(await productService.GetByIdAsync(id));

        [HttpGet]
        public async Task<IActionResult> GetAll() => CreateActionResult(await productService.GetAllListAsync());

        [HttpGet("{pageNumber:int}/{pageSize:int}")] // route constraint
        public async Task<IActionResult> GetPagedAll(int pageNumber, int pageSize) => CreateActionResult(await productService.GetPagedAllList(pageNumber, pageSize));

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequest request) => CreateActionResult(await productService.CreateAsync(request));

        [ServiceFilter(typeof(NotFoundFilter<Product, int>))]
        [HttpPut("{id:int}")]  // tam güncelleme
        public async Task<IActionResult> Update(int id, UpdateProductRequest request) => CreateActionResult(await productService.UpdateAsync(id, request));

        [HttpPatch("stock")] // kısmi güncelleme patch ile 
        public async Task<IActionResult> UpdateStock(UpdateProductStockRequest request) => CreateActionResult(await productService.UpdateStockAsync(request));

        [ServiceFilter(typeof(NotFoundFilter<Product,int>))]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) => CreateActionResult(await productService.DeleteAsync(id));
               
    }
}
