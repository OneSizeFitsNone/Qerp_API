﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Qerp.Models
{
    public partial class Vat
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public int? Percentage { get; set; }
        public int? Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public virtual Company Company { get; set; }
    }
}