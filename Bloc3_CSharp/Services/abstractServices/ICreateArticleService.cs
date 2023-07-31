using Bloc3_CSharp.Data;
using Bloc3_CSharp.Models;

namespace Bloc3_CSharp.Services.abstractServices
{
    public interface ICreateArticleService
    {
        public Articles CreateArticle(Product p, ApplicationDbContext context);
        public bool CheckValidityDiscount(Discount discountToCheck);
        public decimal SetPrice(decimal basePrice, int discountValue);
    }
}
