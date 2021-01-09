using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ZarNet.Models;
using ZarNet.ViewModels;

namespace ZarNet.ViewModels
{
    public class PostCreate : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }
        [Required(ErrorMessage = "Please enter last name")]
        public string Title { get; set; }
        public string MarkCode { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        [Required(ErrorMessage = "Please choose profile image")]
        [Display(Name = "Profile Picture")]
        public IFormFile Img { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        [DefaultValue("Waiting")]
        public string Status { get; set; }
    }
}
