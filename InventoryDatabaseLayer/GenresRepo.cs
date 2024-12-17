using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryModels;
using InventoryModels.Dtos;

using libDB;

using Microsoft.EntityFrameworkCore.Storage;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void DeleteGenre(int id)
        {
            Genre? genre = _context.Genres.FirstOrDefault(genre => genre.Id == id);
            if (genre == null)
            {
                return;
            }
            genre.IsDeleted = true;
            _context.SaveChanges();
        }

        public void DeleteGenres(List<int> genreIds)
        {
            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (int genreId in genreIds)
                    {
                        DeleteGenre(genreId);
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // log it:
                    Debug.WriteLine(ex.ToString());
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public List<GenreDto> GetGenres()
        {
            return _context.Genres
                .ProjectTo<GenreDto>(_mapper.ConfigurationProvider)
                .ToList();
        }

        public int UpsertGenre(Genre genre)
        {
            if (genre.Id > 0)
            {
                return UpdateGenre(genre);
            }

            return CreateGenre(genre);
        }

        private int CreateGenre(Genre genre)
        {
            _context.Genres.Add(genre);
            _context.SaveChanges();
            Genre? newGenre = _context.Genres.ToList()
                .FirstOrDefault(currentGenre => currentGenre.Name.ToLower().Equals(genre.Name.ToLower()));

            if (newGenre == null)
            {
                throw new Exception("Could not Create the genre as expected.");
            }

            return newGenre.Id;
        }

        private int UpdateGenre(Genre genre)
        {
            Genre? dbGenre = _context.Genres
                .FirstOrDefault(currentGenre => currentGenre.Id == genre.Id);

            if (dbGenre == null)
            {
                throw new Exception("Genre not found");
            }

            dbGenre.Name = genre.Name;
            dbGenre.IsActive = genre.IsActive;
            dbGenre.IsDeleted = genre.IsDeleted;
            dbGenre.LastModifiedDate = DateTime.UtcNow;
            _context.SaveChanges();
            return dbGenre.Id;
        }

        public void UpsertGenres(List<Genre> genres)
        {
            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (Genre genre in genres)
                    {
                        bool success = UpsertGenre(genre) > 0;
                        if (!success)
                        {
                            throw new Exception($"ERROR saving the genre {genre.Name}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    // lot it:
                    Debug.WriteLine(ex.ToString());
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
