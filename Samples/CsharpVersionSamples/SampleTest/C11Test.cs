using CsharpVersionSamples.c11;
using CsharpVersionSamples.c12;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleTest
{
    public class C11Test
    {
        [Fact]
        public void Attribute_ShouldHaveCorrectValue()
        {
            // Act
            var attribute = (GenericAttribute<string>)Attribute.GetCustomAttribute(typeof(GenericStringType), typeof(GenericAttribute<string>));

            // Assert
            Assert.NotNull(attribute);
            Assert.Equal("Example", attribute.Value);
        }

        [Fact]
        public void Add_ReturnsCorrectSumForInt()
        {
            // Arrange
            var operations = new MathOperations<int>();

            // Act
            int result = operations.Add(2, 3);

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void Add_ReturnsCorrectSumForDouble()
        {
            // Arrange
            var operations = new MathOperations<double>();

            // Act
            double result = operations.Add(2.5, 3.5);

            // Assert
            Assert.Equal(6.0, result);
        }

        [Fact]
        public void FormatJsonString_ReturnsCorrectJson()
        {
            // Arrange
            var handler = new StringHandler();
            string expectedJson = """
                                  {
                                      "name": "John Doe",
                                      "age": 30,
                                      "isEmployee": true
                                  }
                                  """;

            // Act
            string actualJson = handler.FormatJsonString();

            // Assert
            Assert.Equal(expectedJson, actualJson);
        }

        [Fact]
        public void FormatWelcomeMessage_ReturnsCorrectFormattedMessage()
        {
            // Arrange
            var formatter = new StringHandler();
            string name = "Alice";
            DateTime date = new DateTime(2023, 1, 1);
            string expectedMessage = """
                                     Hello, Alice.
                                     Welcome on January 01, 2023.
                                     Enjoy your stay!
                                     """;

            // Act
            string message = formatter.FormatWelcomeMessage(name, date);

            // Assert
            Assert.Equal(expectedMessage, message);
        }

        [Fact]
        public void StartsWithOneTwoThree_ReturnsTrueForMatchingList()
        {
            // Arrange
            var checker = new ListPatternChecker();
            var numbers = new List<int> { 1, 2, 3, 4, 5 };

            // Act
            bool result = checker.StartsWithOneTwoThree(numbers);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void StartsWithOneTwoThree_ReturnsFalseForNonMatchingList()
        {
            // Arrange
            var checker = new ListPatternChecker();
            var numbers = new List<int> { 4, 5, 6 };

            // Act
            bool result = checker.StartsWithOneTwoThree(numbers);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void UseFileLocalClass_ReturnsCorrectMessage()
        {
            // Arrange
            var publicClass = new PublicClass();

            // Act
            var message = publicClass.UseFileLocalClass();

            // Assert
            Assert.Equal("Hello from a file-local class!", message);
        }

        [Fact]
        public void Person_RequiresNameAndAge()
        {
            // This test demonstrates that Name and Age are required fields.
            // The test will fail to compile if Name or Age are not set.

            var person = new PersonRequiredMember { Name = "Bob", Age = 25 };

            Assert.Equal("Bob", person.Name);
            Assert.Equal(25, person.Age);
        }

        [Fact]
        public void Point_AutoDefaultsToZero()
        {
            PointAutodefaultStructs p=new(); // No need to explicitly initialize

            Assert.Equal(0, p.X);
            Assert.Equal(0, p.Y);
        }

        [Fact]
        public void GetNameOfNestedClass_ReturnsCorrectName()
        {
            var example = new NameScopeExample();

            string name = example.GetNameOfNestedClass();

            Assert.Equal("NestedClass", name);
        }

        [Fact]
        public void AddIntPtrs_ReturnsCorrectSum()
        {
            var calculator = new IntPtrArithmetic();
            var ptr1 = new IntPtr(10);
            var ptr2 = new IntPtr(20);

            var result = calculator.AddIntPtrs(ptr1, ptr2);

            Assert.Equal(new IntPtr(30), result);
        }
        [Theory]
        [InlineData(20, 10, 30)] // Test case with expected result 30 for inputs 20 and 10
        [InlineData(15, 5, 20)]  // Test case with expected result 20 for inputs 15 and 5
        [InlineData(0, 0, 0)]    // Test case with expected result 0 for inputs 0 and 0
        public void Calculate_ShouldReturnCorrectSum(int x, int y, int expected)
        {
            // Act
            int result = MethodGroupConversionExample.Calculate(x, y, MethodGroupConversionExample.Add);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Alice", "Hello, Alice")]
        [InlineData("", null)] // This case would be a concern due to the new warning
        [InlineData(null, null)] // This case too
        public void GetGreeting_ReturnsCorrectResult(string input, string expected)
        {
            var example = new WarningWaveExample();

            var result = example.GetGreeting(input);

            Assert.Equal(expected, result);
        }
    }
}
