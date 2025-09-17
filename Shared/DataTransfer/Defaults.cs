

namespace Shared.DataTransfer
{
    public static class Defaults
    {
        // Limits for item id.
        public const long MinimumItemId = 0;                // Note: Zero is valid because DTO requires a value. API will return 404.
        public const long MaximumItemId = long.MaxValue;

        // Limits for image dimensions.
        public const int MinimumImageWidth = 1;
        public const int MaximumImageWidth = 10000;
        public const int MinimumImageHeight = 1;
        public const int MaximumImageHeight = 10000;

        // Defaults for image dimensions.
        public const int ImageWidth = 1600;
        public const int ImageHeight = 2000;

        // Limits for string properties.
        public const int MinimumStringLength = 3;
        public const int MaximumStringLength = 25;

        // Defaults for pagination.
        public const int PageNumber = 1;
        public const int PageSize = 10;

        // Limits for pagination.
        public const int MinimumPageSize = PageSize;
        public const int MaximumPageSize = 100;

        // Limits for price range queries.
        public const double PracticalMinimumPrice = 0.0;
        public const double PracticalMaximumPrice = 999999.99;

        // Regular expressions for validating string content.
        public const string AlphabeticRegex = @"^[a-zA-Z\s]+$";
        public const string AlphanumericRegex = @"^[a-zA-Z0-9\s]+$";
        public const string KeywordRegex = @"^[a-zA-Z0-9\s-']+$";

        // Error messages for regular expression validation.
        public const string AlphabeticRegexErrorMessage = "Only letters and spaces are allowed.";
        public const string AlphanumericRegexErrorMessage = "Only letters, numbers and spaces are allowed.";
        public const string KeywordRegexErrorMessage = "Only letters, numbers, spaces, hyphens and apostrophes are allowed.";

        // Error messages for string length and range validation.
        public const string StringLengthErrorMessage = " string length out of range.";
        public const string RangeErrorMessage = " value out of range.";
    }
}
