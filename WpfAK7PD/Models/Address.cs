using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAK7PD.Models
{
    public class Address
    {
        [BsonElement("street")]
        public string Street { get; set; }

        [BsonElement("city")]
        public string City { get; set; }

        [BsonElement("zip_code")]
        public string ZipCode { get; set; }
    }
}
