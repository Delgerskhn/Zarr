using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZarNet.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Priority { Low, Normal, High, Urgent }
    public class EmployeeTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Task_ID { set; get; }
        public int Task_Parent_ID { set; get; }

        [Required]
        [Display(Name = "Assigned")]
        public int Task_Assigned_Employee_ID { set; get; }
        public Task Task_Assigned_Employee { set; get; }
        public int Task_Owner_ID { set; get; }
        [Required]
        [Display(Name = "Task Subject")]
        public string Task_Subject { set; get; }

        [Display(Name = "Start Date")]
        public DateTime Task_Start_Date { set; get; }

        [Display(Name = "Due Date")]
        public DateTime Task_Due_Date { set; get; }

        [Display(Name = "Status")]
        public string Task_Status { set; get; }
        public Priority Task_Priority { set; get; }
        public int Task_Completion { set; get; }
        public bool Has_Items { get; set; }
    }
}
