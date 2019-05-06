using System;
using System.Collections.Generic;
using System.Reflection;

namespace Leo.ThirdParty.AutoMapper.Configuration.Conventions
{
    public interface IChildMemberConfiguration
    {
        bool MapDestinationPropertyToSource(ProfileMap options, TypeDetails sourceType, Type destType, Type destMemberType, string nameToSearch, LinkedList<MemberInfo> resolvers, IMemberConfiguration parent);
    }
}