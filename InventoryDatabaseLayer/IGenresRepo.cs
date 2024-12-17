using InventoryModels.Dtos;
using InventoryModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDatabaseLayer
{
    public interface IGenresRepo
    {
        List<GenreDto> GetGenres();
        int UpsertGenre(Genre genre);
        void UpsertGenres(List<Genre> genres);
        void DeleteGenre(int id);
        void DeleteGenres(List<int> genreIds);
    }
}
