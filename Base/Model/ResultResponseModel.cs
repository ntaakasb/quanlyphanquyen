using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Model
{
   public class ResultResponseModel<T> 
    {
        public TraceModel trace { get; set; }
        public T result { get; set; }
        public long timestamp { get; set; }
    }
}
