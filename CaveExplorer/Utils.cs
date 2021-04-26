using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MechStory.Story;

namespace CaveExplorer
{
    static class Utils
    {
        public static bool MatchesTagset(this Chapter chapter, params Tag[] tags)
        {
            if(chapter.Not.Count > 0)
            {
                var not = chapter.Not.Intersect(tags).Count() > 0;

                if (not) return false;
            }

            if(chapter.Only.Count > 0)
            {
                var onlyCount = chapter.Only.Intersect(tags).Count();

                var doesContain = onlyCount >= chapter.Only.Count; //tags fully fulfil the chapter requirements

                return doesContain;
            }

            return chapter.Any.Intersect(tags).Count() > 0;
        }  
        
        public static List<Chapter> FulfillingTagset(this List<Chapter> chapters, params Tag[] tags)
        {
            return chapters.Where(x => x.MatchesTagset(tags)).ToList();
        }

        public static Tag TagFromString(this string tag)
        {
            var str = System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(tag)
                .Replace(" ", "")
                .Replace("#","")
                .Trim();

            return (Tag)Enum.Parse(typeof(Tag), str);
        }
    }
}
