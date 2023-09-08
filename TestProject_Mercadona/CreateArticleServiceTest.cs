using Bloc3_CSharp.Models;
using Bloc3_CSharp.Services.concretServices;

namespace TestProject_Mercadona
{
    public class CreateArticleServiceTest
    {
        
        CreateArticleService createArticleServiceTest;
        CheckStringDateService checkStringDateServiceTest = new CheckStringDateService();
        static decimal BASE_PRICE = 100.0M;

        [SetUp]
        public void Setup()
        {
            //var checkStringDateServiceMock = Mock.Of<ICheckStringDateService>();
            createArticleServiceTest = new CreateArticleService(checkStringDateServiceTest);
        }

        [Test]
        public void TestSetPrice_With_discountValue_50()
        {
            Assert.AreEqual(50,createArticleServiceTest.SetPrice(BASE_PRICE, 50));
        }
        [Test]
        public void TestSetPrice_With_discountValue_Under0()
        {
            Assert.AreEqual(100, createArticleServiceTest.SetPrice(BASE_PRICE, -5));
        }
        [Test]
        public void TestSetPrice_With_discountValue_Greater100()
        {
            Assert.AreEqual(100, createArticleServiceTest.SetPrice(BASE_PRICE, 120));
        }

        [Test]
        public void TestCheckValidityDiscount_With_ValideDiscount()
        {
            Discount valideDiscount = new Discount(2,"1900-01-01","5999-12-31",50);
            Assert.True(createArticleServiceTest.CheckValidityDiscount(valideDiscount));
        }

        [Test]
        public void TestCheckValidityDiscount_With_DiscountOnDateAfterNow()
        {
            Discount wrongDiscount = new Discount(2, DateTime.Now.AddYears(1).ToShortDateString(), "5999-12-31", 50);
            Assert.False(createArticleServiceTest.CheckValidityDiscount(wrongDiscount));
        }

        [Test]
        public void TestCheckValidityDiscount_With_DiscountOffDatebeforeNow()
        {
            Discount wrongDiscount = new Discount(2,"1900-01-01", DateTime.Now.AddYears(-1).ToShortDateString(), 50);
            Assert.False(createArticleServiceTest.CheckValidityDiscount(wrongDiscount));
        }
        // code a remanié : Discount envoyé en parametre
        [Test]
        public void TestCreateArticle()
        {
            Product productTest = new Product(1,"Label","Description",100.0M,1,"Picture",1);
            Discount discountTest = new Discount(1, "1900-01-01", "5999-12-31", 50);
            //Assert.IsNotNull(createArticleServiceTest.CreateArticle(productTest, discountTest));

        }

    }
}
