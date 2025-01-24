namespace ImageChecker_3.Models.Tags
{
    public class SlideTag
    {
        public int Degree { get; set; }

        public int Distance { get; set; }

        public int Duration { get; set; }

        public int Delay { get; set; }

        public int RepeatCount { get; set; }

        public int Interval { get; set; }

        public int TargetLayerIndex { get; set; }

        public override string ToString()
        {
            return
                $"<image "
                + $@"degree=""{Degree}"" "
                + $@"distance=""{Distance}"" "
                + $@"duration=""{Duration}"" "
                + $@"delay=""{Delay}"" "
                + $@"repeatCount=""{RepeatCount}"" "
                + $@"targetLayerIndex=""{TargetLayerIndex}"" "
                + "/>";
        }
    }
}