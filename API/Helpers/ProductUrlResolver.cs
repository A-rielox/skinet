using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    // string es el tipo de lo que devuelve ( el PictureUrl )
    public class ProductUrlResolver : IValueResolver<Product, ProductToReturnDto, string>
    {
        private readonly IConfiguration _config;

        public ProductUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        //////////////////////////////////////
        //////////////////////////////////////////
        public string Resolve(  Product source, 
                                ProductToReturnDto destination, 
                                string destMember,
                                ResolutionContext context)
        {
            // no deberia necesitar el if y el " return null; " xq la prop esta coono notNull, p' xsi
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _config["ApiUrl"] + source.PictureUrl;
            }

            return null;
        }
    }
}
