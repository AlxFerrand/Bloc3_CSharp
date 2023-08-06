using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Bloc3_CSharp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        [ValidateNever]
        public virtual Category Category { get; set; }

        [MaybeNull]
        public string PictureName { get; set; }

        [MaybeNull]
        public int DiscountId { get; set;}

        public Product() { }
        public Product(int id, string label, string description, decimal price, int categoryId, string pictureName, int discountId)
        {
            Id = id;
            Label = label;
            Description = description;
            Price = price;
            CategoryId = categoryId;
            PictureName = pictureName;
            DiscountId = discountId;
        }
    }
}
