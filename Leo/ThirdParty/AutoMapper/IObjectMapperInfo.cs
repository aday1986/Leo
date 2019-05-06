namespace Leo.ThirdParty.AutoMapper
{
    public interface IObjectMapperInfo : IObjectMapper
    {
        TypePair GetAssociatedTypes(TypePair initialTypes);
    }
}