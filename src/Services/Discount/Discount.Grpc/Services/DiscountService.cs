using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly ILogger<DiscountService> _logger;
        private readonly IDiscountRepository _repository;
        private readonly IMapper _mapper;

        public DiscountService(ILogger<DiscountService> logger,
            IDiscountRepository repository,
            IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            //return base.GetDiscount(request, context);
            var coupon = await _repository.GetDiscount(request.ProductName);
            if (coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with product name {request.ProductName} is not found."));
            }

            _logger.LogInformation("Discount is retrieved for Product name: {name}, Amount: {amount}", coupon.ProductName, coupon.Amount);

            var couponModel = _mapper.Map<CouponModel>(coupon);

            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            //return base.CreateDiscount(request, context);
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            await _repository.CreateDiscount(coupon);

            _logger.LogInformation("Discount is successfully created Product name: {name}, Amount: {amount}", coupon.ProductName, coupon.Amount);

            var couponModel = _mapper.Map<CouponModel>(coupon);

            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            //return base.CreateDiscount(request, context);
            var coupon = _mapper.Map<Coupon>(request.Coupon);

            await _repository.CreateDiscount(coupon);

            _logger.LogInformation("Discount is successfully updated Product name: {name}, Amount: {amount}", coupon.ProductName, coupon.Amount);

            var couponModel = _mapper.Map<CouponModel>(coupon);

            return couponModel;
        }
        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            //return base.CreateDiscount(request, context);

            var result = await _repository.DeleteDiscount(request.ProductName);

            return new DeleteDiscountResponse
            {
                Success = result
            };
        }
    }
}
