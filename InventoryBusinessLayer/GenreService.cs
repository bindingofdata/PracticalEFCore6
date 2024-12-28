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

        public async Task DeleteGenre(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Please set a valid ID");
            }

            await _dbRepo.DeleteGenre(id);
        }

        public async Task DeleteGenres(List<int> genreIds)
        {
            try
            {
                await _dbRepo.DeleteGenres(genreIds);
            }
            catch (Exception ex)
            {
                // TODO: better logging/not squelching
                Debug.WriteLine($"The transaction has failed: {ex.Message}");
                throw;
            }
        }

        public async Task<List<GenreDto>> GetGenres()
        {
            return await _dbRepo.GetGenres();
        }

        public async Task<int> UpsertGenre(Genre genre)
        {
            return await _dbRepo.UpsertGenre(genre);
        }

        public async Task UpsertGenres(List<Genre> genres)
        {
            try
            {
                await _dbRepo.UpsertGenres(genres);
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
