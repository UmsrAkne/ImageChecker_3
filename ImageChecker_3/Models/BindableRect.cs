using Prism.Mvvm;

namespace ImageChecker_3.Models
{
    public class BindableRect : BindableBase
    {
        private double width;
        private double height;
        private double x;
        private double y;

        public BindableRect(double x, double y, double width, double height)
        {
            this.width = width;
            this.height = height;
            this.x = x;
            this.y = y;
        }

        public double Width { get => width; set => SetProperty(ref width, value); }

        public double Height { get => height; set => SetProperty(ref height, value); }

        public double X { get => x; set => SetProperty(ref x, value); }

        public double Y { get => y; set => SetProperty(ref y, value); }

        public BindableRect Clone()
        {
            return new BindableRect(X, Y, Width, Height);
        }
    }
}