using DiscountCodeSystem.Application.interfaces;
using DiscountCodeSystem.Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using DiscountCodeSystem.Application.common;

namespace DiscountCodeSystem.Application.services
{
    public class DiscountCodeService : IDiscountCodeService
    {
       
        private readonly IDiscountCodeRepository _discountCodeRepository;
        private readonly ILogger<DiscountCodeService> _logger;
        private const int MaxRetries = 5;

        public DiscountCodeService(IDiscountCodeRepository discountCodeRepository, ILogger<DiscountCodeService> logger)
        {
            _discountCodeRepository = discountCodeRepository;
            _logger = logger;
        }

        public async Task<List<string>> GenerateCodesAsync(int count, int length)
        {
            var resultCodes = new HashSet<string>();
            var random = new Random();
            var charset = DiscountCodeConstants.DiscountCodeCharacterSet;
           
            int retry = 0;

            try
            {
                while (resultCodes.Count < count && retry < DiscountCodeConstants.MAX_RETRIES)
                {
                    int needed = count - resultCodes.Count;
                    var batch = GenerateRandomCodes(needed, length, charset, random);

                    //var existingCodes = await _discountCodeRepository.GetAllCodesAsync();
                    //var newCodes = batch.Except(existingCodes).ToList();

                    var codeEntities = batch.Select(c => new DiscountCode { Code = c }).ToList();

                    try
                    {
                        await _discountCodeRepository.AddCodesAsync(codeEntities);
                        await _discountCodeRepository.SaveAsync();

                        foreach (var code in batch)
                            resultCodes.Add(code);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Attempt {Retry} failed during code insertion. Remaining: {Remaining}", retry + 1, needed);
                        retry++;
                    }
                }

                if (resultCodes.Count < count)
                {
                    _logger.LogWarning("Only {Generated} out of {Requested} codes could be generated after {MaxRetries} retries.",
                        resultCodes.Count, count, DiscountCodeConstants.MAX_RETRIES);
                    throw new ApplicationException($"Unable to generate the requested {count} unique codes. Generated {resultCodes.Count}.");
                }

                return resultCodes.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate discount codes.");
                throw new ApplicationException("An error occurred while generating codes.", ex);
            }
        }

        private HashSet<string> GenerateRandomCodes(int count, int length, string charset, Random random)
        {
            var codes = new HashSet<string>();

            while (codes.Count < count)
            {
                var code = new string(Enumerable.Range(0, length)
                    .Select(_ => charset[random.Next(charset.Length)]).ToArray());
                codes.Add(code);
            }

            return codes;
        }

        public async Task<byte> UseCodeAsync(string code)
        {
            try
            {
                var discountCode = await _discountCodeRepository.GetByCodeAsync(code);

                if (discountCode == null) return DiscountCodeConstants.INVALID_CODE;
                if (discountCode.IsUsed) return DiscountCodeConstants.ALREADY_USED_CODE;

                discountCode.IsUsed = true;
                await _discountCodeRepository.SaveAsync();

                return DiscountCodeConstants.SUCCESS;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to use discount code: {Code}", code);
                throw new ApplicationException("An error occurred while using the code.", ex);
            }
        }
    }
}
