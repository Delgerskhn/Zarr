using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Zar.Models;
using ZarNet.Models;
using ZarNet.Models.SampleData;

namespace ZarNet.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public ICollection<EmployeeTask> Tasks ;
        public IEnumerable<TaskEmployee> Employees => SampleData.TaskEmployees;
        protected  IEnumerable<EmployeeTask> Source => SampleData.EmployeeTasks;
        protected  int GetKey(EmployeeTask item)
        {
            return item.Task_ID;
        }

        protected  void SetKey(EmployeeTask item, int key)
        {
            item.Task_ID = key;
        }

        public DbSet<Post> Post { get; set; }

    }
}
