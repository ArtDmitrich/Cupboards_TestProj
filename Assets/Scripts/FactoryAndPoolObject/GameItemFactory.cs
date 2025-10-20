using Gameplay.GameItems;

namespace FactoryAndPoolObject
{
    public class GameItemFactory<T> : IGameItemFactory<T> where T : GameItem
    {
        private readonly GameItemPool<T> _pool;

        public GameItemFactory(GameItemPool<T> pool)
        {
            _pool = pool;
        }

        public T Create()
        {
            return _pool.Spawn();
        }

        public void Despawn(T item)
        {
            _pool.Despawn(item);
        }
    }
}