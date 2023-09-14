using Bloc3_CSharp.Data;
using Bloc3_CSharp.Models;
using Bloc3_CSharp.Services.abstractServices;
using Microsoft.EntityFrameworkCore;

namespace Bloc3_CSharp.Services.concretServices
{
    public class CreateArticleService : ICreateArticleService
    {
        private readonly ICheckStringDateService _checkStringDateService;
        private readonly ApplicationDbContext _context;
        public CreateArticleService(ICheckStringDateService checkStringDateService, ApplicationDbContext context) 
        {
            _checkStringDateService = checkStringDateService;
            _context = context;
        }

        public List<Articles> CreateArticlesList(List<Product> productsList) 
        {
            List<Articles> articlesList = new List<Articles>();
            foreach (var p in productsList)
            {
                Discount productDiscount;

                if (!(_context.Discounts.Find(p.DiscountId) == null))
                {
                    productDiscount = _context.Discounts.Find(p.DiscountId);
                }
                else
                {
                    productDiscount = new Discount();
                }
                articlesList.Add(CreateArticle(p, productDiscount));
            }
            return articlesList;
        }

        public Articles CreateArticle(Product p, Discount productDiscount)
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
            if (!(productDiscount.Id == 0) 
                && CheckValidityDiscount(productDiscount))
            {
                newArticle.DiscountValue = productDiscount.Value;
                newArticle.OnDateDiscount = productDiscount.OnDate;
                newArticle.OffDateDiscount = productDiscount.OffDate;
                
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
            if ((discountValue > 100) || (discountValue < 0)) 
            {
                return basePrice;
            }
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
