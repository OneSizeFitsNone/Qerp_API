﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Qerp.Models
{
    public partial class Invoiceline
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long ClientId { get; set; }
        public long ProjectId { get; set; }
        public long? TaskId { get; set; }
        public long? InvoiceId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public virtual Client Client { get; set; }
        public virtual Company Company { get; set; }
        public virtual Invoice Invoice { get; set; }
        public virtual Project Project { get; set; }
        public virtual Models.Task Task { get; set; }
    }
}