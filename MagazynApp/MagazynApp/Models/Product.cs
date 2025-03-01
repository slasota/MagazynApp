using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MagazynApp.Models
{
    public class Product
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public string BarCode { get; set; }

    }
}
