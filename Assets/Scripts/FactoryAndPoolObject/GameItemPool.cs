using Gameplay.GameItems;
using Zenject;

namespace FactoryAndPoolObject
{
    public class GameItemPool<T> : MemoryPool<T> where T : GameItem
    {
        protected override void OnDespawned(T item)
        {
            item.gameObject.SetActive(false);
        }

        protected override void OnSpawned(T item)
        {
            item.gameObject.SetActive(true);
        }
    }
}