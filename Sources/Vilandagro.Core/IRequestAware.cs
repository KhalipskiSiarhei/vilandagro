namespace Vilandagro.Core
{
    public interface IRequestAware
    {
        object this[string key] { get; set; }

        T GetValue<T>(string key)
            where T : class;

        T GetValue<T>(string key, bool throwExceptionIfNull)
            where T : class;
    }
}