using Cysharp.Threading.Tasks;

namespace Core
{
    public interface IInitializer
    {
        UniTask Initialize();
    }
}