using System;
using ImageChecker_3.Models;
using NUnit.Framework;

namespace ImageChecker_3Test.Models
{
    [TestFixture]
    public class TagParserTest
    {
        [Test]
        public void LoadImageTagTest()
        {
            const string input = @"<image a=""imgA"" b=""imgB"" c=""imgC"" d=""imgD"" x=""100"" y=""200"" scale=""1.1"" />";
            var actual = TagParser.LoadImageTag(input);
            Assert.That(actual.X, Is.EqualTo(100));
            Assert.That(actual.Y, Is.EqualTo(200));
            Assert.That(actual.A, Is.EqualTo("imgA"));
            Assert.That(actual.B, Is.EqualTo("imgB"));
            Assert.That(actual.C, Is.EqualTo("imgC"));
            Assert.That(actual.D, Is.EqualTo("imgD"));
            Assert.Less(Math.Abs(actual.Scale - 1.1), 0.01);
        }

        [Test]
        public void LoadImageTagTest_要素抜けあり()
        {
            const string input = @"<image a=""imgA"" b=""imgB"" c=""imgC"" y=""200"" scale=""1.1"" />";
            var actual = TagParser.LoadImageTag(input);
            Assert.That(actual.X, Is.EqualTo(0));
            Assert.That(actual.Y, Is.EqualTo(200));
            Assert.That(actual.A, Is.EqualTo("imgA"));
            Assert.That(actual.B, Is.EqualTo("imgB"));
            Assert.That(actual.C, Is.EqualTo("imgC"));
            Assert.That(actual.D, Is.EqualTo(string.Empty));
            Assert.Less(Math.Abs(actual.Scale - 1.1), 0.01);
        }
    }
}