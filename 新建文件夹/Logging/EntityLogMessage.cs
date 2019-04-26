namespace Leo.Logging
{
    /// <summary>
    /// 实体模型的操作日志信息。
    /// </summary>
    public class EntityLogMessage
    {
        public string TraceId { get; set; }
        public string UserId { get; set; }
        public string EntityState { get; set; }
        public string EventState { get; set; }
        public string DataType { get; set; }
        public string DataId { get; set; }

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static EntityLogMessage Create(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<EntityLogMessage>(json);
        }
    }
}
