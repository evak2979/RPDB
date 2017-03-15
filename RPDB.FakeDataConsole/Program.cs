using System;
using System.IO;
using System.Linq;
using Bogus;
using Nest;
using Newtonsoft.Json;
using RODB.ElasticSearch;
using RPDB.Domain.Models;

namespace RPDB.FakeDataConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Randomizer.Seed = new Random(3897234);
            var random = new Bogus.Randomizer();
            var guildNames = new[] {null, "Guild A", "Guild B", "Guild C", "Guild D", "Guild E"};
            var faction = new [] {"Horde", "Alliance", "Undefined"};
            var race = new[]
            {
                "Human", "Kaldorei", "Gnome", "Dwarf", "Worgen", "Orc", "Tauren",
                "Forsaken", "Sindorei", "Goblin", "Pandaren"
            };

            var isActive = new[] {true, false};

            var dateInfo = new Faker<DateInfo>()
                .StrictMode(true)
                .RuleFor(x => x.CreatedDate, f => f.Date.Recent())
                .RuleFor(x => x.UpdatedDate, f => f.Date.Recent());

            var story = new Faker<Story>()
                .StrictMode(true)
                .RuleFor(x => x.Body, f => f.Lorem.Paragraphs(3))
                .RuleFor(x => x.Id, f => f.IndexFaker)
                .RuleFor(x => x.Title, f => f.Lorem.Sentence())
                .RuleFor(x => x.DateInfo, f => dateInfo.Generate());

            var picture = new Faker<Picture>()
                .StrictMode(true)
                .RuleFor(x => x.Id, f => f.IndexFaker)
                .RuleFor(x => x.Caption, f => f.Lorem.Sentence())
                .RuleFor(x => x.Url, f => f.Internet.Url());

            var guild = new Faker<Guild>()
                .StrictMode(true)
                .RuleFor(x => x.Name, f => f.PickRandom(guildNames))
                .RuleFor(x => x.Description, f => f.Lorem.Paragraphs(3));

            var character = new Faker<Character>()
                .StrictMode(true)
                .RuleFor(x => x.Id, f => f.IndexFaker)
                .RuleFor(x => x.FirstName, f => f.Name.FirstName())
                .RuleFor(x => x.LastName, f => f.Name.LastName())
                .RuleFor(x => x.EyeColor, f => f.Commerce.Color())
                .RuleFor(x => x.EyeColor, f => f.Commerce.Color())
                .RuleFor(x => x.Weight, f => f.Lorem.Sentence())
                .RuleFor(x => x.Height, f => f.Lorem.Sentence())
                .RuleFor(x => x.SkinColor, f => f.Commerce.Color())
                .RuleFor(x => x.Race, f => f.PickRandom(race))
                .RuleFor(x => x.Faction, f => f.PickRandom(faction))
                .RuleFor(x => x.Background, f => f.Lorem.Paragraphs(7))
                .RuleFor(x => x.Residence, f => f.Lorem.Word())
                .RuleFor(x => x.Occupation, f => f.Lorem.Sentence())
                .RuleFor(x => x.PhysicalAppearance, f => f.Lorem.Paragraphs(2))
                .RuleFor(x => x.Friends, f => f.Lorem.Words().ToList())
                .RuleFor(x => x.Stories, f => story.Generate(random.Number(1,10)).ToList())
                .RuleFor(x => x.Pictures, f => picture.Generate(random.Number(1, 10)).ToList())
                .RuleFor(x=>x.Guild, f=> guild.Generate())
                .RuleFor(x => x.DateInfo, f => dateInfo.Generate());

            

            var user = new Faker<User>()
                .StrictMode(true)
                .RuleFor(x => x.UserName, f=>f.Internet.UserName())
                .RuleFor(x => x.DateInfo, dateInfo.Generate())
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.Image, f => f.Internet.Url())
                .RuleFor(x => x.IsActive, f => f.PickRandom(isActive))
                .RuleFor(x => x.Characters, f => character.Generate(random.Number(1,10)).ToList());


            var users = user.Generate(1000);
            //string charsJson = JsonConvert.SerializeObject(users);
            ElasticClient client = ElasticClientFactory.GetClient();
            //File.WriteAllText(@"c:\dump\users.txt", charsJson);

            var result = client.Bulk(b => b.IndexMany(users));

            if (!result.IsValid)
            {
                foreach (var item in result.ItemsWithErrors)
                    Console.WriteLine("Failed to index document {0}: {1}", item.Id, item.Error);

                Console.WriteLine(result.DebugInformation);
                Console.Read();
                Environment.Exit(1);
            }

            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
