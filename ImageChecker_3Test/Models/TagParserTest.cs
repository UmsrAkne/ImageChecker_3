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

        [Test]
        public void LoadDrawTagTest()
        {
            const string input = @"<draw a=""imgA"" b=""imgB"" c=""imgC"" d=""imgD"" />";
            var actual = TagParser.LoadDrawTag(input);
            Assert.That(actual.A, Is.EqualTo("imgA"));
            Assert.That(actual.B, Is.EqualTo("imgB"));
            Assert.That(actual.C, Is.EqualTo("imgC"));
            Assert.That(actual.D, Is.EqualTo("imgD"));
        }

        [Test]
        public void LoadSlideTagTest()
        {
            const string input = @"<slide degree=""10"" duration=""20"" distance=""30"" repeatCount=""5"" targetLayerIndex=""2""/>";
            var actual = TagParser.LoadSlideTag(input);
            Assert.That(actual.Degree, Is.EqualTo(10));
            Assert.That(actual.Duration, Is.EqualTo(20));
            Assert.That(actual.Distance, Is.EqualTo(30));
            Assert.That(actual.RepeatCount, Is.EqualTo(5));
            Assert.That(actual.TargetLayerIndex, Is.EqualTo(2));
        }
    }
}