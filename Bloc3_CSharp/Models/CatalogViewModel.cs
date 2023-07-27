namespace Bloc3_CSharp.Models
{
    public class CatalogViewModel
    {
        
        public CatalogViewModel(List<Articles> articles, List<Category> categories, Category selectedCategory) 
        {
            Articles = articles;
            Categories = categories;
            SelectedCategory = selectedCategory;
        }
        public List<Articles> Articles { get; }
        public List<Category> Categories { get; }
        public Category SelectedCategory { get; }
    }
}
