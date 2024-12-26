using App.Application.Contracts.Persistence;
using FluentValidation;

namespace App.Application.Features.Products.Update
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {

        private readonly IProductRepository _productRepository;
        
        public UpdateProductRequestValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ürün ismi gereklidir.")
                .Length(3, 10).WithMessage("Ürün ismi 3 ile 10 karakter arasında olmalıdır.");
            //1.yol  .Must(MustUniqueProductName).WithMessage("Bu isimde bir ürün zaten mevcut.");

            // price validation
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Ürün fiyatı 0'dan büyük olmalıdır.");

            // Category ıd validation
            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Ürün Kategori Değeri 0'dan büyük olmalıdır.");

            // stock inclusiveBetween validation
            RuleFor(x => x.Stock).InclusiveBetween(1, 100).WithMessage("Stok adedi 1 ile 100 arasında olmalıdır.");
        }

        // 1.yol(sync)
        //private bool MustUniqueProductName(string name)
        //{
        //    return !_productRepository.Where(x => x.Name == name).Any();

        //    // false => bir hata var
        //    // true => bir hata yok

        //}
    }
}
