using ECommerceWeb.Data;
using ECommerceWeb.Interface;
using ECommerceWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWeb.Service
{
    public class HomeService : IHomeService
    {
        private readonly UserDbContext _dbContext;
        public HomeService(UserDbContext context)
        {
            _dbContext = context;
        }
        public Products GetProductById(Guid ProductId)
        {
            var product = _dbContext.Products.FirstOrDefault(m => m.ProductId == ProductId);
            return product;
        }
        public async Task<ProductParameters> IndexProducts(int currentPage)
        {
            if(currentPage == 0)
            {
                currentPage = 1;
            }
            ProductParameters productParameter = new ProductParameters();

            productParameter.products = (from Products in _dbContext.Products
                                         select Products)
                            .OrderBy(products => products.ProductName)
                            .Skip((currentPage - 1) * productParameter.PageSize)
                            .Take(productParameter.PageSize).ToList();

            double pageCount = (double)((decimal)_dbContext.Products.Count() / Convert.ToDecimal(productParameter.PageSize));
            productParameter.PageCount = (int)Math.Ceiling(pageCount);

            productParameter.PageNumber = currentPage;

            return productParameter;
        }
        public ProductParameters ProductSearch(string search)
        {
            var data = _dbContext.Products
                                .FromSqlRaw($"ProductSearch {search}")
                                .ToList();
            var currentPage = 1;
            ProductParameters parameters = new ProductParameters();
            parameters.products = (data).OrderBy(products => products.ProductName)
                                .Skip((currentPage - 1) * parameters.PageSize)
                                .Take(parameters.PageSize).ToList();
            double pageCount = (double)(data.Count() / Convert.ToDecimal(parameters.PageSize));
            parameters.PageCount = (int)Math.Ceiling(pageCount);

            parameters.PageNumber = currentPage;
            return parameters;
        }
    }
}
