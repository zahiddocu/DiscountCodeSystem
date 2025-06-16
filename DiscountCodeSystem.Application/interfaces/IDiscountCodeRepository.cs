using DiscountCodeSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCodeSystem.Application.interfaces
{
    public interface IDiscountCodeRepository
    {

   
    
        Task AddCodesAsync(List<DiscountCode> codes);
        Task<List<string>> GetAllCodesAsync();
        Task<DiscountCode?> GetByCodeAsync(string code);
        Task SaveAsync();

    }
}
