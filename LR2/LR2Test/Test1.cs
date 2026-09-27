using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using LR2;

namespace LR2Test
{
    [TestClass]
    public class Task1Test
    {
        [TestMethod]
        public void Calculate_All_ReturnsSumOfCubes()
        {
            // Arrange: 7^3 = 343, 14^3 = 2744, 21^3 = 9261; Сума = 12348
            var calc = new Task1(7, 14, 21);
            long expected = 12348;

            // Act
            long result = calc.CalculateSumOfCubes();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Calculate_None_ReturnsZero()
        {
            // Arrange
            var calc = new Task1(1, 2, 3);
            long expected = 0;

            // Act
            long result = calc.CalculateSumOfCubes();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Calculate_Some_ReturnsSumOfTheirCubes()
        {
            // Arrange: тільки 7 і 14 кратні 7; 7^3 + 14^3 = 343 + 2744 = 3087
            var calc = new Task1(7, 10, 14);
            long expected = 3087;

            // Act
            long result = calc.CalculateSumOfCubes();

            // Assert
            Assert.AreEqual(expected, result);
        }
    }

    [TestClass]
    public class Task2Test
    {
        [TestMethod]
        public void Calculate_A_GreaterThan_B_ThrowsException()
        {
            // Arrange
            var calc = new Task2(20, 10);

            // Act & Assert
            try
            {
                calc.CalculateSum();
                Assert.Fail("Expected ArgumentOutOfRangeException was not thrown.");
            }
            catch (ArgumentOutOfRangeException)
            {
                // Test passed
            }
        }

        [TestMethod]
        public void Calculate_ValidRange_ReturnsCorrectSum()
        {
            // 77 % 11 == 0, 77 % 8 = 5;
            var calc = new Task2(1, 100);
            long expected = 77;

            // Act
            long result = calc.CalculateSum();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Calculate_NoMatchingNumbers_ReturnsZero()
        {
            // Arrange
            var calc = new Task2(1, 20);
            long expected = 0;

            // Act
            long result = calc.CalculateSum();

            // Assert
            Assert.AreEqual(expected, result);
        }
    }

    [TestClass]
    public class TriangleTest
    {
        [TestMethod]
        public void CalculateArea_RightTriangle3_4_5_ReturnsSix()
        {
            // Arrange
            var triangle = new Triangle(3, 4, 5);
            double expected = 6.0;

            // Act
            double result = triangle.CalculateArea();

            // Assert
            Assert.AreEqual(expected, result, 1e-5);
        }

        [TestMethod]
        public void GetTriangleType_RightTriangle_ReturnsRight()
        {
            // Arrange: 3^2 + 4^2 = 5^2
            var triangle = new Triangle(3, 4, 5);

            // Act
            string type = triangle.GetTriangleType();

            // Assert
            Assert.AreEqual("Прямокутний", type);
        }

        [TestMethod]
        public void GetTriangleType_AcuteTriangle_ReturnsAcute()
        {
            // Arrange: 5, 5, 5 (гострокутний)
            var triangle = new Triangle(5, 5, 5);

            // Act
            string type = triangle.GetTriangleType();

            // Assert
            Assert.AreEqual("Гострокутний", type);
        }

        [TestMethod]
        public void GetTriangleType_ObtuseTriangle_ReturnsObtuse()
        {
            // Arrange: 3, 4, 6 (3^2 + 4^2 = 25 < 36) -> тупокутний
            var triangle = new Triangle(3, 4, 6);

            // Act
            string type = triangle.GetTriangleType();

            // Assert
            Assert.AreEqual("Тупокутний", type);
        }

        [TestMethod]
        public void Constructor_InvalidSides_ThrowsArgumentException()
        {
            // Неіснуючий трикутник: 1 + 2 <= 5
            try
            {
                new Triangle(1, 2, 5);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
                // Test passed
            }
        }
    }
}