using Gameplay.GameItems;

namespace FactoryAndPoolObject
{
    public interface IGameItemFactory<T> where T : GameItem
    {
        T Create();
        void Despawn(T item);
    }
}