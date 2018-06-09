using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetCalculator
{
    [TestClass]
    public class BudgetCalculatorTests
    {
        private BudgetService _bugetService;
        private IBudgetRepository _rpt = Substitute.For<IBudgetRepository>();
        private int _actual = 0;

        private void GivenGetBudget(int year, int month, int money)
        {
            _rpt.GetBudget(Arg.Any<int>(), Arg.Any<int>()).Returns(new Budget { Year = year, Month = month, Amount = money });
        }

        private void GivenGetBudgets(IEnumerable<Tuple<int, int, int>> parms)
        {
            _rpt.GetBudgets(Arg.Any<IEnumerable<Tuple<int, int>>>())
                .Returns(
                    parms.Select(o => new Budget() { Year = o.Item1, Month = o.Item2, Amount = o.Item3 }).ToArray()
                );
        }

        private void ActCalculate(DateTime startDate, DateTime endDate)
        {
            _actual = _bugetService.CalculateBudget(startDate, endDate);
        }

        private void AssertResult(int expect)
        {
            Assert.AreEqual(expect, _actual);
        }

        [TestInitialize]
        public void Initializer()
        {
            this._bugetService = new BudgetService(this._rpt);
        }

        [TestMethod]
        public void SingleWholeMonth()
        {
            GivenGetBudget(2018, 6, 300);

            ActCalculate(new DateTime(2018, 6, 1), new DateTime(2018, 6, 30));

            //Assert
            AssertResult(300);
        }

        [TestMethod]
        public void SignleDay()
        {
            GivenGetBudget(2018, 6, 300);

            ActCalculate(new DateTime(2018, 6, 1), new DateTime(2018, 6, 1));

            //Assert
            AssertResult(10);
        }

        [TestMethod]
        public void Search_From_20180601_To_20180730()
        {
            GivenGetBudgets(new List<Tuple<int, int, int>> { Tuple.Create<int, int, int>(2018, 6, 300), Tuple.Create<int, int, int>(2018, 7, 310) });

            ActCalculate(new DateTime(2018, 6, 1), new DateTime(2018, 7, 30));

            AssertResult(300 + 310);
        }

        [TestMethod]
        public void Search_From_20180615_To_20180731()
        {
            GivenGetBudgets(new List<Tuple<int, int, int>> {
                Tuple.Create<int,int,int>(2018,6,300),
                Tuple.Create<int,int,int>(2018,7,310)
            });

            ActCalculate(new DateTime(2018, 6, 15), new DateTime(2018, 7, 31));

            AssertResult(150 + 310);
        }

        [TestMethod]
        public void Search_From_20180601_To_201808_But_201807DoesnotHaveBudget()
        {
            GivenGetBudgets(new List<Tuple<int, int, int>> {
                Tuple.Create<int,int,int>(2018,06,300),
                Tuple.Create<int,int,int>(2018,8,310)
            });

            ActCalculate(new DateTime(2018, 6, 1), new DateTime(2018, 8, 30));

            AssertResult(300 + 310);
        }

        [TestMethod]
        public void Search_From_20180617_To_20180811_But_201807DoesnotHaveBudget()
        {
            GivenGetBudgets(new List<Tuple<int, int, int>> {
                Tuple.Create<int,int,int>(2018,06,300),
                Tuple.Create<int,int,int>(2018,8,310)
            });

            ActCalculate(new DateTime(2018, 6, 17), new DateTime(2018, 8, 11));

            AssertResult(140 + 110);
        }

        [TestMethod]
        public void Search_From_20180711_To_20180830_But_201807DoesnotHaveBudget()
        {
            GivenGetBudgets(new List<Tuple<int, int, int>> {
                Tuple.Create<int,int,int>(2018,8,310)
            });

            ActCalculate(new DateTime(2018, 7, 11), new DateTime(2018, 8, 30));

            AssertResult(310);
        }

        [TestMethod]
        public void Search_From_20180611_To_20180712_But_201807DoesnotHaveBudget()
        {
            GivenGetBudgets(new List<Tuple<int, int, int>> {
                Tuple.Create<int,int,int>(2018,6 ,300)
            });

            ActCalculate(new DateTime(2018, 6, 11), new DateTime(2018, 7, 12));

            AssertResult(190);
        }

        [TestMethod]
        public void Search_From_20180603_To_20200229()
        {
            GivenGetBudgets(new List<Tuple<int, int, int>> {
                Tuple.Create<int,int,int>(2018,6 ,300),
                Tuple.Create<int,int,int>(2020,2,290)
            });

            ActCalculate(new DateTime(2018, 6, 03), new DateTime(2020, 2, 29));

            AssertResult(270 + 290);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void StartDateIsGreaterThanEndDate()
        {
            GivenGetBudgets(new List<Tuple<int, int, int>> {
                Tuple.Create<int,int,int>(2018,6 ,300),
                Tuple.Create<int,int,int>(2020,2,290)
            });

            ActCalculate(new DateTime(2020, 6, 3), new DateTime(2018, 2, 2));
        }

        //private List<Budget> dBugets = new List<Budget>()
        //{
        //    new Budget
        //    {
        //        Year = 2018,
        //        Month = 06,
        //        Money = 300
        //    },
        //    new Budget
        //    {
        //        Year = 2018,
        //        Month = 08,
        //        Money = 310
        //    }
        //};

        //[TestMethod]
        //public void WholeMonth()
        //{
        //    // Arrange

        //    _rpt.GetBudget().Returns(dBugets);

        //    _bugetService = new BudgetService(_rpt);

        //    var expected = 10;

        //    var startDateTime = new DateTime(2018, 6, 1);
        //    var endDateTime = new DateTime(2018, 6, 1);

        //    // Act
        //    var actual = _bugetService.GetBuget(startDateTime, endDateTime);

        //    // Assert
        //    Assert.AreEqual(expected, actual);
        //}

        //[TestMethod]
        //public void WholeMonth2()
        //{
        //    // Arrange

        //    _rpt.GetBudget().Returns(dBugets);

        //    _bugetService = new BudgetService(_rpt);

        //    var expected = 150;

        //    var startDateTime = new DateTime(2018, 6, 1);
        //    var endDateTime = new DateTime(2018, 6, 15);

        //    // Act
        //    var actual = _bugetService.GetBuget(startDateTime, endDateTime);

        //    // Assert
        //    Assert.AreEqual(expected, actual);
        //}

        //[TestMethod]
        //public void test_20180601_20180715()
        //{
        //    // Arrange

        //    _rpt.GetBudget().Returns(dBugets);

        //    _bugetService = new BudgetService(_rpt);

        //    var expected = 450;

        //    var startDateTime = new DateTime(2018, 6, 1);
        //    var endDateTime = new DateTime(2018, 7, 15);

        //    // Act
        //    var actual = _bugetService.GetBuget(startDateTime, endDateTime);

        //    // Assert
        //    Assert.AreEqual(expected, actual);
        //}
    }
}