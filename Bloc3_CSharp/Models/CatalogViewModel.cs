namespace Bloc3_CSharp.Models
{
    public class CatalogViewModel
    {
        
        public CatalogViewModel(List<Articles> articles, List<Category> categories) 
        {
            Articles = articles;
            Categories = categories;
        }
        public List<Articles> Articles { get; }
        public List<Category> Categories { get; }
        public Category SelectedCategory { get; }
    }
}
