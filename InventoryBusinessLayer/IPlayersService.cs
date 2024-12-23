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
        List<PlayerDto> GetPlayers();
        int UpsertPlayer(Player player);
        void UpsertPlayers(List<Player> players);
        void DeletePlayer(int id);
        void DeletePlayers(List<int> playerIds);
    }
}
