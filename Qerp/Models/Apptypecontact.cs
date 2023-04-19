
#nullable disable
using System;
using System.Collections.Generic;

namespace Qerp.Models
{
    public partial class Apptypecontact
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long ApptypeId { get; set; }
        public long LinkedId { get; set; }
        public long ContactId { get; set; }
        public long? ClientId { get; set; }
        public long ContactroleId { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public virtual Client Client { get; set; }
        public virtual Company Company { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual Contactrole Contactrole { get; set; }
        public virtual Project Project { get; set; }
        public virtual Prospect Prospect { get; set; }
        public virtual Models.Task Task { get; set; }
    }
}