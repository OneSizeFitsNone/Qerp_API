﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Qerp.Models
{
    public partial class Parameter
    {
        public Parameter()
        {

        }

        public long Id { get; set; }
        public long GroupId { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public string Systemcode { get; set; }
        public string Description { get; set; }

        public virtual Company Company { get; set; }
        public virtual Parametergroup Group { get; set; }
    }
}