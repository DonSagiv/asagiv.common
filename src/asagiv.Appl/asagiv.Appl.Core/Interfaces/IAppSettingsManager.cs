namespace asagiv.Appl.Core.Interfaces
{
    internal interface IAppSettingsManager
    {
        TValue Get<TValue>(string key);
        bool TryGet<T>(string key, out T value);
    }
}