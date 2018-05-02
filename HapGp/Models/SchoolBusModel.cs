using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HapGp.Models
{
    public class SchoolBusModel
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Discription { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string EXT { get; set; }
    }



}
