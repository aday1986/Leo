using System;
using System.Reflection;

namespace Leo.ThirdParty.AutoMapper.Configuration.Conventions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class MapToAttribute : SourceToDestinationMapperAttribute
    {
        public string MatchingName { get; }

        public MapToAttribute(string matchingName) 
            => MatchingName = matchingName;

        public override bool IsMatch(TypeDetails typeInfo, MemberInfo memberInfo, Type destType, Type destMemberType, string nameToSearch) 
            => string.Compare(MatchingName, nameToSearch, StringComparison.OrdinalIgnoreCase) == 0;
    }
}