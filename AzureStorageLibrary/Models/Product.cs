﻿using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureStorageLibrary.Models
{
    public class Product : TableEntity
    {
        public string Name { get; set; }

        public string Color { get; set; }

        public int Price { get; set; }

        public int Stock { get; set; }
    }
}
