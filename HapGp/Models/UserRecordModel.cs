using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HapGp.Models
{
    [Table(name: "UserRecord")]
    public class UserRecordModel
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string LID { get; set; }

        public string Value { get; set; }
    }
}
