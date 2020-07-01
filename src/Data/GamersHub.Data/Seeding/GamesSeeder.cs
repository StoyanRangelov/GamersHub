namespace GamersHub.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using GamersHub.Data.Models;

    public class GamesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Games.Any())
            {
                return;
            }

            var games = new List<Game>
            {
                new Game
                {
                    Title = "World of Warcraft",
                    SubTitle = "The Burning Crusade",
                    Description = @"<p>World of Warcraft: The Burning Crusade is the first expansion set for the MMORPG World of Warcraft.
                                    It was released on January 16, 2007 at local midnight in Europe and North America, selling nearly 2.4 million copies on release day alone and making it, at the time,
                                    the fastest-selling PC game released at that point.[2] Approximately 3.53 million copies were sold in the first month of release, including 1.9 million in North America,
                                     over 100,000 copies in Australasia, and nearly 1.6 million in Europe.[3]</p>",
                    ImgUrl = "https://res.cloudinary.com/dhagvwgd6/image/upload/v1587466948/hzeuhrc32rxdvxfyuffe.jpg",
                },
                new Game
                {
                    Title = "Dota 2",
                    Description = "<p><em><strong>Dota 2</strong></em>&nbsp;is a&nbsp;<a title=\"Multiplayer online battle arena\" href=\"https://en.wikipedia.org/wiki/Multiplayer_online_battle_arena\">multiplayer online battle arena</a>&nbsp;(MOBA) video game developed and published by&nbsp;<a title=\"Valve Corporation\" href=\"https://en.wikipedia.org/wiki/Valve_Corporation\">Valve</a>. The game is a sequel to&nbsp;<em><a title=\"Defense of the Ancients\" href=\"https://en.wikipedia.org/wiki/Defense_of_the_Ancients\">Defense of the Ancients</a></em>&nbsp;(<em>DotA</em>), which was a community-created&nbsp;<a class=\"mw-redirect\" title=\"Mod (video gaming)\" href=\"https://en.wikipedia.org/wiki/Mod_(video_gaming)\">mod</a>&nbsp;for&nbsp;<a title=\"Blizzard Entertainment\" href=\"https://en.wikipedia.org/wiki/Blizzard_Entertainment\">Blizzard Entertainment</a>'s&nbsp;<em><a title=\"Warcraft III: Reign of Chaos\" href=\"https://en.wikipedia.org/wiki/Warcraft_III:_Reign_of_Chaos\">Warcraft III: Reign of Chaos</a></em>&nbsp;and its expansion pack,&nbsp;<em><a class=\"mw-redirect\" title=\"The Frozen Throne\" href=\"https://en.wikipedia.org/wiki/The_Frozen_Throne\">The Frozen Throne</a></em>.&nbsp;<em>Dota 2</em>&nbsp;is played in matches between two teams of five players, with each team occupying and defending their own separate base on the&nbsp;<a class=\"mw-redirect\" title=\"Level (video gaming)\" href=\"https://en.wikipedia.org/wiki/Level_(video_gaming)\">map</a>. Each of the ten players independently controls a powerful character, known as a \"hero\", who all have unique&nbsp;<a class=\"mw-redirect\" title=\"Skill (role-playing games)\" href=\"https://en.wikipedia.org/wiki/Skill_(role-playing_games)\">abilities</a>&nbsp;and differing styles of play. During a match, players collect&nbsp;<a title=\"Experience point\" href=\"https://en.wikipedia.org/wiki/Experience_point\">experience points</a>&nbsp;and&nbsp;<a class=\"mw-redirect\" title=\"Item (gaming)\" href=\"https://en.wikipedia.org/wiki/Item_(gaming)\">items</a>&nbsp;for their heroes to successfully defeat the opposing team's heroes in&nbsp;<a title=\"Player versus player\" href=\"https://en.wikipedia.org/wiki/Player_versus_player\">player versus player</a>&nbsp;combat. A team wins by being the first to destroy the other team's \"Ancient\", a large structure located within their base.</p>",
                    ImgUrl = "https://res.cloudinary.com/dhagvwgd6/image/upload/v1586684009/mf4dkes9e6vtbwjjuj4o.jpg",
                },
                new Game
                {
                    Title = "Doom Eternal",
                    Description = @"<p>Doom Eternal (stylized as DOOM Eternal) is a first person shooter video game developed by id Software and published by Bethesda Softworks.
                                     It is the fifth main game in the Doom series and a direct sequel to 2016's Doom, and was released on March 20, 2020, for Windows, PlayStation 4, Stadia and Xbox One, with a Nintendo Switch
                                     version planned for a release at a later date. The game received positive reviews, with praise for its campaign, graphics, level design, soundtrack and combat mechanics, while some disliked the game's increased focus on storytelling.</p>",
                    ImgUrl = "https://res.cloudinary.com/dhagvwgd6/image/upload/v1586684089/g2wshq27gqgddfxrvdwb.jpg",
                },
                new Game
                {
                    Title = "Starcraft 2",
                    SubTitle = "Hear of The Swarm",
                    Description = @"<p>StarCraft II: Heart of the Swarm is an expansion pack to the military science fiction real-time strategy game StarCraft II: Wings of Liberty, and the second part of the StarCraft II trilogy developed by Blizzard Entertainment,
                                    with the final part being Legacy of the Void.[3] The game was released on March 12, 2013.</p><p>The expansion includes additional units and multiplayer changes from Wings of Liberty, as well as a continuing campaign focusing on the Zerg race and following Sarah Kerrigan
                                    in her effort to regain control of the swarm and exact her revenge on the Terran Dominion's emperor, Arcturus Mengsk.</p><p>During BlizzCon 2017, Blizzard announced that StarCraft II would be re-branded as a free-to-play game, hence opening the multiplayer mode to everybody
                                    and bringing some changes to previously paid features of the game.[4] The Wings of Liberty campaign was made completely free while the campaigns for Heart of the Swarm and Legacy of the Void still required payment. However, those who had already bought Wings of Liberty before the
                                    free-to-play announcement were gifted the Heart of the Swarm campaign free of charge. This new free-to-play model and changes to the availability of the campaigns was in line with Blizzard's vision to support the game differently going forward. Micro-transactions such as Skins, Co-op Commanders,
                                    Voice Packs, and the War Chests proved to be successful enough to sustain StarCraft II as a story-driven and eSport title.</p>",
                    ImgUrl = "https://res.cloudinary.com/dhagvwgd6/image/upload/v1586684122/ebxf39idbdwvmv8hmh5e.jpg",
                },
                new Game
                {
                    Title = "Destiny 2",
                    Description = "<p><em><strong>Destiny 2</strong></em>&nbsp;is a&nbsp;<a title=\"Free-to-play\" href=\"https://en.wikipedia.org/wiki/Free-to-play\">free-to-play</a>&nbsp;<a title=\"Online game\" href=\"https://en.wikipedia.org/wiki/Online_game\">online-only</a>&nbsp;<a class=\"mw-redirect\" title=\"Multiplayer\" href=\"https://en.wikipedia.org/wiki/Multiplayer\">multiplayer</a>&nbsp;<a title=\"First-person shooter\" href=\"https://en.wikipedia.org/wiki/First-person_shooter\">first-person shooter</a>&nbsp;<a title=\"Video game\" href=\"https://en.wikipedia.org/wiki/Video_game\">video game</a>&nbsp;developed by&nbsp;<a title=\"Bungie\" href=\"https://en.wikipedia.org/wiki/Bungie\">Bungie</a>. It was released for&nbsp;<a title=\"PlayStation 4\" href=\"https://en.wikipedia.org/wiki/PlayStation_4\">PlayStation 4</a>&nbsp;and&nbsp;<a title=\"Xbox One\" href=\"https://en.wikipedia.org/wiki/Xbox_One\">Xbox One</a>&nbsp;on September 6, 2017, followed by a&nbsp;<a title=\"Microsoft Windows\" href=\"https://en.wikipedia.org/wiki/Microsoft_Windows\">Microsoft Windows</a>&nbsp;version the following month.<sup id=\"cite_ref-2\" class=\"reference\"><a href=\"https://en.wikipedia.org/wiki/Destiny_2#cite_note-2\">[1]</a></sup><sup id=\"cite_ref-3\" class=\"reference\"><a href=\"https://en.wikipedia.org/wiki/Destiny_2#cite_note-3\">[2]</a></sup>&nbsp;The game was published by&nbsp;<a title=\"Activision\" href=\"https://en.wikipedia.org/wiki/Activision\">Activision</a>&nbsp;until December 31 2018&nbsp;<sup id=\"cite_ref-4\" class=\"reference\"><a href=\"https://en.wikipedia.org/wiki/Destiny_2#cite_note-4\">[3]</a></sup>, when Bungie acquired the publishing rights to the franchise. It is the sequel to 2014's&nbsp;<em><a title=\"Destiny (video game)\" href=\"https://en.wikipedia.org/wiki/Destiny_(video_game)\">Destiny</a></em>&nbsp;and its&nbsp;<a title=\"Destiny post-release content\" href=\"https://en.wikipedia.org/wiki/Destiny_post-release_content\">subsequent expansions</a>. Set in a \"<a title=\"Science fantasy\" href=\"https://en.wikipedia.org/wiki/Science_fantasy\">mythic science fiction</a>\" world, the game features a multiplayer \"shared-world\" environment with elements of&nbsp;<a title=\"Role-playing video game\" href=\"https://en.wikipedia.org/wiki/Role-playing_video_game\">role-playing</a>&nbsp;games. Like the original, activities in&nbsp;<em>Destiny 2</em>&nbsp;are divided among&nbsp;<a title=\"Player versus environment\" href=\"https://en.wikipedia.org/wiki/Player_versus_environment\">player versus environment</a>&nbsp;(PvE) and&nbsp;<a title=\"Player versus player\" href=\"https://en.wikipedia.org/wiki/Player_versus_player\">player versus player</a>&nbsp;(PvP) game types. In addition to normal story missions, PvE features three-player \"<a title=\"Dungeon crawl\" href=\"https://en.wikipedia.org/wiki/Dungeon_crawl\">strikes</a>\" and six-player&nbsp;<a class=\"mw-redirect\" title=\"Raid (gaming)\" href=\"https://en.wikipedia.org/wiki/Raid_(gaming)\">raids</a>. A free roam patrol mode is also available for each planet and features public events as well as new activities not featured in the original. These new activities have an emphasis on exploration of the planets and interactions with&nbsp;<a class=\"mw-redirect\" title=\"Non-player characters\" href=\"https://en.wikipedia.org/wiki/Non-player_characters\">non-player characters</a>&nbsp;(NPCs); the original&nbsp;<em>Destiny</em>&nbsp;only featured NPCs in social spaces. PvP features objective-based modes, as well as traditional&nbsp;<a title=\"Deathmatch\" href=\"https://en.wikipedia.org/wiki/Deathmatch\">deathmatch</a>&nbsp;game modes.</p>",
                    ImgUrl = "https://res.cloudinary.com/dhagvwgd6/image/upload/v1586684174/yanraveohgbyreq3dotg.jpg",
                },
            };

            foreach (var game in games)
            {
                await dbContext.Games.AddAsync(new Game
                {
                    Title = game.Title,
                    SubTitle = game.SubTitle,
                    Description = game.Description,
                    ImgUrl = game.ImgUrl,
                });
            }
        }
    }
}
