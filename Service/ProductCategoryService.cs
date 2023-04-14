using ECommerceWeb.Data;
using ECommerceWeb.Interface;
using ECommerceWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWeb.Service
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly UserDbContext dbContext;
        public ProductCategoryService(UserDbContext context)
        {
            dbContext = context;
        }
       
        public List<ProductCategory> GetProductCategory()
        {
            return dbContext.ProductCategory.ToList();
        }

        public ProductCategory ProCategoryInfo(string productcategoryid)
        {
            var prdtid = new Guid(productcategoryid);
            ProductCategory productCategory = new ProductCategory();
            var product =  dbContext.ProductCategory.FirstOrDefault(m => m.ProductCategoryId == prdtid);
            if (product != null)
            {
                productCategory.ProductCategoryId = product.ProductCategoryId;
                productCategory.Name = product.Name;
                
                return productCategory;
            }
            return productCategory;
        }

        public int SaveProductCategory(ProductCategory model)
        {
            var retresult = 0;
            if (model.ProductCategoryId == new Guid())
            {
                ProductCategory productCategory = new ProductCategory
                {
                    Name = model.Name,
                };
                dbContext.ProductCategory.Add(productCategory);
                dbContext.SaveChanges();
                retresult = 1;
            }
            else
            {
                var data = dbContext.ProductCategory.FirstOrDefault(m => m.ProductCategoryId == model.ProductCategoryId);
                if (data != null)
                {
                    data.Name = model.Name;
                    dbContext.ProductCategory.Update(data);
                    dbContext.SaveChanges();
                    retresult = 2;
                }
            }
            return retresult;
        }
        public async Task<bool> DeleteProCategory(Guid productcategoryId)
        {
            var productCategory = await dbContext.ProductCategory.FindAsync(productcategoryId);
            dbContext.ProductCategory.Remove(productCategory);
            dbContext.SaveChanges();
            return await Task.FromResult(true);
        }
    }
}
