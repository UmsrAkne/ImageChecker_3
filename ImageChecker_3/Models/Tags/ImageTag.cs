namespace ImageChecker_3.Models.Tags
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
    }
}