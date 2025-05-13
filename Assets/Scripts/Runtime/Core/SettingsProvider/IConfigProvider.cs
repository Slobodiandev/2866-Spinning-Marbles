using Cysharp.Threading.Tasks;

namespace Core
{
    public interface IConfigProvider
    {
        UniTask Initialize();
        T Get<T>() where T : BaseConfig;
        void Set(BaseConfig config);
    }
}