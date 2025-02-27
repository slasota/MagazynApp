using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MagazynApp.Models
{
    public class Pallet
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string  PalletName { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.SpecifyKind(DateTime.UtcNow,DateTimeKind.Utc);
    }
}
