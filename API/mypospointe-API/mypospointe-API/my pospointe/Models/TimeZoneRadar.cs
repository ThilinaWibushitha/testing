namespace my_pospointe.Models
{
    public class TimeZoneRadar
    {
        public Meta Meta { get; set; }
        public List<Address> Addresses { get; set; }
    }

    public class Meta
    {
        public int Code { get; set; }
    }

    public class Address
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Geometry Geometry { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string CountryFlag { get; set; }
        public string County { get; set; }
        public string Confidence { get; set; }
        public string Borough { get; set; }
        public string City { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string PostalCode { get; set; }
        public string StateCode { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public string Layer { get; set; }
        public string FormattedAddress { get; set; }
        public string AddressLabel { get; set; }
        public TimeZoneInfoModel TimeZone { get; set; }
    }

    public class Geometry
    {
        public string Type { get; set; }
        public List<double> Coordinates { get; set; }
    }

    public class TimeZoneInfoModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string CurrentTime { get; set; }
        public int UtcOffset { get; set; }
        public int DstOffset { get; set; }
    }

}
