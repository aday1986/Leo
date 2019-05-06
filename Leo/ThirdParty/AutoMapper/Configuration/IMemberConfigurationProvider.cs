namespace Leo.ThirdParty.AutoMapper.Configuration
{
    public interface IMemberConfigurationProvider
    {
        void ApplyConfiguration(IMemberConfigurationExpression memberConfigurationExpression);
    }
}