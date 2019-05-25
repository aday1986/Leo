using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Leo.Util
{
    /// <summary>
    /// <see cref="T"/>出列委托。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ItemDequeueEventHandler<T>(object sender,QueueEventArgs<T> e);
    /// <summary>
    /// 工作队列。
    /// </summary>
    /// <typeparam name="T">要进行处理的对象。</typeparam>
    public class WorkQueue<T>
    {
        private bool isWorking; //表明处理线程是否正在工作
        private Queue<T> queue; //实际的队列

        /// <summary>
        /// 当<see cref="T"/>出列时发生。
        /// </summary>
        public event ItemDequeueEventHandler<T> AfterItemDequeue;

        /// <summary>
        /// 当<see cref="IEnumerable{T}"/>出列时发生，需配置<see cref="EachQueue"/>值大于1才会触发。
        /// </summary>
        public event ItemDequeueEventHandler<IEnumerable<T>> AfterItemsDequeue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eachQueue">每个队列操作处理的对象个数。</param>
        /// <param name="action"></param>
        public WorkQueue(int eachQueue,ItemDequeueEventHandler<IEnumerable<T>> action) : this()
        {
            EachQueue = eachQueue;
            AfterItemsDequeue += action;
        }

        static WorkQueue()
        {
            //ThreadPool.SetMaxThreads(10, 10);
        }

        public WorkQueue()
        {
            queue = new Queue<T>();
        }

        /// <summary>
        /// <see cref="T"/>剩余未处理的数量。
        /// </summary>
        public int QueueCount { get { return queue.Count; } }

        /// <summary>
        /// 每次队列处理<see cref="T"/>数量。
        /// </summary>
        public int EachQueue { get; } = 1;

        /// <summary>
        /// 队列处理是否单线程顺序执行,默认为True单线程.
        /// </summary>
        public bool IsSingle { get; set; } = true;

        /// <summary>
        /// 向工作队列添加<see cref="T"/>。
        /// </summary>
        /// <param name="item">添加到队列的对象</param>
        public void EnqueueItem(T item)
        {
            lock (this)
            {
                queue.Enqueue(item);
                if (!isWorking)
                {
                    isWorking = true;
                    if (EachQueue > 1 && AfterItemsDequeue != null)
                    {
                        ThreadPool.QueueUserWorkItem(DoUserWorks);
                    }
                    else if (AfterItemDequeue != null)
                    {
                        ThreadPool.QueueUserWorkItem(DoUserWork);
                    }
                    else
                    {
                        throw new ArgumentNullException("未注册队列处理事件。");
                    }
                }
            }
        }

        /// <summary>
        /// 处理队列中对象。
        /// </summary>
        /// <param name="o"></param>
        private void DoUserWork(object o)
        {
            T item;
            while (isWorking)
            {
                if (queue.Count > 0)//双锁
                {
                    lock (this)
                    {
                        if (queue.Count > 0)
                        {
                            item = queue.Dequeue();
                            if (IsSingle)
                            {
                                AfterItemDequeue?.Invoke(this, new QueueEventArgs<T>(item));
                            }
                            else
                            {
                                ThreadPool.QueueUserWorkItem(obj =>
                                {
                                    AfterItemDequeue?.Invoke(this, new QueueEventArgs<T>(item));
                                });
                            }
                        }
                        else isWorking = false;
                    }
                }
            }
        }

        /// <summary>
        /// 处理队列中多个对象。
        /// </summary>
        /// <param name="o"></param>
        private void DoUserWorks(object o)
        {
            List<T> items;
            while (isWorking)
            {
                if (queue.Count > 0)//双锁
                {
                    lock (this)
                    {
                        if (queue.Count > 0)
                        {
                            int count = (queue.Count >= EachQueue ? EachQueue : queue.Count);

                            items = new List<T>();
                            for (int i = 0; i < count; i++)
                            {
                                items.Add(queue.Dequeue());
                            }

                            if (IsSingle)
                            {
                                AfterItemsDequeue?.Invoke(this, new QueueEventArgs<IEnumerable<T>>(items));
                            }
                            else
                            {
                               
                                ThreadPool.QueueUserWorkItem(obj =>
                                {
                                    AfterItemsDequeue?.Invoke(this, new QueueEventArgs<IEnumerable<T>>(items));
                                });
                              
                            }
                        }
                    }
                }
                else
                {
                    isWorking = false;
                }
                   

            }
        }
    }

    /// <summary>
    /// UserWork事件的参数，包含item对象
    /// </summary>
    public class QueueEventArgs<T> : EventArgs
    {
        public T Item { get; }
        public QueueEventArgs(T item)
        {
            Item = item;
        }
    }
}
