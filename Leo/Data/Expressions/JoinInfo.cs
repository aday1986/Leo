namespace Leo.Data.Expressions
{
    public class JoinInfo
    {
        public SourceInfo Left { get; set; }
        public SourceInfo Right { get; set; }
        public string OnText { get; set; }
        public JoinEnum JoinEnum { get; set; }

    }

}
