using System.Linq.Expressions;

namespace Leo.ThirdParty.AutoMapper
{
    public class MemberFinderVisitor : ExpressionVisitor
    {
        public MemberExpression Member { get; private set; }

        protected override Expression VisitMember(MemberExpression node)
        {
            Member = node;
            return base.VisitMember(node);
        }
    }
}