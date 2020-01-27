using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Weather.PublicClasses
{
    /// <summary>
    /// Provides standard JSON response.
    /// </summary>
    public class APIResponse
    {
        public int Status { get; set; }
        public bool Error { get; set; }
        public string Message { get; set; }
        public Pagination Pagination { get; set; }

        public object Data { get; set; }

        /// <summary>
        /// Constructor to generate response without pagination.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public APIResponse(HttpStatusCode status, string message, Object data)
        {
            this.Status = (int)status;
            this.Message = message;
            this.Data = data;
            this.Error = false;
            if (!status.Equals(HttpStatusCode.OK))
            {
                this.Error = true;
            }

            this.Pagination = new Pagination();
        }

        /// <summary>
        /// Constructor to generate response with pagination.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <param name="page"></param>
        /// <param name="total"></param>
        public APIResponse(HttpStatusCode status, string message, IEnumerable<object> data, int page, int total=1)
        {
            this.Status = (int)status;
            this.Message = message;
            this.Data = data;
            this.Error = false;
            if (!status.Equals(HttpStatusCode.OK))
            {
                this.Error = true;
            }

            this.Pagination = new Pagination();

            if(data != null)
            {
                
                double totalPage = total / (double)this.Pagination.PerPage;

                this.Pagination.Total = (int)Math.Ceiling(totalPage);

                this.Pagination.CurrentPage = (page == 0)? 1 : page;
            }

        }
    }
}