using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace App.Repositories.Interceptors
{
    public class AuditDbContextIntercepter : SaveChangesInterceptor
    {
        // delegeler metotları işaret ediyor. ileri düzey bir konu !!!!!!!!!!
        private static readonly Dictionary<EntityState, Action<DbContext, IAuditEntity>> _behaviors = new()
        {
            { EntityState.Added, AddBehavior },
            { EntityState.Modified, ModifiedBehavior }
        };

        private static void AddBehavior(DbContext context, IAuditEntity auditEntity)
        {
            auditEntity.Created = DateTime.Now;
            context.Entry(auditEntity).Property(x => x.Updated).IsModified = false;
        }
        private static void ModifiedBehavior(DbContext context, IAuditEntity auditEntity)
        {
            context.Entry(auditEntity).Property(x => x.Created).IsModified = false;
            auditEntity.Updated = DateTime.Now;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {

            foreach (var entityEntry in eventData.Context!.ChangeTracker.Entries().ToList())
            {

                if (entityEntry.Entity is not IAuditEntity auditEntity) continue;

                _behaviors[entityEntry.State](eventData.Context, auditEntity);

                //switch bloguna gerek kalmadı
                //çünkü yukarıda dictionary oluşturduk ve bu dictionary içerisinde key olarak EntityState'leri, value olarak da metotları tutuyoruz.
                //switch (entityEntry.State)
                //{
                //    case EntityState.Added:

                //        AddBehavior(eventData.Context, auditEntity);

                //        break;
                //    case EntityState.Modified:

                //        ModifiedBehavior(eventData.Context, auditEntity);

                //        break;

                //}

            }
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
