using ImageChecker_3.Images;
using NUnit.Framework;

namespace ImageChecker_3Test.Models.Images
{
    [TestFixture]
    public class SlideControllerTest
    {
        private SlideController slideController;

        [SetUp]
        public void Setup()
        {
            slideController = new SlideController(null);
        }

        /// <summary>
        /// 増加テスト：90 増加させたとき、Degree が正しく加算されるか確認します。
        /// </summary>
        [Test]
        public void ChangeDegreeCommand_IncreasesDegreeWithin360()
        {
            // Arrange
            var initialDegree = slideController.Degree;
            const string increment = "90";

            // Act
            slideController.ChangeDegreeCommand.Execute(increment);

            // Assert
            Assert.AreEqual(initialDegree + 90, slideController.Degree);
        }

        /// <summary>
        /// 360 超えのテスト：350 から 20 増加させたとき、Degree がリセットされて 10 になるか確認します。
        /// </summary>
        [Test]
        public void ChangeDegreeCommand_ResetsDegreeAfter360()
        {
            // Arrange
            slideController.Degree = 350;
            const string increment = "20";

            // Act
            slideController.ChangeDegreeCommand.Execute(increment);

            // Assert
            Assert.AreEqual(10, slideController.Degree);
        }

        /// <summary>
        /// 負数のテスト：10 から -20 減少させたとき、Degree が 350 になるか確認します。
        /// </summary>
        [Test]
        public void ChangeDegreeCommand_NegativeDegreeWrapsTo360()
        {
            // Arrange
            slideController.Degree = 10;
            const string decrement = "-20";

            // Act
            slideController.ChangeDegreeCommand.Execute(decrement);

            // Assert
            Assert.AreEqual(350, slideController.Degree);
        }

        /// <summary>
        /// null チェック：null パラメータを与えたときに Degree が変化しないことを確認します。
        /// </summary>
        [Test]
        public void ChangeDegreeCommand_DoesNothingOnNullParameter()
        {
            // Arrange
            var initialDegree = slideController.Degree;

            // Act
            slideController.ChangeDegreeCommand.Execute(null);

            // Assert
            Assert.AreEqual(initialDegree, slideController.Degree);
        }
    }
}