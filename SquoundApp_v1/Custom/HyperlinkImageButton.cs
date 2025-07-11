using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquoundApp_v1.Custom
{
    internal class HyperlinkImageButton : ImageButton
    {
        public static readonly BindableProperty UrlProperty =
            BindableProperty.Create(nameof(Url), typeof(string), typeof(HyperlinkImageButton), null);

        public string Url
        {
            get { return (string)GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        public HyperlinkImageButton()
        {
            GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    if (string.IsNullOrEmpty(Url))
                        return;

                    try
                    {
                        await Launcher.OpenAsync(Url);
                    }

                    catch (Exception ex)
                    {
                        // Handle any exceptions that may occur when trying to open the URL
                        Console.WriteLine($"Unable to launch {Url} : {ex.Message}");

                        await Shell.Current.DisplayAlert(
                            $"App not found",
                            $"The selected app is not installed on this device.",
                            "OK");
                    }
                })
            });
        }
    }
}