﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Qerp.Models
{
    public partial class Prospect
    {
        public Prospect()
        {

        }

        public long Id { get; set; }
        public long ProjectId { get; set; }
        public long ContactId { get; set; }
        public DateTime? Deadline { get; set; }
        public string Description { get; set; }
        public int? EstimatedBuget { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public virtual Contact Contact { get; set; }
        public virtual Project Project { get; set; }

    }
}