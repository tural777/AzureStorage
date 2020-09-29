﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorageLibrary;
using AzureStorageLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace MvcWebApp.Controllers
{
    public class TableStoragesController : Controller
    {
        private readonly INoSqlStorage<Product> _noSqlStorage;

        public TableStoragesController(INoSqlStorage<Product> noSqlStorage)
        {
            _noSqlStorage = noSqlStorage;
        }


        public IActionResult Index()
        {
            ViewBag.products = _noSqlStorage.All().ToList();
            ViewBag.isUpdate = false;

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Update(string rowKey, string partitionKey)
        {
            var product = await _noSqlStorage.Get(rowKey, partitionKey);

            ViewBag.products = _noSqlStorage.All().ToList();
            ViewBag.isUpdate = true;

            return View("Index", product);
        }


        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            product.ETag = "*"; // eyni anda, eyni datani 1-den chox adam deyishmeye chalishanda,
            // exception atmashin, son kim update edibse onun etdiyi deyishikliklerin qalmasi uchun
            // ETag-a * qoymaq lazimdir.
            // Diger hallar uchun try catch ile de nese etmek olar.

            await _noSqlStorage.Update(product);
            ViewBag.isUpdate = true;

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            product.RowKey = Guid.NewGuid().ToString();
            product.PartitionKey = "Car";

            await _noSqlStorage.Add(product);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string rowKey, string partitionKey)
        {
            await _noSqlStorage.Delete(rowKey, partitionKey);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Query(int price)
        {
            ViewBag.isUpdate = false;

            ViewBag.products = _noSqlStorage.Query(p => p.Price > price).ToList();

            return View("Index");
        }
    }
}
