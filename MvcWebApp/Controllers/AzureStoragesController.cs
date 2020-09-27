using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorageLibrary;
using AzureStorageLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace MvcWebApp.Controllers
{
    public class AzureStoragesController : Controller
    {
        private readonly INoSqlStorage<Product> _noSqlStorage;

        public AzureStoragesController(INoSqlStorage<Product> noSqlStorage)
        {
            _noSqlStorage = noSqlStorage;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
