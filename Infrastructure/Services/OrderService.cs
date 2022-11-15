using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly IGenericRepository<DeliveryMethod> _dmRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IBasketRepository _basketRepo;

        public OrderService(IGenericRepository<Order> orderRepo,
            IGenericRepository<DeliveryMethod> dmRepo, IGenericRepository<Product> productRepo,
            IBasketRepository basketRepo)
        {
            _orderRepo = orderRepo;
            _dmRepo = dmRepo;
            _productRepo = productRepo;
            _basketRepo = basketRepo;
        }

        ////////////////////////////////////////////
        ////////////////////////////////////////////
        //
        public async Task<Order> CreateOrderAsync(string buyerEmail, int delieveryMethodId,
            string basketId, Address shippingAddress)
        {
            // ----- get basket from the repo
            var basket = await _basketRepo.GetBasketAsync(basketId);

            // ----- get items from the product repo
            // customerBasket tiene una lista de items
            var items = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var productItem = await _productRepo.GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id,productItem.Name,
                    productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);

                items.Add(orderItem);
            }

            // ----- get delivery method from repo
            var delieveryMethod = await _dmRepo.GetByIdAsync(delieveryMethodId);

            // ----- calc subtotal
            var subtotal = items.Sum(i => i.Quantity * i.Price);

            // ----- create order
            var order = new Order(items, buyerEmail, shippingAddress, delieveryMethod, subtotal);

            // ----- save to db             TODO

            // ----- return order
            return order;
        }

        ////////////////////////////////////////////
        ////////////////////////////////////////////
        //
        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }

        ////////////////////////////////////////////
        ////////////////////////////////////////////
        //
        public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            throw new NotImplementedException();
        }

        ////////////////////////////////////////////
        ////////////////////////////////////////////
        //
        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
