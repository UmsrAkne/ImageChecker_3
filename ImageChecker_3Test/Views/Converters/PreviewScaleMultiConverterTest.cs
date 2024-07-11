using System;
using System.Globalization;
using System.Windows.Data;
using ImageChecker_3.Views.Converters;
using NUnit.Framework;

namespace ImageChecker_3Test.Views.Converters
{
    public class PreviewScaleMultiConverterTest
    {
        private PreviewScaleMultiConverter converter;

        [SetUp]
        public void SetUp()
        {
            converter = new PreviewScaleMultiConverter();
        }

        [Test]
        public void Convert_NoValues_ReturnsBindingDoNothing()
        {
            // Arrange
            var values = Array.Empty<object>();

            // Act
            var result = converter.Convert(values, typeof(double), null, CultureInfo.InvariantCulture);

            // Assert
            Assert.AreEqual(Binding.DoNothing, result);
        }

        [Test]
        public void Convert_OneValue_ReturnsBindingDoNothing()
        {
            // Arrange
            var values = new object[] { 2.0, };

            // Act
            var result = converter.Convert(values, typeof(double), null, CultureInfo.InvariantCulture);

            // Assert
            Assert.AreEqual(2.0, result);
        }

        [Test]
        public void Convert_TwoValues_ReturnsProduct()
        {
            // Arrange
            var values = new object[] { 3.0, 4.0, };

            // Act
            var result = converter.Convert(values, typeof(double), null, CultureInfo.InvariantCulture);

            // Assert
            Assert.AreEqual(12.0, result);
        }

        [Test]
        public void Convert_ThreeValues_ReturnsProduct()
        {
            // Arrange
            var values = new object[] { 2.0, 3.0, 4.0, };

            // Act
            var result = converter.Convert(values, typeof(double), null, CultureInfo.InvariantCulture);

            // Assert
            Assert.AreEqual(24.0, result);
        }

        [Test]
        public void Convert_MixedValuesWithNonDoubles_IgnoresNonDoubleValues()
        {
            // Arrange
            var values = new object[] { 2.0, "test", 3.0, null, 4.0, };

            // Act
            var result = converter.Convert(values, typeof(double), null, CultureInfo.InvariantCulture);

            // Assert
            Assert.AreEqual(24.0, result);
        }

        [Test]
        public void Convert_NonNumericValues_ReturnsBindingDoNothing()
        {
            // Arrange
            var values = new object[] { "test", null, };

            // Act
            var result = converter.Convert(values, typeof(double), null, CultureInfo.InvariantCulture);

            // Assert
            Assert.AreEqual(Binding.DoNothing, result);
        }
    }
}