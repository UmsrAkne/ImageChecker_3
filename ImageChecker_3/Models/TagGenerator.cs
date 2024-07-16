using System.Globalization;
using ImageChecker_3.Models.Images;

namespace ImageChecker_3.Models
{
    public static class TagGenerator
    {
        public static string GetTag(string baseText, PreviewContainer previewContainer)
        {
            var relPosition = previewContainer.RelativePosition;
            var ws = previewContainer.GetImageFileNames();
            return baseText
                .Replace("$a", ws[0])
                .Replace("$b", ws[1])
                .Replace("$c", ws[2])
                .Replace("$d", ws[3])
                .Replace("$scale", previewContainer.Scale.ToString("0.0", CultureInfo.InvariantCulture))
                .Replace("$x", ((int)relPosition.X).ToString(CultureInfo.CurrentCulture))
                .Replace("$y", ((int)relPosition.Y).ToString(CultureInfo.CurrentCulture));
        }
    }
}