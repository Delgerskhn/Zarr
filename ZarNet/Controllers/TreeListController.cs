using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ZarNet.Controllers
{
    public class TreeListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
