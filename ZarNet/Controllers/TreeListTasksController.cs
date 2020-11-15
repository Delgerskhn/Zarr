using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using ZarNet.Data;
using ZarNet.Models;

namespace ZarNet.Controllers
{

    [Route("api/[controller]/[action]")]
    public class TreeListTasksController : Controller
    {
        private readonly ApplicationDbContext db;

        public TreeListTasksController(IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache, ApplicationDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public object Tasks(DataSourceLoadOptions loadOptions)
        {
            var tasks = from d in db.Tasks
                        select new EmployeeTask
                        {
                            Task_ID = d.Task_ID,
                            Task_Parent_ID = d.Task_Parent_ID,
                            Task_Owner_ID = d.Task_Owner_ID,
                            Task_Assigned_Employee_ID = d.Task_Assigned_Employee_ID,
                            Task_Completion = d.Task_Completion,
                            Task_Priority = d.Task_Priority,
                            Task_Status = d.Task_Status,
                            Task_Subject = d.Task_Subject,
                            Task_Start_Date = d.Task_Start_Date,
                            Task_Due_Date = d.Task_Due_Date,
                            Has_Items = db.Tasks.Count(task => task.Task_Parent_ID == d.Task_ID) > 0
                        };

            return DataSourceLoader.Load(tasks, loadOptions);
        }

        [HttpGet]
        public object TasksWithEmployees(DataSourceLoadOptions loadOptions)
        {
            var tasks = from d in db.Tasks
                        select new EmployeeTask
                        {
                            Task_ID = d.Task_ID,
                            Task_Parent_ID = d.Task_Parent_ID,
                            Task_Owner_ID = d.Task_Owner_ID,
                            Task_Assigned_Employee_ID = d.Task_Assigned_Employee_ID,
                            Task_Assigned_Employee = db.Employees.Where(employee => employee.ID == d.Task_Assigned_Employee_ID).FirstOrDefault(),
                            Task_Completion = d.Task_Completion,
                            Task_Priority = d.Task_Priority,
                            Task_Status = d.Task_Status,
                            Task_Subject = d.Task_Subject,
                            Task_Start_Date = d.Task_Start_Date,
                            Task_Due_Date = d.Task_Due_Date
                        };

            return DataSourceLoader.Load(tasks, loadOptions);
        }

        [HttpGet]
        public object TaskEmployees(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(db.Employees, loadOptions);
        }

        [HttpPost]
        public IActionResult InsertTask(string values)
        {
            var newItem = new EmployeeTask();
            JsonConvert.PopulateObject(values, newItem);

            if (!TryValidateModel(newItem))
                return BadRequest();

            db.Tasks.Add(newItem);
            db.SaveChanges();

            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateTask(int key, string values)
        {
            var item = db.Tasks.First(e => e.Task_ID == key);

            JsonConvert.PopulateObject(values, item);

            if (!TryValidateModel(item))
                return BadRequest(ModelState);

            db.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public void DeleteTask(int key)
        {
            var item = db.Tasks.First(e => e.Task_ID == key);
            db.Tasks.Remove(item);
            db.SaveChanges();
        }
    }
}
