using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscountCodeSystem.Application.interfaces;
using DiscountCodeSystem.Domain;
using DiscountCodeSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DiscountCodeSystem.Infrastructure.Repositories
{
    public class DiscountCodeRepository : IDiscountCodeRepository
    {
        private readonly IDiscountCodeDbContext _context;
        private readonly ILogger<DiscountCodeRepository> _logger;

        public DiscountCodeRepository(IDiscountCodeDbContext context, ILogger<DiscountCodeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        

        public async Task<List<string>> GetAllCodesAsync()
        {
            try
            {
                return await _context.DiscountCodes.AsNoTracking().Select(dc => dc.Code).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all existing discount codes");
                throw;
            }
        }

        public async Task AddCodesAsync(List<DiscountCode> codes)
        {
            try
            {
                await _context.DiscountCodes.AddRangeAsync(codes);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error when adding codes");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error when adding discount codes");
                throw;
            }
        }

        public async Task<DiscountCode?> GetByCodeAsync(string code)
        {
            try
            {
                return await _context.DiscountCodes.FirstOrDefaultAsync(dc => dc.Code == code);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation when retrieving discount code: {Code}", code);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving discount code: {Code}", code);
                throw;
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update failed during SaveAsync()");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during SaveAsync()");
                throw;
            }
        }
    }
}
