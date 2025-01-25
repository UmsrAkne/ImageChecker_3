using System.Windows;
using ImageChecker_3.Models;
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
            previewContainer = new PreviewContainer
            {
                ScreenRect =
                {
                    Width = 1280,
                },
                Scale = 1.0,
            };

            previewContainer.SetImageWrappers(
                new ImageWrapper(new ImageFileInfo(){Width = 1280, Height = 720,}),
                new ImageWrapper(new ImageFileInfo(){Width = 1280, Height = 720,}),
                new ImageWrapper(new ImageFileInfo(){Width = 1280, Height = 720,}),
                new ImageWrapper(new ImageFileInfo(){Width = 1280, Height = 720,})
                );
        }

        [Test]
        [TestCase(1.0, 100, -200)]
        [TestCase(1.5, 420, -380)]
        [TestCase(2.0, 740, -560)]
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
        [TestCase(1.0, 0, -200)]
        [TestCase(1.5, 320, -380)]
        [TestCase(2.0, 640, -560)]
        public void RelativePositionTest_画面サイズ1480(double scale, double x, double y)
        {
            previewContainer.Scale = scale;
            previewContainer.ScreenRect = new BindableRect(0, 0, 1480, 720);
            Assert.That(new Point(previewContainer.X, previewContainer.Y), Is.EqualTo(new Point(0, 0)));

            previewContainer.X = 100;
            previewContainer.Y = 200;

            var actual = previewContainer.RelativePosition;
            Assert.That(actual, Is.EqualTo(new Point(x, y)));
        }

        [Test]
        [TestCase(100, 200, 100, -200)]
        [TestCase(-100, -200, -100, 200)]
        [TestCase(0, 0, 0, 0)]
        public void RelativePositionSetterTest(double x, double y, double exceptX, double exceptY)
        {
            previewContainer.ScreenRect = new BindableRect(0, 0, 1280, 720);
            Assert.That(new Point(previewContainer.X, previewContainer.Y), Is.EqualTo(new Point(0, 0)));

            previewContainer.RelativePosition = new Point(x, y);

            var actual = previewContainer.RelativePosition;
            Assert.That(actual, Is.EqualTo(new Point(exceptX, exceptY)));
        }

        [Test]
        [TestCase(100, 200, 100, -200)]
        [TestCase(-100, -200, -100, 200)]
        [TestCase(0, 0, 0, 0)]
        public void RelativePositionSetterTest_Scale2(double x, double y, double exceptX, double exceptY)
        {
            previewContainer.Scale = 2.0;
            previewContainer.ScreenRect = new BindableRect(0, 0, 1280, 720);
            Assert.That(new Point(previewContainer.X, previewContainer.Y), Is.EqualTo(new Point(0, 0)));

            previewContainer.RelativePosition = new Point(x, y);

            var actual = previewContainer.RelativePosition;
            Assert.That(actual, Is.EqualTo(new Point(exceptX, exceptY)));
        }
    }
}