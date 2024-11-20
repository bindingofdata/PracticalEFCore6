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
                _context.Items.AddRange(GenerateMovies());
                _context.Items.AddRange(GenerateBooks());
                _context.Items.AddRange(GenerateGames());

                _context.SaveChanges();
            }
        }

        private IEnumerable<Item> GenerateMovies()
        {
            return
            [
                new Item()
                {
                    Name = "Airplane!",
                    CurrentOrFinalPrice = 9.99m,
                    Category = _context.Categories.FirstOrDefault(cat => string.Equals(cat.Name, MovieCategoryNameString)),
                    Description = "Don't call me Shirley.",
                    IsOnSale = false,
                    Notes = "https://www.imdb.com/title/tt0080339/",
                    PurchasePrice = 23.99m,
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
                    Category = _context.Categories.FirstOrDefault(cat => string.Equals(cat.Name, MovieCategoryNameString)),
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
                    Category = _context.Categories.FirstOrDefault(cat => string.Equals(cat.Name, MovieCategoryNameString)),
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
                    Category = _context.Categories.FirstOrDefault(cat => string.Equals(cat.Name, MovieCategoryNameString)),
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
                    Category = _context.Categories.FirstOrDefault(cat => string.Equals(cat.Name, MovieCategoryNameString)),
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
                }
            ];
        }
        private IEnumerable<Item> GenerateBooks()
        {
            return
            [
                new Item()
                {
                    Name = "The Lord of the Rings",
                    CurrentOrFinalPrice = 44.99m,
                    Category = _context.Categories.FirstOrDefault(cat => string.Equals(cat.Name, BookCategoryNameString)),
                    Description = "Illustrated (Tolkien Illustrated Editions)",
                    IsOnSale = false,
                    Notes = "https://isbnsearch.org/isbn/9780358653035",
                    PurchasePrice = 49.99m,
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
                            IsActive = true, IsDeleted = false,
                            Name = "J. R. R. Tolkien",
                        },
                    }
                },
                new Item()
                {
                    Name = "Practical Entity Framework Core 6",
                    CurrentOrFinalPrice = 32.99m,
                    Category = _context.Categories.FirstOrDefault(cat => string.Equals(cat.Name, BookCategoryNameString)),
                    Description = "Database Access for Enterprise Applications",
                    IsOnSale = false,
                    Notes = "https://isbnsearch.org/isbn/9781484273005",
                    PurchasePrice = 49.99m,
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
                            IsActive = true, IsDeleted = false,
                            Name = "Brian L. Gorman",
                        },
                    }
                },
                new Item()
                {
                    Name = "Pro ASP.NET Core 6",
                    CurrentOrFinalPrice = 29.99m,
                    Category = _context.Categories.FirstOrDefault(cat => string.Equals(cat.Name, BookCategoryNameString)),
                    Description = "Develop Cloud-Ready Web Applications Using MVC, Blazor, and Razor Pages",
                    IsOnSale = false,
                    Notes = "https://isbnsearch.org/isbn/9781484279564",
                    PurchasePrice = 49.99m,
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
                            IsActive = true, IsDeleted = false,
                            Name = "Adam Freeman",
                        },
                    }
                },
                new Item()
                {
                    Name = "The Wayside School 4-Book Box Set",
                    CurrentOrFinalPrice = 22.99m,
                    Category = _context.Categories.FirstOrDefault(cat => string.Equals(cat.Name, BookCategoryNameString)),
                    Description = "Sideways Stories from Wayside School, Wayside School Is Falling Down, Wayside School Gets a Little Stranger, Wayside School Beneath the Cloud of Doom",
                    IsOnSale = false,
                    Notes = "https://isbnsearch.org/isbn/9780063092099",
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
                            IsActive = true, IsDeleted = false,
                            Name = "Louis Sachar",
                        },
                    }
                },
                new Item()
                {
                    Name = "Hitchhiker's Guide to the Galaxy Trilogy Collection",
                    CurrentOrFinalPrice = 34.99m,
                    Category = _context.Categories.FirstOrDefault(cat => string.Equals(cat.Name, BookCategoryNameString)),
                    Description = "5 Books Set",
                    IsOnSale = false,
                    Notes = "https://isbnsearch.org/isbn/9789123918430",
                    PurchasePrice = 44.99m,
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
                            IsActive = true, IsDeleted = false,
                            Name = "Douglas Adams",
                        },
                    }
                }
            ];
        }
        private IEnumerable<Item> GenerateGames()
        {
            return
            [
                new Item()
                {
                    Name = "Age of Empires IV",
                    CurrentOrFinalPrice = 39.99m,
                    Category = _context.Categories.FirstOrDefault(cat => string.Equals(cat.Name, GameCategoryNameString)),
                    Description = "The award-winning and best-selling strategy franchise continues with Age of Empires IV: Anniversary Edition, putting you at the center of even more epic historical battles that shaped the world.",
                    IsOnSale = false,
                    Notes = "https://store.steampowered.com/app/1466860/Age_of_Empires_IV_Anniversary_Edition/",
                    PurchasePrice = 59.99m,
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
                            IsActive = true, IsDeleted = false, Description = "Developer",
                            Name = "Relic Entertainment",
                        },
                        new Player()
                        {
                            CreatedDate = DateTime.Now, CreatedByUserId = SEED_USER_ID,
                            IsActive = true, IsDeleted = false, Description = "Developer",
                            Name = "Forgotten Empires",
                        },
                        new Player()
                        {
                            CreatedDate = DateTime.Now, CreatedByUserId = SEED_USER_ID,
                            IsActive = true, IsDeleted = false, Description = "Developer",
                            Name = "Climax Studios",
                        },
                        new Player()
                        {
                            CreatedDate = DateTime.Now, CreatedByUserId = SEED_USER_ID,
                            IsActive = true, IsDeleted = false, Description = "Developer",
                            Name = "World's Edge",
                        },
                        new Player()
                        {
                            CreatedDate = DateTime.Now, CreatedByUserId = SEED_USER_ID,
                            IsActive = true, IsDeleted = false, Description = "Publisher",
                            Name = "Xbox Game Studios",
                        }
                    }
                },
                new Item()
                {
                    Name = "Final Fantasy XIV Online",
                    CurrentOrFinalPrice = 19.99m,
                    Category = _context.Categories.FirstOrDefault(cat => string.Equals(cat.Name, GameCategoryNameString)),
                    Description = "Experience an unforgettable story, exhilarating battles, and a myriad of captivating environments to explore.",
                    IsOnSale = false,
                    Notes = "https://store.steampowered.com/app/39210/FINAL_FANTASY_XIV_Online/",
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
                            IsActive = true, IsDeleted = false, Description = "Developer/Publisher",
                            Name = "Square Enix",
                        },
                    }
                },
                new Item()
                {
                    Name = "Call of Duty: Modern Warfare 2",
                    CurrentOrFinalPrice = 19.99m,
                    Category = _context.Categories.FirstOrDefault(cat => string.Equals(cat.Name, GameCategoryNameString)),
                    Description = "Modern Warfare 2 continues the gripping and heart-racing action as players face off against a new threat dedicated to bringing the world to the brink of collapse.",
                    IsOnSale = false,
                    Notes = "https://store.steampowered.com/app/10180/Call_of_Duty_Modern_Warfare_2_2009/",
                    PurchasePrice = 59.99m,
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
                            IsActive = true, IsDeleted = false, Description = "Developer",
                            Name = "Infinity Ward",
                        },
                        new Player()
                        {
                            CreatedDate = DateTime.Now, CreatedByUserId = SEED_USER_ID,
                            IsActive = true, IsDeleted = false, Description = "Publisher",
                            Name = "Activision",
                        }
                    }
                },
                new Item()
                {
                    Name = "Fallout 4",
                    CurrentOrFinalPrice = 19.99m,
                    Category = _context.Categories.FirstOrDefault(cat => string.Equals(cat.Name, GameCategoryNameString)),
                    Description = "As the sole survivor of Vault 111, you enter a world destroyed by nuclear war.",
                    IsOnSale = false,
                    Notes = "https://store.steampowered.com/app/377160/Fallout_4/",
                    PurchasePrice = 59.99m,
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
                            IsActive = true, IsDeleted = false, Description = "Developer",
                            Name = "Bethesda Game Studios",
                        },
                        new Player()
                        {
                            CreatedDate = DateTime.Now, CreatedByUserId = SEED_USER_ID,
                            IsActive = true, IsDeleted = false, Description = "Publisher",
                            Name = "Bethesda Softworks",
                        }
                    }
                },
                new Item()
                {
                    Name = "Slay the Spire",
                    CurrentOrFinalPrice = 24.99m,
                    Category = _context.Categories.FirstOrDefault(cat => string.Equals(cat.Name, GameCategoryNameString)),
                    Description = "Craft a unique deck, encounter bizarre creatures, discover relics of immense power, and Slay the Spire!",
                    IsOnSale = false,
                    Notes = "https://store.steampowered.com/app/646570/Slay_the_Spire/",
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
                            IsActive = true, IsDeleted = false, Description = "Developer/Publisher",
                            Name = "Mega Crit",
                        },
                    }
                }
            ];
        }

        private const string MovieCategoryNameString = "Movies";
        private const string BookCategoryNameString = "Books";
        private const string GameCategoryNameString = "Games";
    }
}
