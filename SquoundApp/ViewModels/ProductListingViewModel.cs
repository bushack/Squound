﻿using CommunityToolkit.Mvvm.ComponentModel;

using Shared.DataTransfer;


namespace SquoundApp.ViewModels
{
    [QueryProperty(nameof(Product), "Product")]
    public partial class ProductListingViewModel : BaseViewModel
    {
        // NOTE - The [QueryProperty] attribute is used to bind the Product property
        // to a query parameter named "Product". This allows the ProductListingViewModel
        // to receive a Product object when the page is navigated to, enabling it to
        // display the details of the selected product.
        // This binding is typically set up in the navigation logic of the application,
        // where the Product object is passed as a parameter when navigating to the ProductListingPage.
        // See SquoundApp/Pages/ProductSearchPage.xaml.cs for the navigation logic which can be
        // found in the TapGestureRecognizer command handler for the product item.
        [ObservableProperty]
        ProductDto product = new();

        public ProductListingViewModel()
        {
            Title = "Product Listing";
        }
    }
}
