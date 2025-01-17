﻿using InventoryModels;
using InventoryModels.Dtos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryBusinessLayer
{
    public interface IPlayersService
    {
        Task<List<PlayerDto>> GetPlayers();
        Task<int> UpsertPlayer(Player player);
        Task UpsertPlayers(List<Player> players);
        Task DeletePlayer(int id);
        Task DeletePlayers(List<int> playerIds);
    }
}
