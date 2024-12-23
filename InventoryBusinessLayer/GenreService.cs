using AutoMapper;

using InventoryModels;
using InventoryModels.Dtos;

using libDB;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDatabaseLayer
{
    public class GenreService : IGenresService
    {
        private readonly IGenresRepo _dbRepo;
        private readonly IMapper _mapper;

        public GenreService(IGenresRepo dbRepo, IMapper mapper)
        {
            _dbRepo = dbRepo;
            _mapper = mapper;
        }

        public void DeleteGenre(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Please set a valid ID");
            }

            _dbRepo.DeleteGenre(id);
        }

        public void DeleteGenres(List<int> genreIds)
        {
            try
            {
                _dbRepo.DeleteGenres(genreIds);
            }
            catch (Exception ex)
            {
                // TODO: better logging/not squelching
                Debug.WriteLine($"The transaction has failed: {ex.Message}");
                throw;
            }
        }

        public List<GenreDto> GetGenres()
        {
            return _dbRepo.GetGenres();
        }

        public int UpsertGenre(Genre genre)
        {
            return _dbRepo.UpsertGenre(genre);
        }

        public void UpsertGenres(List<Genre> genres)
        {
            try
            {
                _dbRepo.UpsertGenres(genres);
            }
            catch (Exception ex)
            {
                // TODO: better logging/not squelching
                Debug.WriteLine($"The transaction has failed: {ex.Message}");
                throw;
            }
        }
    }
}
