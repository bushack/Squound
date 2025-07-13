using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquoundApp_v1
{
    class Constants
    {
        // URL of REST service (replace with your actual service URL)
        //public static string RestUrl = "https://squound.azure.net/api/products/{0}";

        // URL of REST service (Android does not support https://localhost:5001)
        // This URL is used for debugging purposes on Android devices or emulators.
        public static readonly string LocalHostUrl = DeviceInfo.Platform == DevicePlatform.Android ? "10.0.2.2" : "localhost";
        public static readonly string Scheme = "https";
        public static readonly string Port = "5001";
        public static readonly string RestUrl = $"{Scheme}://{LocalHostUrl}:{Port}/api/squound_products/{{0}}";
    }
}
