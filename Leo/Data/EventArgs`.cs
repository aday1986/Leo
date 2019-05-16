using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Data
{
   public class EventArgs<T>:EventArgs
    {
        public EventArgs()
        {
        }

        public EventArgs(T item)
        {
            Item = item;
        }

        public T Item { get; set; }
    }
}
