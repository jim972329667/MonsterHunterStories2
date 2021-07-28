using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace MonsterHunterStories2
{

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string[] Phones { get; set; }
        public bool IsActive { get; set; }
    }
    class DataBase
    {
        public static string Path = @".\info\text.db";
        
        public static void MainDataBase()
        {
            using (var db = new LiteDatabase(Path))
            {
                var col = db.GetCollection<Customer>("customers");
                var customer = new Customer
                {
                    Name = "John Doe",
                    Phones = new string[] { "8000-0000", "9000-0000" },
                    Age = 39,
                    IsActive = true
                };
                col.EnsureIndex(x => x.Name, true);
                col.Insert(customer);
                customer.Name = "Joana Doe";
                col.Update(customer);
                var results = col.Find(x => x.Age > 20);
            }
        }



    }

    

}
