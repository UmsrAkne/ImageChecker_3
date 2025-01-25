using System.IO;
using System.Windows;
using ImageChecker_3.Models;
using ImageChecker_3.Models.Images;
using NUnit.Framework;

namespace ImageChecker_3Test.Models
{
    [TestFixture]
    public class TagGeneratorTest
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
            };

            previewContainer.SetImageWrappers(
            new ImageWrapper( new ImageFileInfo() { FileInfo = new FileInfo("A0101"), Width = 1280, Height = 720, }),
            new ImageWrapper( new ImageFileInfo() { FileInfo = new FileInfo("B0101"), Width = 1280, Height = 720, }),
            new ImageWrapper( new ImageFileInfo() { FileInfo = new FileInfo("C0101"), Width = 1280, Height = 720, }),
            new ImageWrapper( new ImageFileInfo() { FileInfo = new FileInfo("D0101"), Width = 1280, Height = 720, }));
        }

        [Test]
        public void GetTagTest()
        {
            previewContainer.X = 10;
            previewContainer.Y = 20;

            var tagText = @"<image a=""$a"" b=""$b"" c=""$c"" d=""$d"" x=""$x"" y=""$y"" scale=""$scale"" />";
            var actual = TagGenerator.GetTag(tagText, previewContainer, false);
            var expect = @"<image a=""A0101"" b=""B0101"" c=""C0101"" d=""D0101"" x=""10"" y=""-20"" scale=""1.0"" />";

            Assert.That(actual, Is.EqualTo(expect));
        }

        [Test]
        public void GetTagTest_EmptyValues()
        {
            var container = new PreviewContainer();
            container.RelativePosition = new Point();
            var tagText = @"<image a=""$a"" b=""$b"" c=""$c"" d=""$d"" x=""$x"" y=""$y"" scale=""$scale"" />";
            var actual = TagGenerator.GetTag(tagText, container, false);
            var expect = @"<image a="""" b="""" c="""" d="""" x=""0"" y=""0"" scale=""1.0"" />";

            Assert.That(actual, Is.EqualTo(expect));
        }
    }
}