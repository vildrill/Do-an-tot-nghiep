using Web1.Models;
using System.Collections.Generic;
using System.Linq;


namespace Web1.MyClass
{
    public class Helper
    {
        public static MyDB db = new MyDB();
        //lay ten san pham
        public static string GetCategoryname(int _CategoryId)
        {
            ItemLoaiSP record = db.Categories.Where(item => item.Id_Category == _CategoryId).FirstOrDefault();
            return record.Name;

        }
    }
}
