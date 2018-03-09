using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace HapGp.Models
{
    public class ProjectModel
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Key { get; set; }
        public string ProjectName { get; set; }     
        public string Subtitle { get; set; }
        public int TeacherID { get; set; }
        public DayOfWeek DayofWeek { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double SiWest { get; set; }
        public double SiNorth { get; set; }
        public double SiSouth { get; set; }
        public double SiEast { get; set; }

    }




}
