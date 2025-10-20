using UnityEngine;

namespace Extensions
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Рекурсивно устанавливает слой для GameObject и всех его дочерних объектов
        /// </summary>
        /// <param name="gameObject">Целевой GameObject</param>
        /// <param name="layer">Слой (0-31)</param>
        public static void SetLayerRecursively(this GameObject gameObject, int layer)
        {
            gameObject.layer = layer;
        
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetLayerRecursively(layer);
            }
        }
    }
}