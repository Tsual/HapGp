using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HapGp.Models
{
    public class SigninModel
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; } 
        public int StudentID { get; set; }
        public int ProjectID { get; set; }
        public DateTime Time { get; set; }




    }
}
