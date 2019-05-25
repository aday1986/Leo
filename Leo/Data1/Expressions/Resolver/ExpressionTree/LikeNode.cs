using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leo.Data1.Expressions.ValueObjects;

namespace Leo.Data1.Expressions.Resolver.ExpressionTree
{
    class LikeNode : Node
    {
        public LikeMethod Method { get; set; }
        public MemberNode MemberNode { get; set; }
        public string Value { get; set; }
    }
}
