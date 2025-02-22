using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAPYREST.Models
{
    public class FavoriteMovie
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int MovieId { get; set; }
        public string Title { get; set; }
        public string PosterPath { get; set; }
        public string ReleaseYear { get; set; }
        public string Rating { get; set; }
        public string Notes { get; set; } // Campo extra para agregar notas
    }
}
