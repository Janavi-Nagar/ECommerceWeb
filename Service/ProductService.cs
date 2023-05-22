using ECommerceWeb.Data;
using ECommerceWeb.Interface;
using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWeb.Service
{
    public class ProductService : IProductService
    {
        private readonly UserDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductService(IWebHostEnvironment hostEnvironment, UserDbContext context)
        {
            dbContext = context;
            webHostEnvironment = hostEnvironment;
        }

        public List<Products> Products()
        {
            return dbContext.Products.ToList();
        }
        public async Task<IQueryable<ProductViewModel>> GetProducts()
        {
            var data = from prd in dbContext.Products
                       join cat in dbContext.ProductCategory
                       on prd.ProductCategoryId equals cat.ProductCategoryId
                       select new ProductViewModel
                       {
                           ProductId = prd.ProductId,
                           ProductName = prd.ProductName,
                           Price = prd.Price,
                           ProductImage = prd.ProductPicture,
                           InStock = prd.InStock,
                           ProductCategoryName = cat.Name,
                           ProductCategoryId = prd.ProductCategoryId
                       };
            return data;
        }

        public async Task<List<Products>> SellerProducts(string userId)
        {
            var UserId = new Guid(userId);
            var data = await dbContext.Products
                        .Where(m => m.UserId == UserId)
                        .ToListAsync();
            return data;
        }
        public Products GetProductById(Guid ProductId)
        {
            var product = dbContext.Products.FirstOrDefault(m => m.ProductId == ProductId);
            return product;
        }

        public async Task<ProductViewModel> GetProductInfo(string Id)
        {
            var productid = new Guid(Id);
            ProductViewModel productViewModel = new ProductViewModel();
            var product = await dbContext.Products.FirstOrDefaultAsync(m => m.ProductId == productid);
            var data = await dbContext.ProductCategory.FirstOrDefaultAsync(m => m.ProductCategoryId == product.ProductCategoryId);
            if (product != null)
            {
                productViewModel.ProductId = product.ProductId;
                productViewModel.ProductName = product.ProductName;
                productViewModel.Price = product.Price;
                productViewModel.InStock = product.InStock;
                productViewModel.ProductImage = product.ProductPicture;
                productViewModel.ProductCategoryId = product.ProductCategoryId;
                productViewModel.ProductCategoryName = data.Name;
                return productViewModel;
            }
            return productViewModel;
        }
        private string UploadedFile(ProductViewModel model)
        {
            string uniqueFileName = null;

            if (model.ProductPicture != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProductPicture.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProductPicture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        public Task<int> SaveProductData(ProductViewModel model)
        {
            var retresult = 0;
            if (model.ProductId == new Guid())
            {
                string uniqueFileName = UploadedFile(model);
                Products product = new Products
                {
                    ProductName = model.ProductName,
                    Price = model.Price,
                    InStock = model.InStock,
                    //ProductCategory = productCategory,
                    ProductCategoryId = model.ProductCategoryId
                };
                Productimage(model, product);
                dbContext.Products.Add(product);
                dbContext.SaveChanges();
                retresult = 1;
            }
            else
            {
                var product = dbContext.Products.FirstOrDefault(m => m.ProductId == model.ProductId);
                if (product != null)
                {
                    product.ProductName = model.ProductName;
                    product.Price = model.Price;
                    product.InStock = model.InStock;
                    Productimage(model, product);
                    dbContext.Products.Update(product);
                    dbContext.SaveChanges();
                    retresult = 2;
                }
            }
            return Task.FromResult(retresult);
        }
       
        private void Productimage(ProductViewModel model, Products product)
        {
            if (model.ProductPicture != null)
            {
                if(product.ProductPicture != null)
                {
                    string filePath = Path.Combine(webHostEnvironment.WebRootPath, "images", product.ProductPicture);
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }
                product.ProductPicture = UploadedFile(model);
            }
        }
        public async Task<bool> DeleteProduct(Guid productId)
        {
            var product = await dbContext.Products.FindAsync(productId);
            var CurrentImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");
            dbContext.Products.Remove(product);
            dbContext.SaveChanges();
            if (await dbContext.SaveChangesAsync() > 0)
            {
                if (System.IO.File.Exists(CurrentImage))
                {
                    System.IO.File.Delete(CurrentImage);
                }
            }
            return await Task.FromResult(true);
        }
    }
}
