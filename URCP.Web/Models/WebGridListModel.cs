using System;
using System.CodeDom;
using System.Collections.Generic;

namespace URCP.Web.Models
{
    public class WebGridList<T>
    {
        public List<T> List { get; set; }
        public int RowCount { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        #region Ctor

        public WebGridList()
        {
            this.List = new List<T>();
        }

        public WebGridList(List<T> list , int rowCount, int pageSize, int pageIndex) : this ()
        {
            this.List = list;
            this.RowCount = rowCount;
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
        }
        #endregion
    }
}