using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Products
{
    // bildiğimiz class gibi record.
    public record ProductDto(int Id, string Name , decimal Price ,int Stock, int CategoryId);


    //public record ProductDto
    //{
    //    public int Id { get; init; }
    //    public string Name { get; init; }
    //    public decimal Price { get; init; }
    //    public int Stock { get; init; }
    //}
}
