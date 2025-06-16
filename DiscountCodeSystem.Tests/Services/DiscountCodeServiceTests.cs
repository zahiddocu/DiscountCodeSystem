using DiscountCodeSystem.Application.common;
using DiscountCodeSystem.Application.interfaces;
using DiscountCodeSystem.Application.services;
using DiscountCodeSystem.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace DiscountCodeSystem.Tests.Services
{
    public class DiscountCodeServiceTests
    {
        private readonly Mock<IDiscountCodeRepository> _mockRepo;
        private readonly DiscountCodeService _service;

        public DiscountCodeServiceTests()
        {
            _mockRepo = new Mock<IDiscountCodeRepository>();
            var logger = new Mock<ILogger<DiscountCodeService>>();
            _service = new DiscountCodeService(_mockRepo.Object, logger.Object);
        }

        [Fact]
        public async Task GenerateCodesAsync_ShouldReturnExpectedNumberOfCodes()
        {
            
            int count = 5;
            int length = 7;

            _mockRepo.Setup(repo => repo.AddCodesAsync(It.IsAny<List<DiscountCode>>()))
                .Returns(Task.CompletedTask);

            _mockRepo.Setup(repo => repo.SaveAsync())
                .Returns(Task.CompletedTask);

            var result = await _service.GenerateCodesAsync(count, length);

            
            Assert.Equal(count, result.Count);
            Assert.All(result, code => Assert.Equal(length, code.Length));
        }

        [Fact]
        public async Task UseCodeAsync_ShouldReturnSuccess_WhenCodeIsValidAndNotUsed()
        {
            
            string code = "TEST123";
            var discountCode = new DiscountCode { Code = code, IsUsed = false };

            _mockRepo.Setup(r => r.GetByCodeAsync(code)).ReturnsAsync(discountCode);
            _mockRepo.Setup(r => r.SaveAsync()).Returns(Task.CompletedTask);

           
            var result = await _service.UseCodeAsync(code);

          
            Assert.Equal(DiscountCodeConstants.SUCCESS, result);
        }

        [Fact]
        public async Task UseCodeAsync_ShouldReturnInvalid_WhenCodeIsNotFound()
        {
          
            string code = "UNKNOWN";
            _mockRepo.Setup(r => r.GetByCodeAsync(code)).ReturnsAsync((DiscountCode?)null);

           
            var result = await _service.UseCodeAsync(code);

          
            Assert.Equal(DiscountCodeConstants.INVALID_CODE, result);
        }

        [Fact]
        public async Task UseCodeAsync_ShouldReturnAlreadyUsed_WhenCodeIsUsed()
        {
            string code = "USED123";
            var discountCode = new DiscountCode { Code = code, IsUsed = true };
            _mockRepo.Setup(r => r.GetByCodeAsync(code)).ReturnsAsync(discountCode);

            
            var result = await _service.UseCodeAsync(code);

            
            Assert.Equal(DiscountCodeConstants.ALREADY_USED_CODE, result);
        }
    }
}
