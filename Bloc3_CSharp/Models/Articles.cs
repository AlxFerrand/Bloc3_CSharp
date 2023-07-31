using Bloc3_CSharp.Data;
using Bloc3_CSharp.Services;
using Bloc3_CSharp.Services.abstractServices;
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
    }
}
