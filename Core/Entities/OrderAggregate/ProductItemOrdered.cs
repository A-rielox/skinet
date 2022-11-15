namespace Core.Entities.OrderAggregate
{
    // snapshot del product al momento de la compra
    public class ProductItemOrdered
    {
        // NO tiene Id xq no va a ser una tabla, va a ser una prop dentro de una tabla ( la de order )

        public ProductItemOrdered()
        {// sin parametros p' no tener problemas en la migracion
        }

        public ProductItemOrdered(int productItemId, string productName, string pictureUrl)
        {
            ProductItemId = productItemId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }

        public int ProductItemId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
    }
}
