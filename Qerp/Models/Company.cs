﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Bogus.DataSets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Qerp.Models
{
    public partial class Company
    {
        public Company()
        {

        }

        public long Id { get; set; }
        public long ApptypeId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public long? CityId { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string InvoiceAddress { get; set; }
        public long? InvoiceCityId { get; set; }
        public string InvoiceEmail { get; set; }
        public string Vat { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Iban { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public virtual City City { get; set; }
        public virtual City InvoiceCity { get; set; }

    }
}