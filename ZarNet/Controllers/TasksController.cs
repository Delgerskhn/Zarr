using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ZarNet.Data;
using ZarNet.Models;

namespace ZarNet.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TasksController : Controller
    {
        private ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var task = _context.Task.Select(i => new {
                i.ID,
                i.ParentID,
                i.Name,
                i.Picture
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ID" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(task, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new ZarNet.Models.Task();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Task.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.ID });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Task.FirstOrDefaultAsync(item => item.ID == key);
            if(model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async System.Threading.Tasks.Task Delete(int key) {
            var model = await _context.Task.FirstOrDefaultAsync(item => item.ID == key);

            _context.Task.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(ZarNet.Models.Task model, IDictionary values) {
            string ID = nameof(ZarNet.Models.Task.ID);
            string PARENT_ID = nameof(ZarNet.Models.Task.ParentID);
            string NAME = nameof(ZarNet.Models.Task.Name);
            string PICTURE = nameof(ZarNet.Models.Task.Picture);

            if(values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(PARENT_ID)) {
                model.ParentID = Convert.ToInt32(values[PARENT_ID]);
            }

            if(values.Contains(NAME)) {
                model.Name = Convert.ToString(values[NAME]);
            }

            if(values.Contains(PICTURE)) {
                model.Picture = Convert.ToString(values[PICTURE]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}