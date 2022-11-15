namespace Core.Entities.OrderAggregate
{
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        { // sin parametros p' no tener problemas en la migracion
        }

        public OrderItem(ProductItemOrdered itemOrdered, decimal price, int quantity)
        {
            ItemOrdered = itemOrdered;
            Price = price;
            Quantity = quantity;
        }

        public ProductItemOrdered ItemOrdered { get; set; }  // snapshot del product al momento de la compra
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
