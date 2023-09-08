using Bloc3_CSharp.Services.concretServices;

namespace TestProject_Mercadona
{
    public class CheckStringDateServiceTest
    {
        CheckStringDateService checkStringDateServiceTest = new CheckStringDateService();
        static string dateBeforeAll = "1900-01-01";
        static string dateAfterAll = "5999-12-31";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestDateIsAfterNow_With_DateBeforeAll ()
        {
            Assert.False(checkStringDateServiceTest.DateIsAfterNow(dateBeforeAll));
        }

        [Test]
        public void TestDateIsAfterNow_With_dateAfterAll()
        {
            Assert.True(checkStringDateServiceTest.DateIsAfterNow(dateAfterAll));
        }

        [Test]
        public void TestDateIsBeforeNow_With_DateBeforeAll()
        {
            Assert.True(checkStringDateServiceTest.DateIsBeforeNow(dateBeforeAll));
        }

        [Test]
        public void TestDateIsBeforeNow_With_DateAfterAll()
        {
            Assert.False(checkStringDateServiceTest.DateIsBeforeNow(dateAfterAll));
        }
        [Test]
        public void TestDateIsBeforeOrEqualNow_With_DateBeforeAll()
        {
            Assert.True(checkStringDateServiceTest.DateIsBeforeOrEqualNow(dateBeforeAll));
        }

        [Test]
        public void TestDateIsBeforeOrEqualNow_With_DateAfterAll()
        {
            Assert.False(checkStringDateServiceTest.DateIsBeforeOrEqualNow(dateAfterAll));
        }
        [Test]
        public void TestDateIsBeforeOrEqualNow_With_DateNow()
        {
            Assert.True(checkStringDateServiceTest.DateIsBeforeOrEqualNow(DateTime.Now.ToShortDateString()));
        }
        [Test]
        public void TestDateIsAfterOrEqualNow_With_DateBeforeAll()
        {
            Assert.False(checkStringDateServiceTest.DateIsAfterOrEqualNow(dateBeforeAll));
        }

        [Test]
        public void TestDateIsAfterOrEqualNow_With_DateAfterAll()
        {
            Assert.True(checkStringDateServiceTest.DateIsAfterOrEqualNow(dateAfterAll));
        }
        [Test]
        public void TestDateIsAfterOrEqualNow_With_DateNow()
        {
            Assert.True(checkStringDateServiceTest.DateIsAfterOrEqualNow(DateTime.Now.ToShortDateString()));
        }

        [Test]
        public void TestDateOneIsAfterDateTwo_With_WrongDates()
        {
            Assert.False(checkStringDateServiceTest.DateOneIsAfterDateTwo(dateBeforeAll, dateAfterAll));
        }
        [Test]
        public void TestDateOneIsAfterDateTwo_With_GoodDates()
        {
            Assert.True(checkStringDateServiceTest.DateOneIsAfterDateTwo(dateAfterAll, dateBeforeAll));
        }

        [Test]
        public void TestDateOneIsAfterDateTwo_With_SameDates()
        {
            Assert.False(checkStringDateServiceTest.DateOneIsAfterDateTwo(dateAfterAll, dateAfterAll));
        }
    }
}
