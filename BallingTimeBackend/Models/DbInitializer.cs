using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;

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

                context.SaveChanges();
            }
                
        }

        static List<string> GetDrillNames(HtmlDocument doc)
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

        static List<string> GetDrillDescriptions(HtmlDocument doc)
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
    }
}
