using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HapGp.Models
{
    public class LeaveModel
    {
        [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int StudentID { get; set; }
        public int TeacherID { get; set; }
        public int ClassID { get; set; }
        public DateTime LeaveBeginTime { get; set; }
        public DateTime LeaveEndTime { get; set; }
        public int RequestTargetTeacherID { get; set; }
        public int ApprovedTeacherID { get; set; }
        public bool IsApproved { get; set; }

        public string EXT { get; set; }
    }



}
