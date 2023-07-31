using System.Collections;

namespace Bloc3_CSharp.Models
{
    public class DiscountsIndexViewModel
    {
        public DiscountsIndexViewModel(List<Discount> discounts) 
        {
            Discounts = discounts;
        }
        public List<Discount> Discounts { get; }
    }
}
