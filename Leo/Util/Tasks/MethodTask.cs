using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Leo.Util.Tasks
{
    public class MethodTask : ITask
    {
        private readonly string methodBase64;
        private readonly object[] args;

        public MethodTask(string methodBase64, object[] args)
        {

            this.methodBase64 = methodBase64;
            this.args = args;
        }

        public MethodTask(MethodInfo method, object[] args)
        {
            methodBase64 = ToBase64(method);
            this.args = args;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Execute()
        {
            MethodInfo method = null;
            try
            {
                method = ToMethodInfo(methodBase64);
            }
            catch (Exception ex)
            {
                throw new Exception($"反序列化MethodInfo失败：\n{ex.Message}");
            }
            method.Invoke(null, args);
            return true;
        }

        public static string ToBase64(MethodInfo method) => Converter.SerializeBase64(method);

        public static MethodInfo ToMethodInfo(string base64) => Converter.DeserializeBase64<MethodInfo>(base64);

    }



}



