using Leo.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Leo.Util.SLB
{


    public interface IServer<T>
    {
        event ItemDequeueEventHandler<T> AfterItemDequeue;
        event EventHandler Completed;
        string Name { get; set; }
        int CompletedCount { get; }
        int QueueCount { get; }
        void EnqueueItem(T item);
    }

    public class Server<T> : IServer<T>
    {
        private readonly WorkQueue<T> workQueue = new WorkQueue<T>();

        public event EventHandler Completed;
        public event ItemDequeueEventHandler<T> AfterItemDequeue;

        public Server()
        {
            workQueue.AfterItemDequeue += WorkQueue_AfterItemDequeue;
        }

        private void WorkQueue_AfterItemDequeue(object sender, QueueEventArgs<T> e)
        {
            AfterItemDequeue?.Invoke(this, e);
            CompletedCount++;
            Completed?.Invoke(this, null);
        }
        public int QueueCount => workQueue.QueueCount;

        public string Name { get; set; }

        public int CompletedCount { get; private set; }

        public void EnqueueItem(T item)
        {
            workQueue.EnqueueItem(item);
        }
    }

    public interface IServerCollection<T> : IList<IServer<T>>
    {
        void EnqueueItem(T item);
    }

    public class ServerCollection<T> : List<IServer<T>>, IServerCollection<T>
    {

        private Random random = new Random();

        public void EnqueueItem(T item)
        {

            Task.Run(() =>
            {
                var s = this.OrderBy(t => random.Next()).ThenBy(t => t.QueueCount).FirstOrDefault();
                s.EnqueueItem(item);
            });


        }

    }
}
