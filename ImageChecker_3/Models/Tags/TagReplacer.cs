using System.Collections.Generic;
using System.Text.RegularExpressions;
using ImageChecker_3.Models.Images;

namespace ImageChecker_3.Models.Tags
{
    public static class TagReplacer
    {
        public static string ReplaceImageNames(string tag, PreviewContainer container, bool includeId = true)
        {
            var imageFileNames = container.GetImageFileNames();
            var names = new List<(string Key, string Name)>
                {
                    ("a", imageFileNames[0]),
                    ("b", imageFileNames[1]),
                    ("c", imageFileNames[2]),
                    ("d", imageFileNames[3]),
                };

            foreach (var name in names)
            {
                tag = Regex.Replace(tag, @$"{name.Key}=""[^""]*""", @$"{name.Key}=""{name.Name}""");
            }

            if (includeId)
            {
                var reg = new Regex(@"id="".*""");
                tag = reg.Replace(tag, string.Empty);

                var id = TagGenerator.GetId(tag);
                var regex = new Regex(@"\s>");
                tag = regex.Replace(tag, $@" id=""{id}"">", 1);
            }

            return tag;
        }
    }
}