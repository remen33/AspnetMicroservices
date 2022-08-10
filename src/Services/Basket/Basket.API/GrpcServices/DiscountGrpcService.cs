using Discount.Grpc.Protos;

namespace Basket.API.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoServices.DiscountProtoServicesClient discountProtoServicesClient;

        public DiscountGrpcService(DiscountProtoServices.DiscountProtoServicesClient discountProtoServicesClient)
        {
            this.discountProtoServicesClient = discountProtoServicesClient;
        }

        public async Task<CouponModel> GetDiscount(string productName) 
        { 
            var discountRequest =  new GetDiscountRequest { PrductName = productName };

            return await this.discountProtoServicesClient.GetDiscountAsync(discountRequest);
        }
    }
}
