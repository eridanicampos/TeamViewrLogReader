using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamViewerLogReader.Domain.Class
{
    public class QueryParameters
    {
        public QueryParameters(string queryParameter, object value)
        {
            QueryParameter = queryParameter;
            Value = value;
        }
        public string QueryParameter { get; set; }
        public object Value { get; set; }
    }
}
