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

        public async Task DeletePlayer(int id)
        {
            Player? player = await _context.Players.FirstOrDefaultAsync(player => player.Id == id);
            if (player ==  null)
            {
                return;
            }
            player.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePlayers(List<int> playerIds)
        {
            using (TransactionScope scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (int playerId in playerIds)
                    {
                        await DeletePlayer(playerId);
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

        public async Task<List<PlayerDto>> GetPlayers()
        {
            return await _context.Players
                .ProjectTo<PlayerDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<int> UpsertPlayer(Player player)
        {
            if (player.Id > 0)
            {
                return await UpdatePlayer(player);
            }

            return await CreatePlayer(player);
        }

        private async Task<int> CreatePlayer(Player player)
        {
            player.CreatedDate = DateTime.UtcNow;
            player.CreatedByUserId = Environment.UserName;
            await _context.Players.AddAsync(player);
            await _context.SaveChangesAsync();
            Player? newPlayer = await _context.Players.FirstOrDefaultAsync(
                currentPlayer => currentPlayer.Name.ToLower().Equals(player.Name.ToLower()));

            if (newPlayer == null)
            {
                throw new Exception("Could not Create the player as expected.");
            }

            return newPlayer.Id;
        }

        private async Task<int> UpdatePlayer(Player player)
        {
            Player? dbPlayer = await _context.Players
                .FirstOrDefaultAsync(currentPlayer => currentPlayer.Id == player.Id);

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
            await _context.SaveChangesAsync();
            return dbPlayer.Id;
        }

        public async Task UpsertPlayers(List<Player> players)
        {
            using (TransactionScope scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    foreach (Player player in players)
                    {
                        bool success = await UpsertPlayer(player) > 0;
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
