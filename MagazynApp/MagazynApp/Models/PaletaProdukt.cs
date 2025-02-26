using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;
namespace MagazynApp.Models
{
    public class PaletaProdukt
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int PaletaId { get; set; }
        public int ProduktId { get; set; }

    }
}
