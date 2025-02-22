//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace MyAPYREST.Models
{
    public class Movie : INotifyPropertyChanged
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("vote_average")]
        public double Rating { get; set; }

        [JsonPropertyName("poster_path")]
        public string PosterPath { get; set; }

        // Fecha de lanzamiento
        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; }

        // Géneros
        [JsonPropertyName("genre_ids")]
        public List<int> GenreIds { get; set; }

        //Overview
        [JsonPropertyName("overview")]
        public string Overview { get; set; }

        //Language
        [JsonPropertyName("original_language")]
        public string Language { get; set; }

        private bool _isFavorite;
        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                _isFavorite = value;
                OnPropertyChanged(nameof(IsFavorite)); // 🔥 Notificar cambio de estado para actualizar UI
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }




    }
}
