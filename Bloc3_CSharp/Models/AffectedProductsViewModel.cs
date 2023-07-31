namespace Bloc3_CSharp.Models
{
    public class AffectedProductsViewModel
    {
        public AffectedProductsViewModel(Discount discount, List<Articles> articlesListOfDiscount) 
        {
            Discount = discount;
            ArticlesListOfDiscount = articlesListOfDiscount;
        }
        public Discount Discount { get; }
        public List<Articles> ArticlesListOfDiscount { get; }
    }
    

}
