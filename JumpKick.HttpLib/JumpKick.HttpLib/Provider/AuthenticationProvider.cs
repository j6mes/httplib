namespace JumpKick.HttpLib.Provider
{
    public interface AuthenticationProvider
    {
        Header GetAuthHeader();
    }
}
