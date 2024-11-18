using libDB;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryModels;

namespace InventoryDataMigrator
{
    public sealed class ItemBuilder : BaseBuilder
    {
        public ItemBuilder(InventoryDbContext context) : base(context) { }

        public override void GenerateSeedData()
        {
            if (!_context.Items.Any())
            {
                _context.Items.AddRange(
                    new Item()
                    {
                        Name = "Airplane!", CurrentOrFinalPrice = 9.99m,
                        Description = "Don't call me Shirley.", IsOnSale = false,
                        Notes = "https://www.imdb.com/title/tt0080339/", PurchasePrice = 23.99m,
                        PurchasedDate = null, Quantity = 1_000, SoldDate = null,
                        CreatedByUserId = SEED_USER_ID, CreatedDate = DateTime.Now,
                        IsDeleted = false, IsActive = true, Players = new List<Player>()
                        {
                            new Player()
                            {
                                CreatedDate = DateTime.Now, CreatedByUserId = SEED_USER_ID,
                                IsActive = true, IsDeleted = false, Description = "https://www.imdb.com/name/nm0001332/",
                                Name = "Robert Hays",
                            },
                            new Player()
                            {
                                CreatedDate = DateTime.Now, CreatedByUserId = SEED_USER_ID,
                                IsActive = true, IsDeleted = false, Description = "https://www.imdb.com/name/nm0353546/",
                                Name = "Julie Hagerty",
                            }
                        }
                    },
                    new Item()
                    {
                        Name = "The Rocky Horror Picture Show",
                        CurrentOrFinalPrice = 14.99m,
                        Description = "Turn yourself over, to absolute pleasure.",
                        IsOnSale = false,
                        Notes = "https://www.imdb.com/title/tt0073629/",
                        PurchasePrice = 29.99m,
                        PurchasedDate = null,
                        Quantity = 1_000,
                        SoldDate = null,
                        CreatedByUserId = SEED_USER_ID,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                        Players = new List<Player>()
                        {
                            new Player()
                            {
                                CreatedDate = DateTime.Now, CreatedByUserId = SEED_USER_ID,
                                IsActive = true, IsDeleted = false, Description = "https://www.imdb.com/name/nm0000347/",
                                Name = "Tim Curry",
                            },
                            new Player()
                            {
                                CreatedDate = DateTime.Now, CreatedByUserId = SEED_USER_ID,
                                IsActive = true, IsDeleted = false, Description = "https://www.imdb.com/name/nm0000215/",
                                Name = "Susan Sarandon",
                            },
                            new Player()
                            {
                                CreatedDate = DateTime.Now, CreatedByUserId = SEED_USER_ID,
                                IsActive = true, IsDeleted = false, Description = "https://www.imdb.com/name/nm0000960/",
                                Name = "Barry Bostwick",
                            }
                        }
                    },
                    new Item()
                    {
                        Name = "Elf",
                        CurrentOrFinalPrice = 13.99m,
                        Description = "Son of a nutcracker!",
                        IsOnSale = false,
                        Notes = "https://www.imdb.com/title/tt0319343/",
                        PurchasePrice = 14.99m,
                        PurchasedDate = null,
                        Quantity = 1_000,
                        SoldDate = null,
                        CreatedByUserId = SEED_USER_ID,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                        Players = new List<Player>()
                        {
                            new Player()
                            {
                                CreatedDate = DateTime.Now, CreatedByUserId = SEED_USER_ID,
                                IsActive = true, IsDeleted = false, Description = "https://www.imdb.com/name/nm0002071/",
                                Name = "Will Ferrell",
                            },
                            new Player()
                            {
                                CreatedDate = DateTime.Now, CreatedByUserId = SEED_USER_ID,
                                IsActive = true, IsDeleted = false, Description = "https://www.imdb.com/name/nm0001001/",
                                Name = "James Caan",
                            }
                        }
                    },
                    new Item()
                    {
                        Name = "Mars Attacks!",
                        CurrentOrFinalPrice = 13.99m,
                        Description = "Ack ack. ACK!!",
                        IsOnSale = false,
                        Notes = "https://www.imdb.com/title/tt0116996/",
                        PurchasePrice = 14.99m,
                        PurchasedDate = null,
                        Quantity = 1_000,
                        SoldDate = null,
                        CreatedByUserId = SEED_USER_ID,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                        Players = new List<Player>()
                        {
                            new Player()
                            {
                                CreatedDate = DateTime.Now, CreatedByUserId = SEED_USER_ID,
                                IsActive = true, IsDeleted = false, Description = "https://www.imdb.com/name/nm0001305/",
                                Name = "Lukas Haas",
                            },
                            new Player()
                            {
                                CreatedDate = DateTime.Now, CreatedByUserId = SEED_USER_ID,
                                IsActive = true, IsDeleted = false, Description = "https://www.imdb.com/name/nm0000204/",
                                Name = "Natalie Portman",
                            }
                        }
                    },
                    new Item()
                    {
                        Name = "Batman The Movie",
                        CurrentOrFinalPrice = 13.99m,
                        Description = "Sometimes you just can't get rid of a bomb.",
                        IsOnSale = false,
                        Notes = "https://www.imdb.com/title/tt0060153/",
                        PurchasePrice = 14.99m,
                        PurchasedDate = null,
                        Quantity = 1_000,
                        SoldDate = null,
                        CreatedByUserId = SEED_USER_ID,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                        Players = new List<Player>()
                        {
                            new Player()
                            {
                                CreatedDate = DateTime.Now, CreatedByUserId = SEED_USER_ID,
                                IsActive = true, IsDeleted = false, Description = "https://www.imdb.com/name/nm0911431/",
                                Name = "Burt Ward",
                            },
                            new Player()
                            {
                                CreatedDate = DateTime.Now, CreatedByUserId = SEED_USER_ID,
                                IsActive = true, IsDeleted = false, Description = "https://www.imdb.com/name/nm0001842/",
                                Name = "Adam West",
                            }
                        }
                    });

                _context.SaveChanges();
            }
        }
    }
}
