﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AzureStorageLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using MvcWebApp.Models;

namespace MvcWebApp.Controllers
{
    public class BlobsController : Controller
    {
        private readonly IBlobStorage _blobStorage;

        public BlobsController(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        public async Task<IActionResult> Index()
        {
            var names = _blobStorage.GetNames(EContainerName.pictures);
            string blobUrl = $"{_blobStorage.BlobUrl}/{EContainerName.pictures.ToString()}";
            ViewBag.blobs = names.Select(x => new FileBlob { Name = x, Url = $"{blobUrl}/{x}" }).ToList();

            // appendBlob Microsoft Azure Storage Explorer ile ishlemir, 
            // ancaq online qoshulanda loglama ishlarmizi ede bilerik.
            // kod hazirdi, sadece kommentleri silmek lazimdi.
            //ViewBag.logs = await _blobStorage.GetLogAsync("controller.txt");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile picture)
        {
            //await _blobStorage.SetLogAsync("Upload methoduna giriş yapıldı", "controller.txt");

            var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(picture.FileName);

            await _blobStorage.UploadAsync(picture.OpenReadStream(), newFileName, EContainerName.pictures);

            //await _blobStorage.SetLogAsync("Upload methodundan çıkış yapıldı", "controller.txt");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Download(string fileName)
        {
            var stream = await _blobStorage.DownloadAsync(fileName, EContainerName.pictures);

            return File(stream, "application/octet-stream", fileName);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string fileName)
        {
            await _blobStorage.DeleteAsync(fileName, EContainerName.pictures);
            return RedirectToAction("Index");
        }
    }
}