﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Qerp.Models
{
    public partial class Clientcontact
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public long ContactId { get; set; }
        public long ContactroleId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public virtual Client Client { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual Contactrole Contactrole { get; set; }
    }
}