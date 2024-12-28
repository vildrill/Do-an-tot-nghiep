using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Web1.Models
{
	public class ItemCart
	{
		protected static readonly MyDB db = new MyDB();
		//------        
		public static T GetObjectFromJson<T>(ISession session, string key)
		{
			var value = session.GetString(key);
			return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
		}
		//------
		//lay gio hang dang ton tai
		public static List<Item> GetCart(ISession session)
		{
			//JsonConvert.DeserializeObject<T>("cart")
			List<Item> cart = ItemCart.GetObjectFromJson<List<Item>>(session, "cart");
			return cart;
		}
		//add item to cart
		public static void CartAdd(ISession session, int id)
		{
			if (ItemCart.GetObjectFromJson<List<Item>>(session, "cart") == null)
			{
				List<Item> cart = new List<Item>();
				ItemSanPham item = db.Products.Where(tbl => tbl.Id_Product == id).FirstOrDefault();
				cart.Add(new Item { ProductRecord = item, Quantity = 1 });
				session.SetString("cart", JsonConvert.SerializeObject(cart));
			}
			else
			{
				List<Item> cart = ItemCart.GetObjectFromJson<List<Item>>(session, "cart");

				int index = ItemCart.isExist(session, id);
				if (index != -1)
				{
					cart[index].Quantity++;
				}
				else
				{
					ItemSanPham item = db.Products.Where(tbl => tbl.Id_Product == id).FirstOrDefault();
					cart.Add(new Item { ProductRecord = item, Quantity = 1 });
				}
				session.SetString("cart", JsonConvert.SerializeObject(cart));
			}
		}
		//remove item in cart
		public static void CartRemove(ISession session, int id)
		{
			List<Item> cart = ItemCart.GetObjectFromJson<List<Item>>(session, "cart");
			int index = isExist(session, id);
			cart.RemoveAt(index);
			session.SetString("cart", JsonConvert.SerializeObject(cart));
		}
		//remove all item in cart
		public static void CartDestroy(ISession session)
		{
			List<Item> cart = new List<Item>();
			session.SetString("cart", JsonConvert.SerializeObject(cart));
		}
		public static void CartUpdate(ISession session, int id, int quantity)
		{
			List<Item> cart = ItemCart.GetObjectFromJson<List<Item>>(session, "cart");
			//---
			for (int i = 0; i < cart.Count; i++)
			{
				if (cart[i].ProductRecord.Id_Product == id)
				{
					cart[i].Quantity = quantity;
				}
			}
			//---
			session.SetString("cart", JsonConvert.SerializeObject(cart));
		}
		private static int isExist(ISession session, int id)
		{
			List<Item> cart = ItemCart.GetObjectFromJson<List<Item>>(session, "cart");
			for (int i = 0; i < cart.Count; i++)
			{
				if (cart[i].ProductRecord.Id_Product == id)
				{
					return i;
				}
			}
			return -1;
		}
	}
}
