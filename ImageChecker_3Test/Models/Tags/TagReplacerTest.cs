using ImageChecker_3.Images;
using ImageChecker_3.Tags;
using NUnit.Framework;

namespace ImageChecker_3Test.Models.Tags
{
    [TestFixture]
    public class TagReplacerTest
    {
        [Test]
        public void ReplaceImageNamesTest()
        {
            var tag =
                @"<anime name=""animationChain"">" +
                @"<anime name=""image"" a=""A1111"" b=""B1111"" c=""C1111"" d=""D1111"" />" +
                @"<image name=""image"" a=""A1111"" b=""B1111"" c=""C1111"" d=""D1111"" />" +
                "<anime />";

            var expected =
                @"<anime name=""animationChain"">" +
                @"<anime name=""image"" a=""A0101"" b=""B0101"" c=""C0101"" d=""D0101"" />" +
                @"<image name=""image"" a=""A0101"" b=""B0101"" c=""C0101"" d=""D0101"" />" +
                "<anime />";

            var container = new PreviewContainer();
            container.SetImageWrappers(
                    new (new ImageFileInfo(@"C:\MyFiles\temp\pngs\A0101.png") { Width = 1280, Height = 720, }),
                    new (new ImageFileInfo(@"C:\MyFiles\temp\pngs\B0101.png") { Width = 1280, Height = 720, }),
                    new (new ImageFileInfo(@"C:\MyFiles\temp\pngs\C0101.png") { Width = 1280, Height = 720, }),
                    new (new ImageFileInfo(@"C:\MyFiles\temp\pngs\D0101.png") { Width = 1280, Height = 720, })
                );

            var actual = TagReplacer.ReplaceImageNames(tag, container, false);
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}