﻿using System;
using System.Collections.Generic;

namespace SqlzooAWdbC.Models.DB
{
    public partial class VProductAndDescription
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ProductModel { get; set; }
        public string Culture { get; set; }
        public string Description { get; set; }
    }
}
