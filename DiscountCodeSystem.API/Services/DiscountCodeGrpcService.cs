using DiscountCodeSystem.Application.interfaces;
using DiscountCodeSystem.Application.services;
using DiscountServiceProto;
using Grpc.Core;

namespace DiscountCodeSystem.API.Services
{
    public class DiscountCodeGrpcService : DiscountService.DiscountServiceBase
    {
    
        private readonly IDiscountCodeService _discountCodeService;
        private readonly ILogger<DiscountCodeGrpcService> _logger;

        public DiscountCodeGrpcService(IDiscountCodeService discountCodeService, ILogger<DiscountCodeGrpcService> logger)
        {
            _discountCodeService = discountCodeService;
            _logger = logger;
        }

        public override async Task<GenerateResponse> GenerateCodes(GenerateRequest request, ServerCallContext context)
        {
            this.ValidateRequest(request);

            try
            {
                var codes = await _discountCodeService.GenerateCodesAsync((int)request.Count, (int)request.Length);
                return BuildGenerateResponse(codes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GenerateCodes gRPC call failed.");
                throw new RpcException(new Status(StatusCode.Internal, "Code generation failed due to a server error."));
            }
        }


        public override async Task<UseCodeResponse> UseCode(UseCodeRequest request, ServerCallContext context)
        {
            var response = new UseCodeResponse();

            try
            {
                response.Result = await _discountCodeService.UseCodeAsync(request.Code);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UseCode gRPC call failed.");
                throw new RpcException(new Status(StatusCode.Internal, "Code usage failed."));
            }
        }


        private void ValidateRequest(GenerateRequest request)
        {
            if (request.Count > 2000)
            {
                _logger.LogWarning("GenerateCodes request rejected: Count exceeds limit. Count = {Count}", request.Count);
                throw new RpcException(
                    new Status(StatusCode.InvalidArgument, "Maximum allowed code count is 2000.")
                );
            }

            if (request.Length < 7 || request.Length > 8)
            {
                _logger.LogWarning("GenerateCodes request rejected: Length out of bounds. Length = {Length}", request.Length);
                throw new RpcException(
                    new Status(StatusCode.InvalidArgument, "Code length must be between 7 and 8 characters.")
                );
            }
        }

        private GenerateResponse BuildGenerateResponse(List<string> codes)
        {
            var response = new GenerateResponse();
            response.Codes.AddRange(codes);
            return response;
        }
    }
}
