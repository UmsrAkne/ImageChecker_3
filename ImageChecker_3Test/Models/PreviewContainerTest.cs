using System.Windows;
using ImageChecker_3.Models.Images;
using NUnit.Framework;

namespace ImageChecker_3Test.Models
{
    [TestFixture]
    public class PreviewContainerTest
    {
        private PreviewContainer previewContainer;

        [SetUp]
        public void Setup()
        {
            previewContainer = new PreviewContainer();

            previewContainer.SetImageWrappers(
                new ImageWrapper(new ImageFileInfo(){Width = 1280, Height = 720,}),
                new ImageWrapper(new ImageFileInfo(){Width = 1280, Height = 720,}),
                new ImageWrapper(new ImageFileInfo(){Width = 1280, Height = 720,}),
                new ImageWrapper(new ImageFileInfo(){Width = 1280, Height = 720,})
                );
        }

        [Test]
        [TestCase(1.0, 100, 200)]
        [TestCase(1.5, 420, 380)]
        [TestCase(2.0, 740, 560)]
        public void RelativePositionTest(double scale, double x, double y)
        {
            previewContainer.Scale = scale;
            Assert.That(new Point(previewContainer.X, previewContainer.Y), Is.EqualTo(new Point(0, 0)));

            previewContainer.X = 100;
            previewContainer.Y = 200;

            var actual = previewContainer.RelativePosition;
            Assert.That(actual, Is.EqualTo(new Point(x, y)));
        }

        [Test]
        [TestCase(1.0, 0, 200)]
        [TestCase(1.5, 320, 380)]
        [TestCase(2.0, 640, 560)]
        public void RelativePositionTest_画面サイズ1480(double scale, double x, double y)
        {
            previewContainer.Scale = scale;
            previewContainer.ScreenRect = new Rect(0, 0, 1480, 720);
            Assert.That(new Point(previewContainer.X, previewContainer.Y), Is.EqualTo(new Point(0, 0)));

            previewContainer.X = 100;
            previewContainer.Y = 200;

            var actual = previewContainer.RelativePosition;
            Assert.That(actual, Is.EqualTo(new Point(x, y)));
        }

    }
}