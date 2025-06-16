using DiscountCodeSystem.Application.interfaces;
using DiscountCodeSystem.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscountCodeSystem.Infrastructure.Data;
using DiscountCodeSystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DiscountCodeSystem.Tests.Repositories
{
    public class DiscountCodeRepositoryTests
    {
        private DiscountDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<DiscountDbContext>()
                .UseInMemoryDatabase(databaseName: $"DiscountDb_{Guid.NewGuid()}")
                .Options;

            return new DiscountDbContext(options);
        }

        [Fact]
        public async Task AddCodesAsync_ShouldAddCodes()
        {
            
            var context = GetDbContext();
            var logger = new Mock<ILogger<DiscountCodeRepository>>();
            var repo = new DiscountCodeRepository(context, logger.Object);

            var codes = new List<DiscountCode>
            {
                new DiscountCode { Code = "ABC123" },
                new DiscountCode { Code = "XYZ789" }
            };

           
            await repo.AddCodesAsync(codes);
            await repo.SaveAsync();

           
            var savedCodes = await context.DiscountCodes.ToListAsync();
            Assert.Equal(2, savedCodes.Count);
        }

       

        [Fact]
        public async Task GetByCodeAsync_ShouldReturnCorrectCode()
        {
            var context = GetDbContext();
            var logger = new Mock<ILogger<DiscountCodeRepository>>();
            var repo = new DiscountCodeRepository(context, logger.Object);

            var expected = new DiscountCode { Code = "FINDME", IsUsed = false };
            context.DiscountCodes.Add(expected);
            await context.SaveChangesAsync();

            var result = await repo.GetByCodeAsync("FINDME");

            Assert.NotNull(result);
            Assert.Equal("FINDME", result.Code);
        }
        //
        [Fact]
        public async Task GetAllCodesAsync_ShouldReturnAllCodes()
        {
            var context = GetDbContext();
            var logger = new Mock<ILogger<DiscountCodeRepository>>();
            var repo = new DiscountCodeRepository(context, logger.Object);

            context.DiscountCodes.AddRange(new[]
            {
                new DiscountCode { Code = "A1" },
                new DiscountCode { Code = "B2" }
            });
            await context.SaveChangesAsync();

            var codes = await repo.GetAllCodesAsync();

            Assert.Contains("A1", codes);
            Assert.Contains("B2", codes);
        }
    }
}
