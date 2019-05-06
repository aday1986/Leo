using System;
using System.Reflection;

namespace Leo.ThirdParty.AutoMapper.Configuration.Conventions
{
    public abstract class SourceToDestinationMapperAttribute : Attribute
    {
        public abstract bool IsMatch(TypeDetails typeInfo, MemberInfo memberInfo, Type destType, Type destMemberType, string nameToSearch);
    }
}