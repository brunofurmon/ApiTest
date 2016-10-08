using ApiTest.Components;
using ApiTest.Daos;
using ApiTest.Dto;
using ApiTest.Models;
using EpicomTest.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


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
            OrderType orderType = GetOrderTypeFromString(orderString);

            switch (orderType)
            {
                case OrderType.SkuCreation:
                    Sku newSku = CreateSkuFromOrder(order);
                    skuDao.Create(newSku);
                    break;

                default:
                case OrderType.Invalid:
                    throw new OrderException("Error while trying to process a valid order");
            }

            return;
        }

        private OrderType GetOrderTypeFromString(string orderString)
        {
            OrderType orderType;
            switch (orderString)
            {
                case Constants.SkuCreationKey:
                    orderType = OrderType.SkuCreation;
                    break;
                default:
                    orderType = OrderType.Invalid;
                    break;
            }
            return orderType;
        }

        private Sku CreateSkuFromOrder(OrderForm form)
        {
            Sku createdSku = new Sku
            {
                ProductId = form.Parametros.IdProduto,
                SkuId = form.Parametros.IdSku,
                Price = form.Parametros.Preco
            };

            return createdSku;
        }
    }
}