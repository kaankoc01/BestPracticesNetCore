﻿using App.Application.Contracts.Persistence;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Persistence
{
    public class ProductRepository(AppDbContext context) : GenericRepository<Product,int>(context), IProductRepository
    {
        public Task<List<Product>> GetTopPriceProductsAsync(int count)
        {
            return Context.Products.OrderByDescending(x => x.Price).Take(count).ToListAsync();
        }
    }
}