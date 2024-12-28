using Web1.Models;
using Microsoft.CodeAnalysis.Elfie.Model.Map;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Web.CodeGeneration.Templating;
using System.IO;
namespace Web1.Models
{
	public class MyDB : DbContext 
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//tao doi tuong de doc thong tin cua file appsettings.json
			var builder = new ConfigurationBuilder();
			//set duong dan cua file appsettings.json
			builder.SetBasePath(Directory.GetCurrentDirectory());
			//add file appsettings.json vao doi tuong builder
			builder.AddJsonFile("appsettings.json");
			var configuration = builder.Build();
			//doc chuoi ket noi o trong file appsettings.json
			string strDbConnectString = configuration.GetConnectionString("DBConnectString").ToString();
			//ket noi voi csdl thong qua chuoi ket noi
			optionsBuilder.UseSqlServer(strDbConnectString);
		}

		public DbSet<ItemTaiKhoan> Users { get; set; } 
		public DbSet<ItemSanPham> Products { get; set; }
        public DbSet<ItemKichCo> Sizes { get; set; }
        public DbSet<ItemMau> Colors { get; set; }
        public DbSet<ItemLoaiSP> Categories { get; set; }
        public DbSet<ItemOrder> Orders { get; set; }
        public DbSet<ItemOrderDetail> OrdersDetail { get; set; }
		public DbSet<ItemRating> Rating { get; set; }
		public DbSet<ItemDiscount> Discount { get; set; }
		public DbSet<ItemPayment> Payment { get; set; }
		public DbSet<City> City { get; set; }
		public DbSet<District> District { get; set; }
		public DbSet<Ward> Ward { get; set; }


		//public DbSet<ItemCategory> Categories { get; set; }
		/*public DbSet<ItemProducts> Products { get; set; }
		public DbSet<ItemCategoriesProducts> CategoriesProducts { get; set; }
		public DbSet<ItemTag> Tags { get; set; }
		public DbSet<ItemTagsProducts> TagsProducts { get; set; }
		public DbSet<ItemNews> News { get; set; }//<=> table News
		public DbSet<ItemRating> Rating { get; set; }//<=> table Rating
		public DbSet<ItemCustomers> Customers { get; set; }//<=> table Customers
		public DbSet<ItemOrders> Orders { get; set; }//<=> table Orders
		public DbSet<ItemOrdersDetail> OrdersDetail { get; set; }//<=> table OrdersDetail
		public DbSet<ItemAdv> Adv { get; set; }//<=> table Adv
		public DbSet<ItemSlide> Slides { get; set; }//<=> table Slides
		public DbSet<ItemBlog> Blog { get; set; }//<=> table Blog*/

	}
}