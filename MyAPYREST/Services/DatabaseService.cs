using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAPYREST.Models;

namespace MyAPYREST.Services
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "favorites.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<FavoriteMovie>().Wait();
        }

        public async Task<FavoriteMovie> GetFavoriteByIdAsync(int movieId)
        {
            var favorite = await _database.Table<FavoriteMovie>().Where(m => m.Id == movieId).FirstOrDefaultAsync();
            return favorite ?? null;
        }


        public Task<List<FavoriteMovie>> GetFavoritesAsync()
        {
            return _database.Table<FavoriteMovie>().ToListAsync();
        }

        public Task<int> AddFavoriteAsync(FavoriteMovie movie)
        {
            movie.PosterPath = string.IsNullOrEmpty(movie.PosterPath) ? "placeholder.png" : movie.PosterPath;
            return _database.InsertAsync(movie);
        }

        public async Task<int> UpdateFavoriteAsync(FavoriteMovie movie)
        {
            if (movie == null) return 0;

            var existingMovie = await GetFavoriteByIdAsync(movie.Id);
            if (existingMovie == null) return 0;

            existingMovie.Notes = movie.Notes; // 🔥 Asegurar que la nota se actualiza
            return await _database.UpdateAsync(existingMovie);
        }


        public Task<int> RemoveFavoriteAsync(FavoriteMovie movie)
        {
            return _database.DeleteAsync(movie);
        }


    }
}
