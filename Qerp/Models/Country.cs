﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Qerp.Models
{
    public partial class Country
    {
        public Country()
        {

        }

        public long Id { get; set; }
        public string Iso { get; set; }
        public string Name { get; set; }
        public string Nicename { get; set; }
        public string Iso3 { get; set; }
        public short? Numcode { get; set; }
        public int Phonecode { get; set; }

    }
}