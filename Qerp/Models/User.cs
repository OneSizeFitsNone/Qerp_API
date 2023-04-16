﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Qerp.Models
{
    public partial class User
    {
        public User()
        {
            Saveditems = new HashSet<Saveditem>();
        }

        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long ContactId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string LastToken { get; set; }

        public virtual Company Company { get; set; }
        public virtual Contact Contact { get; set; }
        public virtual ICollection<Saveditem> Saveditems { get; set; }
    }
}