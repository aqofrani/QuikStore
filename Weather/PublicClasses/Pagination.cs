using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Weather.PublicClasses
{
    
    /// <summary>
    /// Provides pagination functionality for API requests. 
    /// </summary>
    public class Pagination
    {
        const int perPage = 2;
        public int Total { get; set; }
        public int CurrentPage { get; set; }

        public int PerPage 
        { 
            get { return perPage; } 
        }

        public Pagination()
        {
            this.Total = 1;
            this.CurrentPage = 1;
        }

        public static int ItemPerPage
        {
            get { return perPage; }
        }
    }
}