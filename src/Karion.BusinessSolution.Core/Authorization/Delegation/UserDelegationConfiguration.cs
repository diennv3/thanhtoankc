namespace Karion.BusinessSolution.Authorization.Delegation
{
    public class UserDelegationConfiguration : IUserDelegationConfiguration
    {
        public bool IsEnabled { get; set; }

        public UserDelegationConfiguration()
        {
            IsEnabled = true;
        }
    }
}
