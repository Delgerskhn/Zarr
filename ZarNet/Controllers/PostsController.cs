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
    public class PostsController : Controller
    {
        private ApplicationDbContext _context;

        public PostsController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPostView(DataSourceLoadOptions loadOptions)
        {
            var query = _context.Category.Join(_context.Post, cat => cat.CategoryId, post => post.CategoryId,
                (c, d) => new PostView
                {
                    CategoryId = c.CategoryId,
                    ParentCategoryId = c.ParentId,
                    CategoryName = c.Name,
                    CompanyId = d.CompanyId,
                    CompanyName = d.Company.Name,
                    Description = d.Description,
                    Img = d.Img,
                    MarkCode = d.MarkCode,
                    Price = d.Price,
                    Title = d.Name
                }
                );

            var postView = 
                from c in _context.Category
                join p in _context.Post on c.CategoryId equals p.CategoryId into Details
                from d in Details.DefaultIfEmpty()
                           select new PostView
                           {
                               CategoryId = c.CategoryId,
                               ParentCategoryId = c.ParentId,
                               CategoryName = c.Name,
                               CompanyId = d.CompanyId,
                               CompanyName = d.Company.Name,
                               Description = d.Description,
                               Img = d.Img,
                               MarkCode = d.MarkCode,
                               Price = d.Price,
                               Title = d.Name
                           };

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "PostId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(postView, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var post = _context.Post.Select(i => new {
                i.PostId,
                i.Name,
                i.MarkCode,
                i.Description,
                i.Price,
                i.Img,
                i.CompanyId,
                i.CategoryId
            });

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "PostId" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(post, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Post();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Post.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.PostId });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Post.FirstOrDefaultAsync(item => item.PostId == key);
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
        public async Task Delete(int key) {
            var model = await _context.Post.FirstOrDefaultAsync(item => item.PostId == key);

            _context.Post.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> CompanyLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Company
                         orderby i.Name
                         select new {
                             Value = i.CompanyId,
                             Text = i.Name
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> CategoryLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Category
                         orderby i.Name
                         select new {
                             Value = i.CategoryId,
                             Text = i.Name
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(Post model, IDictionary values) {
            string POST_ID = nameof(ZarNet.Models.Post.PostId);
            string NAME = nameof(ZarNet.Models.Post.Name);
            string MARK_CODE = nameof(ZarNet.Models.Post.MarkCode);
            string DESCRIPTION = nameof(ZarNet.Models.Post.Description);
            string PRICE = nameof(ZarNet.Models.Post.Price);
            string IMG = nameof(ZarNet.Models.Post.Img);
            string COMPANY_ID = nameof(ZarNet.Models.Post.CompanyId);
            string CATEGORY_ID = nameof(ZarNet.Models.Post.CategoryId);

            if(values.Contains(POST_ID)) {
                model.PostId = Convert.ToInt32(values[POST_ID]);
            }

            if(values.Contains(NAME)) {
                model.Name = Convert.ToString(values[NAME]);
            }

            if(values.Contains(MARK_CODE)) {
                model.MarkCode = Convert.ToString(values[MARK_CODE]);
            }

            if(values.Contains(DESCRIPTION)) {
                model.Description = Convert.ToString(values[DESCRIPTION]);
            }

            if(values.Contains(PRICE)) {
                model.Price = Convert.ToString(values[PRICE]);
            }

            if(values.Contains(IMG)) {
                model.Img = Convert.ToString(values[IMG]);
            }

            if(values.Contains(COMPANY_ID)) {
                model.CompanyId = Convert.ToInt32(values[COMPANY_ID]);
            }

            if(values.Contains(CATEGORY_ID)) {
                model.CategoryId = Convert.ToInt32(values[CATEGORY_ID]);
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