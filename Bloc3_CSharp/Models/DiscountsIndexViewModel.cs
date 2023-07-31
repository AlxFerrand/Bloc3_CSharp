using System.Collections;

namespace Bloc3_CSharp.Models
{
    public class DiscountsIndexViewModel
    {
        public DiscountsIndexViewModel(List<Discount> discounts, Dictionary<int,int> nombersOfProductByDiscount) 
        {
            Discounts = discounts;
            NombersOfProductByDiscount = nombersOfProductByDiscount;
        }
        public List<Discount> Discounts { get; }
        public Dictionary<int,int> NombersOfProductByDiscount { get; }
    }
}
