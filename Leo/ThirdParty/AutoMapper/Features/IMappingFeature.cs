namespace Leo.ThirdParty.AutoMapper.Features
{
    public interface IMappingFeature
    {
        void Configure(TypeMap typeMap);
        IMappingFeature Reverse();
    }
}
