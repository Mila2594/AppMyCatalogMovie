using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAPYREST.Models
{
    public class GenreResponse
    {
        [JsonProperty("genres")]
        public List<Genre> Genres { get; set; }
    }
}
