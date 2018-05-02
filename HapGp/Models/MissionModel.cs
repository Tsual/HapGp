using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HapGp.Models
{
    public class MissionModel
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int TeacherID { get; set; }
        public int StudentID { get; set; }
        public string Name { get; set; }
        public DateTime ActiveTime { get; set; }
        public DateTime DeadLine { get; set; }
        public bool IsFinished { get; set; }
        public string EXT { get; set; }
    }



}
