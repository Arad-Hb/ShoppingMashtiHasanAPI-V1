using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain.BaseModel
{
    public class PageModel
    {
        public int PageIndex { get; set; }
        private int pageSize; 
        public int PageSize
        {
            get { 
                if (pageSize==0) { pageSize = 10; }
                
                return this.pageSize; 
            }
            set
            {
                if (value == 0)
                    value = 10;
                this.pageSize = value;
            }
        }
        private int recordCount;
        public int RecordCount { get 
            {
                return this.recordCount;
            } 
            
            set { 
                this.recordCount = value;
            } }
        public int PageCount
        {
            get
            {
                if (this.pageSize==0)
                {
                    this.PageSize = 10;
                }
                if (RecordCount % PageSize == 0)
                {
                    return RecordCount / PageSize;

                }
                else
                {
                    return (RecordCount / PageSize) + 1;
                }
            }
        }
    }
}
