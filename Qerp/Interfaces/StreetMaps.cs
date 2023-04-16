namespace Qerp.Interfaces
{
    public class StreetMaps
    {
        public long Place_Id { get; set; }
        public string? Licence { get; set; }
        public string? Osm_Type { get; set; }
        public long Osm_Id { get; set; }
        public List<string>? Boundingbox { get; set; }
        public string? Lat { get; set; }
        public string? Long { get; set; }
        public string? Display_Name { get; set; }
        public string? Class { get; set; }
        public string? Type { get; set; }
        public decimal Importance { get; set; }
        public StreetMapsAddress? Address { get; set; }

    }

    public class StreetMapsAddress
    {
        public string? House_Number { get; set; }
        public string? Road { get; set; }
        public string? Hamlet { get; set; }
        public string? Neighbourhood { get; set; }
        public string? Suburb { get; set; }
        public string? Village { get; set; }
        public string? Town { get; set; }
        public string? City { get; set; }
        public string? County { get; set; }
        public string? State_Disctrict { get; set; }
        public string? State { get; set; }
        public string? ISO31662lvl4 { get; set;}
        public string? ISO31662lvl6 { get; set;}
        public string? Region { get; set; }
        public string? Postcode { get; set; }
        public string? Country { get; set; }
        public string? Country_Code { get; set; }

    }
}
