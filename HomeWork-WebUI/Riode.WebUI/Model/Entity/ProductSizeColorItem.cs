﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Model.Entity
{
    public class ProductSizeColorItem:BaseEntity
    {
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int SizeId { get; set; }

        public virtual ProductSize Size { get; set; }

        public int ColorId { get; set; }

        public virtual ProductColor Color { get; set; }
    }
}
