using CsharpVersionSamples.c12;
using Buffer = CsharpVersionSamples.c12.Buffer;

namespace SampleTest
{
    public class C12Test
    {
        [Fact]
        public void Constructor_Sets_Name_And_Age()
        {
            // Arrange & Act
            var person = new Person("John Doe", 30);

            // Assert
            Assert.Equal("John Doe", person.Name);
            Assert.Equal(30, person.Age);
        }

        [Fact]
        public void CreateNumbersList_ReturnsCorrectList()
        {
            // Arrange
            var collectionExample = new CollectionExample();
            var expectedList = new List<int> { 1, 2, 3, 4, 5 };

            // Act
            var result = collectionExample.CreateNumbersList();

            // Assert
            Assert.Equal(expectedList, result);
        }

        [Fact]
        public void Buffer10_Creation_Test()
        {
            // Arrange
            var bufferUsage = new Buffer10Usage();

            // Act
            var buffer = bufferUsage.CreateBuffer10();

            // Assert
            // Here you would typically assert certain conditions or behaviors specific to your buffer usage
            Assert.Equal(typeof(Buffer10<int>), buffer.GetType());
        }

        [Fact]
        public void Buffer_Creation_Test()
        {
            // Act
            var buffer = new Buffer();
            //for (int i = 0; i < 10; i++)
            //{
            //    buffer[i] = i;
            //}

            //foreach (var i in buffer)
            //{
            //    Console.WriteLine(i);
            //}
        }

        [Fact]
        public void IncrementBy_UsesDefaultValue_WhenNoSecondArgument()
        {
            // Arrange
            var example = new OptionLamdaExample();

            // Act
            var result = example.IncrementBy(3, 1); // increment defaults to 1

            // Assert
            Assert.Equal(4, result); // 3 + 1
        }

        [Fact]
        public void IncrementBy_UsesProvidedValue_WhenSecondArgumentGiven()
        {
            // Arrange
            var example = new OptionLamdaExample();

            // Act
            var result = example.IncrementBy(3, 2);

            // Assert
            Assert.Equal(5, result); // 3 + 2
        }

        [Fact]
        public void FindMax_ReturnsMaximumValue()
        {
            // Arrange
            var arrayUtilities = new CalculatorRefReadOnly();
            var numbers = new int[] { 3, 6, 2, 8, 4 };

            // Act
            ref readonly int maxValue = ref arrayUtilities.FindMax(ref numbers);

            // Assert
            Assert.Equal(8, maxValue);
        }

        [Fact]
        public void FindMax_ThrowsExceptionForNullOrEmptyArray()
        {
            // Arrange
            var arrayUtilities = new CalculatorRefReadOnly();
            int[] numbers = null;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => arrayUtilities.FindMax(ref numbers));
        }

        [Fact]
        public void GetNames_ReturnsListOfNames()
        {
            // Arrange
            var example = new AliasExample();

            // Act
            var names = example.GetNames();

            // Assert
            Assert.NotNull(names);
            Assert.Equal(3, names.Count);
            Assert.Contains("Alice", names);
            Assert.Contains("Bob", names);
            Assert.Contains("Charlie", names);
        }

        [Fact]
        public void ExperimentalMethod_DoesNotThrowException()
        {
            // Arrange
            var experimentalClass = new ExperimentalClass();

            // Act & Assert
            var exception = Record.Exception(() => experimentalClass.ExperimentalMethod());
            Assert.Null(exception);
        }
    }
}