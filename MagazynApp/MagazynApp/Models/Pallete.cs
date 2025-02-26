using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace MagazynApp.Models
{
    public class Pallete
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string  PalleteName { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
