using System;
using System.Collections.Generic;

namespace AssetManagement.Dto
{
    public class EntityVendorDto
    {
        public int id { get; set; }
        public AddressDto corporate_address_id { get; set; }
        public AddressDto correspondence_address_id { get; set; }
        public AddressDto registered_address_id { get; set; }
        public string firm_name { get; set; }
        public string legal_status { get; set; }
        public OtherRegistrationDto other_registration { get; set; }
        public string pan_no { get; set; }
        public string esi_no { get; set; }
        public string pf_no { get; set; }
        public string gst_no { get; set; }
        public string contact_person { get; set; }
        public string contact_no { get; set; }
        public string email { get; set; }
        public List<int> users { get; set; }
    }

    public class AddressDto
    {
        public int id { get; set; }
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }
        public string address_type { get; set; }
    }

    public class OtherRegistrationDto
    {
        public bool msme { get; set; }
        public bool other { get; set; }
        public string msme_no { get; set; }
        public bool startup { get; set; }
        public string other_no { get; set; }
        public string startup_no { get; set; }
    }
}
