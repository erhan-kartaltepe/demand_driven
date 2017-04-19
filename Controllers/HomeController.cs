using System;
using System.IO;
using System.Linq;
using System.Text;
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

        public IActionResult Complete(string guid)
        {
            ViewData["Message"] = "Thank you for your upload! Your GUID is " + guid + ". Please save this for your records.";
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Download(string guid)
        {
            var file = GetFile(guid, "CSV");

            if (file == null) {
                ViewData["Message"] = "No such GUID found.";                                                                                                               
                return View("Index");
            }

            return File(file, "text/plain", guid + ".csv");
        }


        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            Converter conversion = null;
            string guid = string.Empty;
            try
            {
                conversion = Generate(file);
                guid = SaveToModel(conversion);
            }
            catch (Exception e)
            {
                ViewData["Message"] = e.Message;
                return View("Index");
            }
            return RedirectToAction("Complete", new {guid = guid});
        }

        private static string SaveToModel(Converter conversion)
        {
            Context context = new Context();
            var names = conversion.Entries.Select(x => x.ParentName);
            names = names.Union(conversion.Entries.Select(x => x.ChildName));
            names = names.ToList();
            context.UpsertNodeNames(names.ToList());
            return context.AddGraph(conversion.Entries);
        }

        private static byte[] GetFile(string guid, string type)
        {
            if (string.IsNullOrWhiteSpace(guid)) {
                throw new ArgumentException("Guid cannot be empty");
            }
            
            Context context = new Context();
            var edges = context.GetGraph(guid);
            if (edges == null) {
                return null;
            }

            Converter converter = null;
            switch(type) {
                case "CSV":
                    converter = GraphFactory.Generate(InputType.CSV, edges);
                    break;
                // CSV default
                default:
                    converter = GraphFactory.Generate(InputType.CSV, edges);
                    break;
            }
            return Encoding.ASCII.GetBytes(converter.File);
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
