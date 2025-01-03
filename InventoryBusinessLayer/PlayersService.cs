﻿using AutoMapper;

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
        private readonly IPlayersRepo _dbRepo;
        private readonly IMapper _mapper;

        public PlayersService(IPlayersRepo dbRepo, IMapper mapper)
        {
            _dbRepo = dbRepo;
            _mapper = mapper;
        }

        public async Task DeletePlayer(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Please set a valid ID");
            }

            await _dbRepo.DeletePlayer(id);
        }

        public async Task DeletePlayers(List<int> playerIds)
        {
            try
            {
                await _dbRepo.DeletePlayers(playerIds);
            }
            catch (Exception ex)
            {
                // TODO: better logging/not squelch
                Debug.WriteLine($"The transaction has failed: {ex.Message}");
                throw;
            }
        }

        public async Task<List<PlayerDto>> GetPlayers()
        {
            return await _dbRepo.GetPlayers();
        }

        public async Task<int> UpsertPlayer(Player player)
        {
            return await _dbRepo.UpsertPlayer(player);
        }

        public async Task UpsertPlayers(List<Player> players)
        {
            try
            {
                await _dbRepo.UpsertPlayers(players);
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
