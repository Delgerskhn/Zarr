using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zar.Models
{
    public class Post
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string MarkCode { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Img { get; set; }
        public Company company { get; set; }
        public string industry { get; set; }
    }
}
