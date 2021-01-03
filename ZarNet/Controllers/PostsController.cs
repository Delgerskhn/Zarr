using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
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
    public class PostsController : Controller
    {
        private ApplicationDbContext _context;
        private readonly IHtmlLocalizer<PostsController> _localizer;
        private readonly UserManager<IdentityUser> _userManager;

        public PostsController(ApplicationDbContext context, IHtmlLocalizer<PostsController> localizer, UserManager<IdentityUser> userManager) {
            _context = context;
            _localizer = localizer;
            _userManager = userManager;
        }

        // GET: Posts1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .Include(p => p.Category)
                .Include(p => p.Company)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> GetPostView(DataSourceLoadOptions loadOptions)
        {
            var query = from c in _context.Category
                        from p in _context.Post.Where(p => c.CategoryId == p.CategoryId).DefaultIfEmpty()
                        select new PostView
                        {
                            CategoryId = c.CategoryId ,
                            ParentCategoryId = c.ParentId,
                            CategoryName = c.Name,
                            CompanyId = p==null? 0 : p.CompanyId,
                            CompanyName = p==null? "": p.Company.Name,
                            Description = p==null ? "" : p.Description,
                            Img = p == null ? "" : p.Img,
                            MarkCode = p == null? "" : p.MarkCode,
                            Price = p == null? "" : p.Price,
                            Title = p == null? "" : p.Name
                        };

            // If you work with a large amount of data, consider specifying the PaginateViaPrimaryKey and PrimaryKey properties.
            // In this case, keys and data are loaded in separate queries. This can make the SQL execution plan more efficient.
            // Refer to the topic https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "PostId" };
            // loadOptions.PaginateViaPrimaryKey = true;
            var data = await DataSourceLoader.LoadAsync(query, loadOptions);
            return Json(data);
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

        // GET: Posts1/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId");
            ViewData["CompanyId"] = new SelectList(_context.Company, "CompanyId", "CompanyId");
            return View();
        }

        // POST: Posts1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Name,MarkCode,Description,Price,Img,CompanyId,CategoryId")] Post post)
        {
            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", post.CategoryId);
            ViewData["CompanyId"] = new SelectList(_context.Company, "CompanyId", "CompanyId", post.CompanyId);
            return View(post);
        }

        // GET: Posts1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", post.CategoryId);
            ViewData["CompanyId"] = new SelectList(_context.Company, "CompanyId", "CompanyId", post.CompanyId);
            return View(post);
        }

        // POST: Posts1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Name,MarkCode,Description,Price,Img,CompanyId,CategoryId")] Post post)
        {
            if (id != post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", post.CategoryId);
            ViewData["CompanyId"] = new SelectList(_context.Company, "CompanyId", "CompanyId", post.CompanyId);
            return View(post);
        }

        // GET: Posts1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .Include(p => p.Category)
                .Include(p => p.Company)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post.FindAsync(id);
            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.PostId == id);
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