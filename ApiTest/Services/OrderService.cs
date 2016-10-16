using ApiTest.Components;
using ApiTest.Daos;
using ApiTest.Dto;
using ApiTest.Models;
using ApiTest.Exceptions;


namespace ApiTest.Services
{
    public interface IOrderService
    {
        void ProcessOrder(OrderForm order);
    }

    public class OrderService: IOrderService
    {
        public OrderService() : base()
        {
        }

        public void ProcessOrder(OrderForm order)
        {
            string orderString = order.Tipo;
            ApiEnums.OrderType orderType = GetOrderTypeFromString(orderString);
            Parametros parametros = order.Parametros;
            switch (orderType)
            {
                case ApiEnums.OrderType.SkuCreation:
                    // Currently, this does nothing, in favor of automatic insertion
                    Sku newSku = new Sku { Id = parametros.IdSku };
                    using (AbstractDao<Sku> dao = new GenericDao<Sku>())
                    {
                        dao.Create(newSku);
                    }
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