using App.Repositories.Products;

namespace App.Repositories.Categories
{
    public class Category
    {
        // 1 e çok ilişki 
        public int Id { get; set; }
        public string Name { get; set; }

        //bir kategorinin mutlaka ürünü olacak diye bir koşul yok.
        public List<Product>? Products { get; set; }
    }
}
