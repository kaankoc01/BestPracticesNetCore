
using App.Domain.Entities.Common;

namespace App.Domain.Entities
{
    public class Category :BaseEntity<int>, IAuditEntity
    {
        // 1 e çok ilişki 

        public string Name { get; set; } = default!;

        //bir kategorinin mutlaka ürünü olacak diye bir koşul yok.
        public List<Product>? Products { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
