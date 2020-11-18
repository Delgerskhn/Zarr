using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZarNet.Models;

namespace ZarNet.Models
{
    public class PostView
    {
        public int CategoryId { get; set; }
        public int ParentCategoryId { get; set; }
        public string CategoryName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Title { get; set; }
        public string MarkCode { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Img { get; set; }
    }
}
