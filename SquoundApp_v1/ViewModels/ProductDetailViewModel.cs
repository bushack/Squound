using CommunityToolkit.Mvvm.ComponentModel;
using SquoundApp_v1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquoundApp_v1.ViewModels
{
    [QueryProperty("Product", "Product")]
    public partial class ProductDetailViewModel : BaseViewModel
    {
        [ObservableProperty]
        Product product;

        public ProductDetailViewModel(Product product)
        {
            this.product = product;

            Title = "Product Details";
        }
    }
}
