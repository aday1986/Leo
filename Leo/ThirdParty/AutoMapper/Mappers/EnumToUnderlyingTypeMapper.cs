using System;
using System.Linq.Expressions;
using System.Reflection;
using Leo.ThirdParty.AutoMapper.Internal;
using Leo.ThirdParty.AutoMapper.Mappers.Internal;
using static System.Linq.Expressions.Expression;
using Convert = System.Convert;

namespace Leo.ThirdParty.AutoMapper.Mappers
{
    using static ExpressionFactory;

    public class EnumToUnderlyingTypeMapper : IObjectMapper
    {
        private static readonly MethodInfo ChangeTypeMethod = Method(() => Convert.ChangeType(null, typeof(object)));

        public bool IsMatch(TypePair context)
        {
            var sourceEnumType = ElementTypeHelper.GetEnumerationType(context.SourceType);

            return sourceEnumType != null && context.DestinationType.IsAssignableFrom(Enum.GetUnderlyingType(sourceEnumType));
        }

        public Expression MapExpression(IConfigurationProvider configurationProvider, ProfileMap profileMap,
            IMemberMap memberMap, Expression sourceExpression, Expression destExpression,
            Expression contextExpression) =>
                ToType(
                    Call(ChangeTypeMethod, ToObject(sourceExpression),
                        Constant(destExpression.Type)),
                    destExpression.Type
                );
    }
    
}