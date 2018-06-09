using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace BudgetCalculator
{
    [TestClass]
    public class BudgetCalculatorTests
    {
        private BugetService _bugetService;
        private IRespository _respository = Substitute.For<IRespository>();

        private List<Buget> dBugets = new List<Buget>()
        {
            new Buget
            {
                Year = 2018,
                Month = 06,
                Money = 300
            },
            new Buget
            {
                Year = 2018,
                Month = 08,
                Money = 310
            }
        };

        [TestMethod]
        public void WholeMonth()
        {
            // Arrange

            _respository.GetBudget().Returns(dBugets);

            _bugetService = new BugetService(_respository);

            var expected = 10;

            var startDateTime = new DateTime(2018, 6, 1);
            var endDateTime = new DateTime(2018, 6, 1);

            // Act
            var actual = _bugetService.GetBuget(startDateTime, endDateTime);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WholeMonth2()
        {
            // Arrange

            _respository.GetBudget().Returns(dBugets);

            _bugetService = new BugetService(_respository);

            var expected = 150;

            var startDateTime = new DateTime(2018, 6, 1);
            var endDateTime = new DateTime(2018, 6, 15);

            // Act
            var actual = _bugetService.GetBuget(startDateTime, endDateTime);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void test_20180601_20180715()
        {
            // Arrange

            _respository.GetBudget().Returns(dBugets);

            _bugetService = new BugetService(_respository);

            var expected = 450;

            var startDateTime = new DateTime(2018, 6, 1);
            var endDateTime = new DateTime(2018, 7, 15);

            // Act
            var actual = _bugetService.GetBuget(startDateTime, endDateTime);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }

    public interface IRespository
    {
        List<Buget> GetBudget();
    }
}