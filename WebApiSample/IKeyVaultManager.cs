namespace Products.API
{
    public interface IKeyVaultManager
    {
        public Task<string> GetSecret(string secretName);
    }
}
