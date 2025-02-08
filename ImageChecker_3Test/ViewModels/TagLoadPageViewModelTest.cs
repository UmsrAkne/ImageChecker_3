using ImageChecker_3.ViewModels;
using NUnit.Framework;

namespace ImageChecker_3Test.ViewModels
{
    [TestFixture]
    public class TagLoadPageViewModelTest
    {
        [TestCase(
            @"<image a=""A0101"" b=""B0102"" c=""C0102"" d=""D0102"" x=""100"" y=""200"" />",
        @"<image a=""A0101"" b=""B0102"" c=""C0102"" d=""D0102"" x=""100"" y=""200"" scale=""1"" targetLayerIndex=""0"" />")]
        [TestCase(
            @"<draw a=""A0101"" b=""B0102"" c=""C0102"" d=""D0102"" />",
        @"<draw a=""A0101"" b=""B0102"" c=""C0102"" d=""D0102"" targetLayerIndex=""0"" />")]
        [TestCase(
            @"<anime name=""image"" a=""A0101"" b=""B0102"" c=""C0102"" d=""D0102"" x=""100"" y=""200"" scale=""1"" />",
        @"<anime name=""image"" a=""A0101"" b=""B0102"" c=""C0102"" d=""D0102"" x=""100"" y=""200"" scale=""1"" targetLayerIndex=""0"" />")]
        [TestCase(
            @"<anime name=""draw"" a=""A0101"" b=""B0102"" c=""C0102"" d=""D0102"" />",
        @"<anime name=""draw"" a=""A0101"" b=""B0102"" c=""C0102"" d=""D0102"" targetLayerIndex=""0"" />")]
        [Test]
        public void ParseTagTextCommandTest(string input, string expected)
        {
            var vm = new TagLoadPageViewModel
            {
                InputText = input,
            };

            vm.ParseTagTextCommand.Execute();
            Assert.That(vm.ParsedResult, Is.EqualTo(expected));
        }
    }
}