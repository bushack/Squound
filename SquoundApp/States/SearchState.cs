using Shared.DataTransfer;


namespace SquoundApp.States
{
    public class SearchState
    {
        private SearchQueryDto m_CurrentQuery = new();
        private SearchQueryDto m_PreviousQuery = new();

        // Flags if any search parameter has changed since last fetch.
        private bool m_HasChanged = true;

        // Time of last successful fetch.
        private DateTime m_LastFetchTime;

        // The data should be considered out-of-date if the interval since the last fetch exceeds 30 minutes.
        private readonly TimeSpan m_MaxFetchInterval = TimeSpan.FromMinutes(30);


        /// <summary>
        /// Returns true if any internal data has changed since the last API call.
        /// </summary>
        public bool HasChanged
        {
            get => m_HasChanged || ((DateTime.Now - m_LastFetchTime) > m_MaxFetchInterval);
        }

        /// <summary>
        /// Returns true if no internal data has changed since the last API call.
        /// </summary>
        public bool HasNotChanged
        {
            get => !HasChanged;
        }

        //
        public void ResetChangedFlag()
        {
            m_HasChanged = false;
            m_LastFetchTime = DateTime.Now;
        }


        //
        public string? Category
        {
            get => m_CurrentQuery.Category;
            set
            {
                m_CurrentQuery.Category = value;
                m_HasChanged = true;
            }
        }

        //
        public string? Subcategory
        {
            get => m_CurrentQuery.Subcategory;
            set
            {
                m_CurrentQuery.Subcategory = value;
                m_HasChanged = true;
            }
        }

        //
        public string? Manufacturer
        {
            get => m_CurrentQuery.Manufacturer;
            set
            {
                m_CurrentQuery.Manufacturer = value;
                m_HasChanged = true;
            }
        }

        //
        public string? Material
        {
            get => m_CurrentQuery.Material;
            set
            {
                m_CurrentQuery.Material = value;
                m_HasChanged = true;
            }
        }

        //
        public string? Keyword
        {
            get => m_CurrentQuery.Keyword;
            set
            {
                m_CurrentQuery.Keyword = value;
                m_HasChanged = true;
            }
        }

        //
        public decimal? MinPrice
        {
            get => m_CurrentQuery.MinPrice;
            set
            {
                m_CurrentQuery.MinPrice = value;
                m_HasChanged = true;
            }
        }

        //
        public decimal? MaxPrice
        {
            get => m_CurrentQuery.MaxPrice;
            set
            {
                m_CurrentQuery.MaxPrice = value;
                m_HasChanged = true;
            }
        }

        //
        public int PageNumber
        {
            get => m_CurrentQuery.PageNumber;
        }

        //
        public void IncrementPageNumber()
        {
            m_CurrentQuery.PageNumber++;
            m_HasChanged = true;
        }

        //
        public void DecrementPageNumber()
        {
            m_CurrentQuery.PageNumber--;
            m_HasChanged = true;
        }

        //
        public int PageSize
        {
            get => m_CurrentQuery.PageSize;
            set
            {
                m_CurrentQuery.PageSize = value;
                m_HasChanged = true;
            }
        }

        //
        public ItemSortOption SortBy
        {
            get => m_CurrentQuery.SortBy;
            set
            {
                m_CurrentQuery.SortBy = value;
                m_HasChanged = true;
            }
        }

        /// <summary>
        /// Returns a deep copy of the current search query.
        /// </summary>
        public SearchQueryDto CopyOfCurrentQuery
        {
            get => CloneQuery(m_CurrentQuery);
        }

        /// <summary>
        /// Returns a deep copy of the previous search query.
        /// </summary>
        public SearchQueryDto CopyOfPreviousQuery
        {
            get => CloneQuery(m_PreviousQuery);
        }


        //
        public string ToQueryString()
        {
            return m_CurrentQuery.ToQueryString();
        }

        /// <summary>
        /// Resets the current search query to its default state.
        /// </summary>
        public void ResetSearch()
        {
            m_CurrentQuery = new SearchQueryDto();
        }


        /// <summary>
        /// Restores the previous search query to the current search query.
        /// </summary>
        public void RestorePreviousSearch()
        {
            m_CurrentQuery = CloneQuery(m_PreviousQuery);
        }


        /// <summary>
        /// Saves the current search query to the previous search query.
        /// </summary>
        public void SaveCurrentSearch()
        {
            m_PreviousQuery = CloneQuery(m_CurrentQuery);
        }


        //
        private static SearchQueryDto CloneQuery(SearchQueryDto query)
        {
            return new SearchQueryDto
            {
                Category        = query.Category,
                Subcategory     = query.Subcategory,
                Manufacturer    = query.Manufacturer,
                Material        = query.Material,
                Keyword         = query.Keyword,

                MinPrice        = query.MinPrice,
                MaxPrice        = query.MaxPrice,

                PageNumber      = query.PageNumber,
                PageSize        = query.PageSize,
                SortBy          = query.SortBy
            };
        }
    }
}
