using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;
namespace MagazynApp.Models
{
    public class PalletProduct
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int PalletId { get; set; }
        public int ProductId { get; set; }

    }
}
