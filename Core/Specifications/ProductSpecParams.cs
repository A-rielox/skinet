namespace Core.Specifications
{
    public class ProductSpecParams
    {
        private const int MaxPageSize = 50;
        public int PageIndex { get; set; } = 1; // la pagina q devuelvo xdefault

        private int _pageSize = 6; // backing field. Cantidad xdefault de items
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public int? BrandId { get; set; } // p' Criteria
        public int? TypeId { get; set; } // p' Criteria
        public string Sort { get; set; }


        private string _search; // backing field.
        public string Search // p' Criteria
        {
            get => _search;
            set => _search = value.ToLower();
        }
    }
}
