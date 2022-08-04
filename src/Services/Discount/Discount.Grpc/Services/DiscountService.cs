using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repostories;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoServices.DiscountProtoServicesBase
    {
        private readonly IDiscountRepository repostory;

        private readonly ILogger<DiscountService> logger;

        public DiscountService(IDiscountRepository repostory, ILogger<DiscountService> logger, IMapper mapper)
        {
            this.repostory = repostory;
            this.logger = logger;
            Mapper = mapper;
        }

        public IMapper Mapper { get; }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = Mapper.Map<Coupon>(request.Coupon);
            
            await this.repostory.CreateDiscount(coupon);

            var couponModel = Mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await this.repostory.DeleteDiscount(request.ProductName);
            var response = new DeleteDiscountResponse
            {
                Success = deleted
            };

            return response;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var  coupon = await this.repostory.GetDiscount(request.PrductName);

            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName ={request.PrductName} is not found."));
            }

            logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

            var couponModel = Mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = Mapper.Map<Coupon>(request.Coupon);

            await this.repostory.UpdateDiscount(coupon);
            logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", coupon.ProductName);

            var couponModel = Mapper.Map<CouponModel>(coupon);
            return couponModel;
        }
    }
}