using Bloc3_CSharp.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Bloc3_CSharp.Models
{
    public class Articles
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal BasePrice { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string PictureName { get; set; }
        public int DiscountValue { get; set; }
        public int DiscountId { get; set; }
        public string OnDateDiscount { get; set; }
        public string OffDateDiscount { get; set; }

        public Articles() { }

        public Articles(Product p,ApplicationDbContext context)
        {
            Id = p.Id;
            Label = p.Label;
            Description = p.Description;
            PictureName = p.PictureName;
            CategoryId = p.CategoryId;
            CategoryName = p.Category.Name;
            BasePrice = p.Price;
            DiscountId = p.DiscountId;
            if (!(DiscountId == 0))
            {
                DiscountValue = context.Discounts.Find(DiscountId).Value;
                OnDateDiscount = context.Discounts.Find(DiscountId).OnDate;
                OffDateDiscount = context.Discounts.Find(DiscountId).OffDate;
            }
            else
            {
                DiscountValue = 0;
                OnDateDiscount = "";
                OffDateDiscount = "";
            }
                SetPrice();
        }

        public void SetPrice()
        {
            decimal price = Math.Round(BasePrice * (100 - DiscountValue))/100.0M;
            if (price > 0)
            {
                Price = price;
            }
            else
            {
                Price = 0;
            }
        }
    }
}
