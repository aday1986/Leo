using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Leo.Util
{
    /// <summary>
    /// 工作队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WorkQueue<T>
    {
        private bool isWorking; //表明处理线程是否正在工作
        private Queue<T> queue; //实际的队列

        public delegate void UserWorkEventHandler<item>(object sender, WorkQueue<item>.EnqueueEventArgs e);

        /// <summary>
        /// 绑定用户需要对队列中的 <see cref="T"/> 对象施加的操作的事件
        /// </summary>
        public event UserWorkEventHandler<T> UserWork;

        public WorkQueue(int n)
        {
            queue = new Queue<T>(n);
        }

        public WorkQueue()
        {
            queue = new Queue<T>();
        }


        /// <summary>
        /// 队列未处理数量
        /// </summary>
        public int QueueCount { get { return queue.Count; } }

        /// <summary>
        /// 队列处理是否单线程顺序执行,默认为True单线程.
        /// </summary>
        public bool IsSingle { get; set; } = true;

        /// <summary>
        /// 向工作队列添加对象,对象添加以后，如果已经绑定工作的事件,会触发事件处理程序,对item对象进行处理
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
                    ThreadPool.QueueUserWorkItem(DoUserWork);
                }
            }

        }

        /// <summary>
        /// 处理队列中对象的函数
        /// </summary>
        /// <param name="o"></param>
        private void DoUserWork(object o)
        {
            T item;
            while (isWorking)
            {
                if (queue.Count > 0)
                {
                    lock (this)
                    {
                        if (queue.Count > 0)
                        {
                            item = queue.Dequeue();
                                if (IsSingle)
                                {
                                    UserWork?.Invoke(this, new EnqueueEventArgs(item));
                                }
                                else
                                {
                                    ThreadPool.QueueUserWorkItem(obj =>
                                    {
                                        UserWork?.Invoke(this, new EnqueueEventArgs(item));
                                    });
                                }      
                        }
                    }
                }
                else
                {
                    lock (this)
                    {
                        isWorking = false;
                    }
                }
            }

        }

        /// <summary>
        /// UserWork事件的参数，包含item对象
        /// </summary>
        public class EnqueueEventArgs : EventArgs
        {
            public T Item { get; private set; }
            public EnqueueEventArgs(T item)
            {

                Item = item;

            }
        }
    }
}
