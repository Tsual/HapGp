using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HapGp.Models
{
    public class ProjectSelectModel
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Key { get; set; }
        public int StudentID { get; set; }
        public int ProjectID { get; set; }

    }


}
