using ApiTest.Components;
using ApiTest.Daos;
using ApiTest.Dto;
using ApiTest.Models;
using EpicomTest.Exceptions;


namespace ApiTest.Services
{
    public interface IOrderService
    {
        void ProcessOrder(OrderForm order);
    }

    public class OrderService: IOrderService
    {
        private IGenericDao<Sku> skuDao { get; set; }

        public OrderService() : base()
        {
            this.skuDao = new GenericDao<Sku>();
        }

        public void ProcessOrder(OrderForm order)
        {
            string orderString = order.Tipo;
            ApiEnums.OrderType orderType = GetOrderTypeFromString(orderString);

            switch (orderType)
            {
                case ApiEnums.OrderType.SkuCreation:
                    Sku newSku = Sku.FromOrderForm(order);
                    skuDao.Create(newSku);
                    break;

                default:
                case ApiEnums.OrderType.Invalid:
                    throw new OrderException(string.Format("Error while trying to process an order. Null or Invalid Operation {0}", orderString));
            }

            return;
        }

        private ApiEnums.OrderType GetOrderTypeFromString(string orderString)
        {
            ApiEnums.OrderType orderType;
            switch (orderString)
            {
                case Constants.SkuCreationKey:
                    orderType = ApiEnums.OrderType.SkuCreation;
                    break;
                default:
                    orderType = ApiEnums.OrderType.Invalid;
                    break;
            }
            return orderType;
        }
    }
}