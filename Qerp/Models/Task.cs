﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Bogus.DataSets;
using System;
using System.Collections.Generic;

namespace Qerp.Models
{
    public partial class Task
    {
        public Task()
        {

        }

        public long Id { get; set; }
        public long ApptypeId { get; set; }
        public long CompanyId { get; set; }
        public long? ClientId { get; set; }
        public long? ProjectId { get; set; }
        public long? ProspectId { get; set; }
        public long? ContactId { get; set; }
        public long? MilestoneId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? Deadline { get; set; }
        public float? Timer { get; set; }
        public float? MaxTime { get; set; }
        public ulong? ToInvoice { get; set; }
        public ulong? Completed { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public virtual Client Client { get; set; }
        public virtual Company Company { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual Milestone Milestone { get; set; }
        public virtual Project Project { get; set; }
        public virtual Prospect Prospect { get; set; }

    }
}