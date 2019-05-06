using System.Linq.Expressions;
using System.Reflection;

namespace Leo.ThirdParty.AutoMapper.Configuration
{
    public interface IPropertyMapConfiguration
    {
        void Configure(TypeMap typeMap);
        MemberInfo DestinationMember { get; }
        LambdaExpression SourceExpression { get; }
        LambdaExpression GetDestinationExpression();
        IPropertyMapConfiguration Reverse();
    }
}