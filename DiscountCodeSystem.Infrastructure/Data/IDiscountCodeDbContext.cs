using DiscountCodeSystem.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCodeSystem.Infrastructure.Data
{
    public interface IDiscountCodeDbContext
    {
        DbSet<DiscountCode> DiscountCodes { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
