using AutoMapper;

using InventoryDatabaseLayer;

using InventoryModels;
using InventoryModels.Dtos;

using libDB;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryBusinessLayer
{
    public class PlayersService : IPlayersService
    {
        private readonly PlayersRepo _dbRepo;

        public PlayersService(InventoryDbContext context, IMapper mapper)
        {
            _dbRepo = new PlayersRepo(context, mapper);
        }

        public void DeletePlayer(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Please set a valid ID");
            }

            _dbRepo.DeletePlayer(id);
        }

        public void DeletePlayers(List<int> playerIds)
        {
            try
            {
                _dbRepo.DeletePlayers(playerIds);
            }
            catch (Exception ex)
            {
                // TODO: better logging/not squelch
                Debug.WriteLine($"The transaction has failed: {ex.Message}");
                throw;
            }
        }

        public List<PlayerDto> GetPlayers()
        {
            return _dbRepo.GetPlayers();
        }

        public int UpsertPlayer(Player player)
        {
            return _dbRepo.UpsertPlayer(player);
        }

        public void UpsertPlayers(List<Player> players)
        {
            try
            {
                _dbRepo.UpsertPlayers(players);
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
