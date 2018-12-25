using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace BallingTimeBackend.Models
{
    public class DbInitializer
    {
        public static void Initialize(BallingContext context)
        {
            if (context.Database.EnsureCreated()) {
                #region Parsing site to get drills
                var url = "https://www.basketballforcoaches.com/50-basketball-dribbling-drills/";
                var web = new HtmlWeb();
                var doc = web.Load(url);

                var videoRef = "https://www.youtube.com/watch?v=bnjed9YVCRs";

                List<string> drillNamesList = GetDrillNames(doc);
                List<string> drillDescriptionsList = GetDrillDescriptions(doc);


                for (int i = 0; i < drillNamesList.Count; ++i)
                {
                    context.DribblingDrills.Add(new DribblingDrill()
                    {
                        Description = drillDescriptionsList[i],
                        Name = drillNamesList[i],
                        VideoReference = videoRef
                    });
                }
                #endregion

                context.Difficulties.Add(new Difficulty()
                {
                    DifficultyLevel = 1,
                    SecondsForExercise = 30
                });
                context.Difficulties.Add(new Difficulty()
                {
                    DifficultyLevel = 2,
                    SecondsForExercise = 45
                });
                
                context.SaveChanges();

                #region Adding training program
                var easyDifficulty = context.Difficulties.Where(x => x.DifficultyLevel.Equals(1)).First();
                var mediumDifficulty = context.Difficulties.Where(x => x.DifficultyLevel.Equals(2)).First();

                var dribblingDrills = context.DribblingDrills.ToList();
                
                for (int i = 0; i < dribblingDrills.Count; ++i) {
                    if (i < dribblingDrills.Count / 2) {
                        context.TrainingPrograms.Add(new TrainingProgram()
                        {
                            DribblingDrill = dribblingDrills[i],
                            Difficulty = easyDifficulty
                        });

                        context.TrainingPrograms.Add(new TrainingProgram()
                        {
                            DribblingDrill = dribblingDrills[i],
                            Difficulty = mediumDifficulty
                        });
                    }
                    else {
                        context.TrainingPrograms.Add(new TrainingProgram()
                        {
                            DribblingDrill = dribblingDrills[i],
                            Difficulty = mediumDifficulty
                        });
                    }
                }
                #endregion

                context.Users.Add(new User()
                {
                    DifficultyId = context.Difficulties.Where(dif => dif.DifficultyLevel == 1).First().Id,
                    Email = "john@google.com",
                    Name = "John",
                    Password = "john",
                    PracticeDays = JsonConvert.SerializeObject(new List<int>() { 0, 1, 2, 3, 4, 5, 6 })
                });

                context.SaveChanges();
                
                #region Adding artificial user stats
                DateTime currentDay = DateTime.Now.AddDays(-10);
                User john = context.Users.Where(user => user.Email == "john@google.com").First();
                List<int> drillIdList = 
                    context
                    .TrainingPrograms
                    .Where(tp => tp.DifficultyId == john.DifficultyId)
                    .Select(x=>x.DribblingDrillId)
                    .ToList();

                var startAccuracy = 0.7;
                var endAccuracy = 0.9;

                var startAverageSpeed = 10;
                var endAverageSpeed = 20;

                var startRepetitionsPerSecond = 0.7;
                var endRepetitionsPerSecond = 1;

                Random r = new Random();
                for (int i = 1; i <= 10; i++) {
                    for (int j = 0; j < drillIdList.Count; j++) {
                        context.UserProgresses.Add(new UserProgress()
                        {
                            Accuracy = Math.Sqrt(startAccuracy * startAccuracy 
                            + (endAccuracy * endAccuracy - startAccuracy * startAccuracy) / 10 * (i + r.NextDouble())),

                            AverageSpeed = Math.Sqrt(startAverageSpeed * startAverageSpeed 
                            + (endAverageSpeed * endAverageSpeed - startAverageSpeed * startAverageSpeed) / 10 * (i + r.NextDouble())),

                            RepeationsPerSecond = Math.Sqrt(startRepetitionsPerSecond * startRepetitionsPerSecond
                            + (endRepetitionsPerSecond * endRepetitionsPerSecond 
                            - startRepetitionsPerSecond * startRepetitionsPerSecond) / 10 * (i + r.NextDouble())),

                            Date = currentDay,
                            DribblingDrillId = drillIdList[j],
                            IsCompleted = true,
                            UserId = john.Id
                        });
                    }
                    currentDay = currentDay.AddDays(1);
                }
                #endregion

                context.SaveChanges();
            }
            //CheckUserLevelTest(context);
        }

        private static List<string> GetDrillNames(HtmlDocument doc)
        {
            var drillNames = doc.DocumentNode
                .Descendants("strong");

            List<string> drillNamesList = new List<string>();
            foreach (var node in drillNames)
            {
                if (node.InnerHtml.Contains("Freestyle")) break;

                if (Int32.TryParse(node.InnerHtml[0].ToString(), out int number))
                {
                    drillNamesList.Add(node.InnerHtml.Substring(node.InnerHtml.IndexOf('.') + 2).Replace("&#8211;", "-"));
                }
            }
            return drillNamesList;
        }

        private static List<string> GetDrillDescriptions(HtmlDocument doc)
        {
            var drillDescriptions = doc.DocumentNode
                .Descendants("p");

            StringBuilder s = new StringBuilder();

            foreach (var desc in drillDescriptions)
            {
                s.Append(desc.InnerHtml);
            }


            Regex r1 = new Regex(@"<br>(.*?)<");
            MatchCollection matches = r1.Matches(s.ToString());

            List<string> drillDescriptionsList = new List<string>();
            foreach (Match m in matches)
            {
                if (m.Groups[1].Value.Contains("Using all the moves")) break;
                drillDescriptionsList.Add(m.Groups[1]
                                .Value
                                .Replace("&#8217;", "'")
                                .Replace("&#8216;", "'"));
            }

            return drillDescriptionsList;
        }

        private static void CheckUserLevelTest(BallingContext context) {
            context.Users.Add(new User()
            {
                DifficultyId = context.Difficulties.Where(dif => dif.DifficultyLevel == 1).First().Id,
                Email = "ben@yandex.ru",
                Name = "Ben",
                Password = "ben",
                PracticeDays = JsonConvert.SerializeObject(new List<int>() { 0, 1, 2, 3, 4, 5, 6 })
            });
            context.SaveChanges();
            User ben = context.Users.Where(user => user.Email == "ben@yandex.ru").First();
            List<int> drillIds =
                    context
                    .TrainingPrograms
                    .Where(tp => tp.DifficultyId == ben.DifficultyId)
                    .Select(x => x.DribblingDrillId)
                    .ToList();

            for (int i = 0; i < drillIds.Count; ++i)
            {
                context.UserProgresses.Add(new UserProgress()
                {
                    Accuracy = 0.5,

                    AverageSpeed = 0.5,

                    RepeationsPerSecond = 0.5,

                    Date = DateTime.Now.AddDays(-5),
                    DribblingDrillId = drillIds[i],
                    IsCompleted = true,
                    UserId = ben.Id
                });
            }
            for (int i = 0; i < drillIds.Count; ++i)
            {
                context.UserProgresses.Add(new UserProgress()
                {
                    Accuracy = 1,

                    AverageSpeed = 1,

                    RepeationsPerSecond = 1,

                    Date = DateTime.Now.AddDays(-3),
                    DribblingDrillId = drillIds[i],
                    IsCompleted = true,
                    UserId = ben.Id
                });
            }
            context.SaveChanges();
        }
    }
}
