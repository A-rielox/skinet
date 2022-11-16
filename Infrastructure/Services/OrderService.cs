using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {

        /*private readonly IGenericRepository<Order> _orderRepo;
        private readonly IGenericRepository<DeliveryMethod> _dmRepo;
        private readonly IGenericRepository<Product> _productRepo;*/
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepo;

        public OrderService(/*IGenericRepository<Order> orderRepo,
                            IGenericRepository<DeliveryMethod> dmRepo,
                            IGenericRepository<Product> productRepo,*/
                            IUnitOfWork unitOfWork,
                            IBasketRepository basketRepo)
        {
            /*_orderRepo = orderRepo;
            _dmRepo = dmRepo;
            _productRepo = productRepo;*/
            _unitOfWork = unitOfWork;
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

            // si NO existe la basket => me da exception en el foreach ( ya q es null y quiero acceder a
            // .Items )
            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name,
                    productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);

                items.Add(orderItem);
            }

            // ----- get delivery method from repo
            var delieveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(delieveryMethodId);

            // ----- calc subtotal
            var subtotal = items.Sum(i => i.Quantity * i.Price);

            // ----- create order
            var order = new Order(items, buyerEmail, shippingAddress, delieveryMethod, subtotal);

            // ----- save to db
            _unitOfWork.Repository<Order>().Add(order);

            var result = await _unitOfWork.Complete();

            if(result <= 0) return null;

            // ----- si se guarda la order => borro la basket
            await _basketRepo.DeleteBasketAsync(basketId);

            // ----- return order
            return order;
        }

        ////////////////////////////////////////////
        ////////////////////////////////////////////
        //
        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        ////////////////////////////////////////////
        ////////////////////////////////////////////
        //
        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);

            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        ////////////////////////////////////////////
        ////////////////////////////////////////////
        //
        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);

            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }
    }
}
