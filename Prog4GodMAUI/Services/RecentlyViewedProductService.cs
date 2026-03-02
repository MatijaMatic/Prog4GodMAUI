using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Prog4GodMAUI.Models;
using Newtonsoft.Json;

namespace Prog4GodMAUI.Services
{
    public partial class RecentlyViewedProductService : ObservableObject
    {
        public RecentlyViewedProductService() { }

        [ObservableProperty]
        private Collection<Product> recentlyViewedProducts = new();

        public void AddProduct(Product product)
        {
            var existingProduct = RecentlyViewedProducts.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                RecentlyViewedProducts.Remove(existingProduct);
            }

            RecentlyViewedProducts.Insert(0, product);

            if (RecentlyViewedProducts.Count > 8)
            {
                RecentlyViewedProducts.RemoveAt(RecentlyViewedProducts.Count - 1);
            }
            SaveProduct();
        }

        public void LoadProducts()
        {
            var productsJson = Preferences.Get("recently_viewed", string.Empty);
            if (!string.IsNullOrEmpty(productsJson))
            {
                var products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(productsJson);
                RecentlyViewedProducts = products ?? new ObservableCollection<Product>();
            }
        }

        public void SaveProduct()
        {
            var productsJson = JsonConvert.SerializeObject(RecentlyViewedProducts);//pogledati sta je Newtonsoft.Json to si koristio ovde
            Preferences.Set("recently_viewed", productsJson);
        }
    }
}
