namespace ImageChecker_3.Tags
{
    public class ImageTag
    {
        public string A { get; set; } = string.Empty;

        public string B { get; set; } = string.Empty;

        public string C { get; set; } = string.Empty;

        public string D { get; set; } = string.Empty;

        public int X { get; set; }

        public int Y { get; set; }

        public double Scale { get; set; } = 1.0;

        public int TargetLayerIndex { get; set; } = 0;

        public bool IsDrawTag { get; set; }

        public bool IsAnimation { get; set; }

        public override string ToString()
        {
            var tagName = IsDrawTag ? "draw" : "image";
            var tagHeader = IsAnimation ? @$"<anime name=""{tagName}"" " : $"<{tagName} ";
            var drawTagStr =
                tagHeader
                + $@"a=""{A}"" "
                + $@"b=""{B}"" "
                + $@"c=""{C}"" "
                + $@"d=""{D}"" ";

            if (IsDrawTag)
            {
                return drawTagStr
                       + $@"targetLayerIndex=""{TargetLayerIndex}"" "
                       + "/>";
            }

            // ImageTag の場合は更に情報を加えて return する
            return drawTagStr
                + $@"x=""{X}"" "
                + $@"y=""{Y}"" "
                + $@"scale=""{Scale}"" "
                + $@"targetLayerIndex=""{TargetLayerIndex}"" "
                + "/>";
        }
    }
}