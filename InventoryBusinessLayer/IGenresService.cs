using InventoryModels;
using InventoryModels.Dtos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDatabaseLayer
{
    public interface IGenresService
    {
        Task<List<GenreDto>> GetGenres();
        Task<int> UpsertGenre(Genre genre);
        Task UpsertGenres(List<Genre> genres);
        Task DeleteGenre(int id);
        Task DeleteGenres(List<int> genreIds);
    }
}
