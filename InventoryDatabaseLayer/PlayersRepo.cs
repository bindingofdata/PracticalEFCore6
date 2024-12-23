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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace InventoryDatabaseLayer
{
    public class PlayersRepo : IPlayersRepo
    {
        private readonly IMapper _mapper;
        private readonly InventoryDbContext _context;

        public PlayersRepo(InventoryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void DeletePlayer(int id)
        {
            Player? player = _context.Players.FirstOrDefault(player => player.Id == id);
            if (player ==  null)
            {
                return;
            }
            player.IsDeleted = true;
            _context.SaveChanges();
        }

        public void DeletePlayers(List<int> playerIds)
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
                    foreach (int playerId in playerIds)
                    {
                        DeletePlayer(playerId);
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

        public List<PlayerDto> GetPlayers()
        {
            return _context.Players
                .ProjectTo<PlayerDto>(_mapper.ConfigurationProvider)
                .ToList();
        }

        public int UpsertPlayer(Player player)
        {
            if (player.Id > 0)
            {
                return UpdatePlayer(player);
            }

            return CreatePlayer(player);
        }

        private int CreatePlayer(Player player)
        {
            _context.Players.Add(player);
            _context.SaveChanges();
            Player? newPlayer = _context.Players.ToList()
                .FirstOrDefault(currentPlayer => currentPlayer.Name.ToLower().Equals(player.Name.ToLower()));

            if (newPlayer == null)
            {
                throw new Exception("Could not Create the player as expected.");
            }

            return newPlayer.Id;
        }

        private int UpdatePlayer(Player player)
        {
            Player? dbPlayer = _context.Players
                .FirstOrDefault(currentPlayer => currentPlayer.Id == player.Id);

            if (dbPlayer == null)
            {
                throw new Exception("Player not found");
            }

            dbPlayer.Name = player.Name;
            dbPlayer.Description = player.Description;
            if (player.Items != null)
            {
                dbPlayer.Items = player.Items;
            }
            dbPlayer.IsActive = player.IsActive;
            dbPlayer.IsDeleted = player.IsDeleted;
            dbPlayer.LastModifiedDate = DateTime.UtcNow;
            _context.SaveChanges();
            return dbPlayer.Id;
        }

        public void UpsertPlayers(List<Player> players)
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
                    foreach (Player player in players)
                    {
                        bool success = UpsertPlayer(player) > 0;
                        if (!success)
                        {
                            throw new Exception($"ERROR saving the player {player.Name}");
                        }
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
    }
}
