using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryModels;
using InventoryModels.Dtos;

using libDB;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace InventoryDatabaseLayer
{
    public class GenresRepo : IGenresRepo
    {
        private readonly IMapper _mapper;
        private readonly InventoryDbContext _context;

        public GenresRepo(InventoryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task DeleteGenre(int id)
        {
            Genre? genre = await _context.Genres.FirstOrDefaultAsync(genre => genre.Id == id);
            if (genre == null)
            {
                return;
            }
            genre.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGenres(List<int> genreIds)
        {
            using (TransactionScope scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                }))
            {
                try
                {
                    foreach (int genreId in genreIds)
                    {
                        await DeleteGenre(genreId);
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    // log it:
                    Debug.WriteLine(ex.ToString());
                    throw;
                }
            }
        }

        public async Task<List<GenreDto>> GetGenres()
        {
            return await _context.Genres
                .ProjectTo<GenreDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<int> UpsertGenre(Genre genre)
        {
            if (genre.Id > 0)
            {
                return await UpdateGenre(genre);
            }

            return await CreateGenre(genre);
        }

        private async Task<int> CreateGenre(Genre genre)
        {
            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();
            Genre? newGenre = await _context.Genres
                .FirstOrDefaultAsync(currentGenre => currentGenre.Name.ToLower().Equals(genre.Name.ToLower()));

            if (newGenre == null)
            {
                throw new Exception("Could not Create the genre as expected.");
            }

            return newGenre.Id;
        }

        private async Task<int> UpdateGenre(Genre genre)
        {
            Genre? dbGenre = await _context.Genres
                .FirstOrDefaultAsync(currentGenre => currentGenre.Id == genre.Id);

            if (dbGenre == null)
            {
                throw new Exception("Genre not found");
            }

            dbGenre.Name = genre.Name;
            dbGenre.IsActive = genre.IsActive;
            dbGenre.IsDeleted = genre.IsDeleted;
            dbGenre.LastModifiedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return dbGenre.Id;
        }

        public async Task UpsertGenres(List<Genre> genres)
        {
            using (TransactionScope scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                }))
            {
                try
                {
                    foreach (Genre genre in genres)
                    {
                        bool success = await UpsertGenre(genre) > 0;
                        if (!success)
                        {
                            throw new Exception($"ERROR saving the genre {genre.Name}");
                        }
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    // lot it:
                    Debug.WriteLine(ex.ToString());
                    throw;
                }
            }
        }
    }
}
