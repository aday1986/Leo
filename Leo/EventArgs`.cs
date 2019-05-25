using System;
using System.Collections.Generic;
using System.Text;

namespace Leo
{
    public delegate void EventHandler<T>(object sender, T e);
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
