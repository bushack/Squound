using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SquoundApp_v1.Models;
using SquoundApp_v1.Pages;
using SquoundApp_v1.Services;

namespace SquoundApp_v1.ViewModels
{
    public partial class AboutUsViewModel : BaseViewModel
    {
        public AboutUsModel model { get; set; }

        AboutUsService service;

        public AboutUsViewModel(AboutUsService service)
        {
            this.service = service;

            Title = "About Us";
        }

        [RelayCommand]
        async Task GoToAboutUsAsync()
        {
            if (model is null)
                await GetAboutUsAsync();

            await Shell.Current.GoToAsync(nameof(AboutUsPage));
        }

        [RelayCommand]
        async Task GetAboutUsAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                model = await service.GetHTTP();
            }

            catch (Exception ex)
            {
                // Handle exceptions, e.g., log them or show an alert to the user.
                Console.WriteLine($"Error fetching data: {ex.Message}");

                await Shell.Current.DisplayAlert(
                    "Error",
                    "An error occurred while attempting to fetch data",
                    "OK");
            }

            finally
            {
                IsBusy = false;
            }
        }
    }
}
