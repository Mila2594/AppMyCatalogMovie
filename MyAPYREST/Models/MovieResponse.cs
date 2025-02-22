//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace MyAPYREST.Models
{
    public class MovieResponse
    {
        [JsonPropertyName("results")]
        public List<Movie> Results { get; set; }
    }
}
