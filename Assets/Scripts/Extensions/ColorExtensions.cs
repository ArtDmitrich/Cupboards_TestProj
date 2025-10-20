using UnityEngine;

namespace Extensions
{
    public static class ColorExtensions
    {
        /// <summary>
        /// Генерирует визуально различимый цвет на основе ID
        /// </summary>
        /// <param name="id">Уникальный идентификатор для генерации цвета</param>
        /// <returns>Различимый цвет</returns>
        public static Color GetDistinctColor(this int id)
        {
            // Равномерно распределяем оттенки по цветовому кругу
            var hue = (id * 0.618034f) % 1f; // Золотое сечение для лучшего распределения
            var saturation = 0.8f; // Высокая насыщенность
            var lightness = 0.6f;  // Средняя яркость
        
            return Color.HSVToRGB(hue, saturation, lightness);
        }
    }
}