using Bloc3_CSharp.Data;
using Bloc3_CSharp.Models;
using Bloc3_CSharp.Services.abstractServices;

namespace Bloc3_CSharp.Services.concretServices
{
    public class CreateArticleService : ICreateArticleService
    {
        private readonly ICheckStringDateService _checkStringDateService;
        public CreateArticleService(ICheckStringDateService checkStringDateService) 
        {
            _checkStringDateService = checkStringDateService;
        }

        public Articles CreateArticle(Product p, ApplicationDbContext context)
        {
            Articles newArticle = new Articles();
            newArticle.Id = p.Id;
            newArticle.Label = p.Label;
            newArticle.Description = p.Description;
            newArticle.PictureName = p.PictureName;
            newArticle.CategoryId = p.CategoryId;
            newArticle.CategoryName = p.Category.Name;
            newArticle.BasePrice = p.Price;
            newArticle.DiscountId = p.DiscountId;
            if (!(newArticle.DiscountId == 0)
                && !(context.Discounts.Find(newArticle.DiscountId) == null) 
                && CheckValidityDiscount(context.Discounts.Find(newArticle.DiscountId)))
            {
                newArticle.DiscountValue = context.Discounts.Find(newArticle.DiscountId).Value;
                newArticle.OnDateDiscount = context.Discounts.Find(newArticle.DiscountId).OnDate;
                newArticle.OffDateDiscount = context.Discounts.Find(newArticle.DiscountId).OffDate;
                
            }
            else
            {
                newArticle.DiscountValue = 0;
                newArticle.OnDateDiscount = "";
                newArticle.OffDateDiscount = "";
            }
            newArticle.Price = SetPrice(newArticle.BasePrice, newArticle.DiscountValue);
            return newArticle;
        }
        public bool CheckValidityDiscount(Discount discountToCheck)
        {
            bool dateOnIsGood = _checkStringDateService.DateIsBeforeOrEqualNow(discountToCheck.OnDate);
            bool dateOffIsGood = _checkStringDateService.DateIsAfterOrEqualNow(discountToCheck.OffDate);
            return dateOnIsGood && dateOffIsGood;
        }

        public decimal SetPrice(decimal basePrice, int discountValue)
        {
            decimal price = Math.Round(basePrice * (100 - discountValue)) / 100.0M;
            if (price > 0)
            {
                return price;
            }
            else
            {
                return 0;
            }
        }
    }
}
