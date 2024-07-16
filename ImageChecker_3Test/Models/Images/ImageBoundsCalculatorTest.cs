using System;
using System.IO;
using System.Windows;
using ImageChecker_3.Models.Images;
using NUnit.Framework;

namespace ImageChecker_3Test.Models.Images
{
    [TestFixture]
    public class ImageBoundsCalculatorTest
    {
        [Test]
        public void GetOpaquePixelBounds_AllOpaquePixels_ReturnsFullImage()
        {
            var imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "partially_opaque_image.png");

            // partially_opaque_image はサイズ 4x4 で、透明部分(0)と不透明部分(1)が以下のように配置された画像です。

            // 0000
            // 0110
            // 0110
            // 0000

            var expected = new Int32Rect(1, 1, 2, 2); // Assuming the image is 100x100

            var actual = ImageBoundsCalculator.GetOpaquePixelBounds(imagePath);

            Assert.AreEqual(expected, actual);
        }
    }
}