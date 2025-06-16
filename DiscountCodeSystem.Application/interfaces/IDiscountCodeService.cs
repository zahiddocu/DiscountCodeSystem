using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCodeSystem.Application.interfaces
{
    public interface IDiscountCodeService
    {
        Task<List<string>> GenerateCodesAsync(int count, int length);
        Task<byte> UseCodeAsync(string code);
    }
}
