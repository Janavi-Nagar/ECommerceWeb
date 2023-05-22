using ECommerceWeb.Data;
using ECommerceWeb.Interface;
using ECommerceWeb.Models;
using ECommerceWeb.Models.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ECommerceWeb.Service
{
    public class HomeService : IHomeService
    {
        private readonly UserDbContext _dbContext;
        public HomeService(UserDbContext context)
        {
            _dbContext = context;
        }
        public ProductModel ProductSearch(ProductSearchViewModel searchmodel) 
        {
            ProductModel productParameters = new ProductModel();
            var builder = WebApplication.CreateBuilder();
            string conStr = builder.Configuration.GetConnectionString("UserDbContextConnection");
            DataTable result = new DataTable();

            SqlConnection con = new SqlConnection(conStr);
            SqlCommand cmd = new SqlCommand("ProductSearch", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("ProductName", string.IsNullOrEmpty(searchmodel.searchtext) ? "" : searchmodel.searchtext); // Check how to add parameter with value
            cmd.Parameters.AddWithValue("PageSize", searchmodel.pagesize);
            cmd.Parameters.AddWithValue("PageNumber", searchmodel.pagenumber);

            SqlParameter parm = new SqlParameter("@totalrecords", SqlDbType.Int);
            parm.Direction = ParameterDirection.Output; // Check diff in output and return value from sql SP
            cmd.Parameters.Add(parm);
            
            con.Open();
            cmd.ExecuteNonQuery();

            var adapt = new SqlDataAdapter(); // Check what is SqlDataAdapter
            adapt.SelectCommand = cmd;
            var dataset = new DataSet(); // Check diff Dataset and datatable
            adapt.Fill(dataset);

            int cnt = (int)cmd.Parameters["@totalrecords"].Value;
            double pageCount = (double)((decimal)cnt / Convert.ToDecimal(searchmodel.pagesize));
            productParameters.pagecount = (int)Math.Ceiling(pageCount);
            productParameters.PageNumber = searchmodel.pagenumber;

            List<Products> products = new List<Products>();
            for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
            {
                Products products1 = new Products();
                products1.ProductId = new Guid(dataset.Tables[0].Rows[i]["ProductId"].ToString());
                products1.ProductName = dataset.Tables[0].Rows[i]["ProductName"].ToString();
                products1.Price = Convert.ToInt16(dataset.Tables[0].Rows[i]["Price"].ToString());
                products1.InStock = Convert.ToBoolean(dataset.Tables[0].Rows[i]["InStock"].ToString());
                products1.ProductPicture = dataset.Tables[0].Rows[i]["ProductPicture"].ToString();
                products1.ProductCategoryId = new Guid(dataset.Tables[0].Rows[i]["ProductCategoryId"].ToString());
                products.Add(products1);
            }
            productParameters.products = products;
            return productParameters;
        }
    }
}
