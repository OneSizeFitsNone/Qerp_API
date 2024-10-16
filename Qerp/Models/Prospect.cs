﻿#nullable disable
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
        public long ApptypeId { get; set; }
        public long CompanyId { get; set; }
        public long? ClientId { get; set; }
        public long? ContactId { get; set; }
        public long? ProspectTypeId { get; set; }
        public string Number { get; set; }
        public DateTime? Deadline { get; set; }
        public string Description { get; set; }
        public int? EstimatedBudget { get; set; }
        public long? StatusId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public virtual Client? Client { get; set; }
        public virtual Company? Company { get; set; }
        public virtual Contact? Contact { get; set; }
        public virtual Parameter? ProspectType { get; set; }
        public virtual Parameter? Status { get; set; }

    }
}