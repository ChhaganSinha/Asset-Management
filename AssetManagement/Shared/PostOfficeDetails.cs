using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AssetManagement.Dto
{
    public class PincodeApiResponse
    {
        [JsonPropertyName("Message")]
        public string Message { get; set; }

        [JsonPropertyName("Status")]
        public string Status { get; set; }

        [JsonPropertyName("PostOffice")]
        public List<PostOffice> PostOffices { get; set; }
    }
}

public class PostOffice
{
    [JsonPropertyName("Name")]
    public string Name { get; set; }

    [JsonPropertyName("Description")]
    public string Description { get; set; }

    [JsonPropertyName("BranchType")]
    public string BranchType { get; set; }

    [JsonPropertyName("DeliveryStatus")]
    public string DeliveryStatus { get; set; }

    [JsonPropertyName("Circle")]
    public string Circle { get; set; }

    [JsonPropertyName("District")]
    public string District { get; set; }

    [JsonPropertyName("Division")]
    public string Division { get; set; }

    [JsonPropertyName("Region")]
    public string Region { get; set; }

    [JsonPropertyName("Block")]
    public string Block { get; set; }

    [JsonPropertyName("State")]
    public string State { get; set; }

    [JsonPropertyName("Country")]
    public string Country { get; set; }

    [JsonPropertyName("Pincode")]
    public string Pincode { get; set; }
}
