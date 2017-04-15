using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DemandDriven.Data;
using DemandDriven.Pocos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemandDriven.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        public IActionResult Complete()
        {
            ViewData["Message"] = "Thank you for your upload!";
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            Converter conversion = null;
            try
            {
                conversion = Generate(file);
                SaveToModel(conversion);
            }
            catch (Exception e)
            {
                ViewData["Message"] = e.Message;
                return View("Index");
            }

            return RedirectToAction("Complete");
        }

        private static void SaveToModel(Converter conversion)
        {
            Context context = new Context();
            var names = conversion.Entries.Select(x => x.ParentName);
            names = names.Union(conversion.Entries.Select(x => x.ChildName));
            names = names.ToList();
            context.UpsertNodeNames(names.ToList());
            context.InsertEdges(conversion.Entries);
        }

        private Converter Generate(IFormFile file) {
            Converter converter = null;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var fileContent = reader.ReadToEnd();
                string extension = Path.GetExtension(file.Name).ToUpper();
                switch(extension) {
                    case "CSV":
                        converter = GraphFactory.Generate(InputType.CSV, fileContent);
                        break;
                    // CSV default
                    default:
                        converter = GraphFactory.Generate(InputType.CSV, fileContent);
                        break;
                }
            }
            return converter;
        }
    }
}
