using System.Collections.Generic;
using System.Text.RegularExpressions;
using ImageChecker_3.Models.Tags;

namespace ImageChecker_3.Models
{
    public static class TagParser
    {
        private static string tagPattern = @"^<(\w+)(?:\s+(\w+)=""([^""]+)"")*\s*/>$";

        public static ImageTag LoadImageTag(string input)
        {
            var result = new ImageTag();

            if (!Regex.IsMatch(input, tagPattern))
            {
                System.Diagnostics.Debug.WriteLine($"タグの形式が正しくありません (TagParser : 17)");
                return result;
            }

            var attributes = ExtractAttributes(input);

            // 属性値の変数に格納（適切な型に変換）
            result.A = attributes.GetValueOrDefault("a", string.Empty);
            result.B = attributes.GetValueOrDefault("b", string.Empty);
            result.C = attributes.GetValueOrDefault("c", string.Empty);
            result.D = attributes.GetValueOrDefault("d", string.Empty);
            result.X = int.Parse(attributes.GetValueOrDefault("x", "0"));
            result.Y = int.Parse(attributes.GetValueOrDefault("y", "0"));
            result.Scale = double.Parse(attributes.GetValueOrDefault("scale", "1.0"));

            return result;
        }

        private static Dictionary<string, string> ExtractAttributes(string input)
        {
            // 属性を抽出する正規表現
            const string attributePattern = @"(\w+)=""([^""]+)""";
            var matches = Regex.Matches(input, attributePattern);

            // 各属性を格納する辞書
            var attributes = new Dictionary<string, string>();

            foreach (Match match in matches)
            {
                var key = match.Groups[1].Value;
                var value = match.Groups[2].Value;
                attributes[key] = value;
            }

            return attributes;
        }
    }
}