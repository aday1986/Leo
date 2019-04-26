using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Util
{
    /// <summary>
    /// 单例模式
    /// </summary>
   sealed class SingletonDemo
    {
        //volatile修饰：编译器在编译代码的时候会对代码的顺序进行微调，用volatile修饰保证了严格意义的顺序。
        //一个定义为volatile的变量是说这变量可能会被意想不到地改变，这样，编译器就不会去假设这个变量的值了。
        //精确地说就是，优化器在用到这个变量时必须每次都小心地重新读取这个变量的值，而不是使用保存在寄存器里的备份。
        private volatile static SingletonDemo mySingleton;

        // 定义一个标识确保线程同步
        private static readonly object locker = new object();

        // 定义私有构造函数，使外界不能创建该类实例
        private SingletonDemo()
        {
        }

        //定义公有方法提供一个全局访问点。
        public static SingletonDemo GetInstance()
        {
            if (mySingleton == null)//双锁减少锁使用的开销，只在第一次构建时使用锁。
            {
                //这里的lock其实使用的原理可以用一个词语来概括“互斥”这个概念也是操作系统的精髓
                //其实就是当一个进程进来访问的时候，其他进程便先挂起状态
                lock (locker)
                {
                    if (mySingleton == null)
                    {
                        mySingleton = new SingletonDemo();
                    }
                }
            }
            return mySingleton;
        }
    }
}
