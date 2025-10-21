using System.Collections.Generic;
using DG.Tweening;
using Gameplay.GameItems;
using UnityEngine;

namespace Extensions
{
    public static class CameraExtensions
    {
        /// <summary>
        /// Плавно настраивает ортографическую камеру чтобы вместить все точки
        /// </summary>
        /// <param name="camera">Камера для настройки</param>
        /// <param name="points">Список точек которые нужно вместить</param>
        /// <param name="padding">Отступ от краев</param>
        /// <param name="animationDuration">Длительность анимации</param>
        /// <param name="moveEase">Ease функция для перемещения</param>
        /// <param name="zoomEase">Ease функция для зума</param>
        /// <param name="onComplete">Колбек завершения анимации</param>
        /// <returns>Sequence анимации</returns>
        public static void FitToPoints(this Camera camera,
            List<Transform> points,
            float padding = 4f,
            float animationDuration = 1f,
            Ease moveEase = Ease.OutCubic,
            Ease zoomEase = Ease.OutCubic,
            System.Action onComplete = null)
        {
            if (points == null || points.Count == 0)
                return;

            if (!camera.orthographic)
            {
                Debug.LogWarning("FitToPoints работает только с ортографической камерой!");
                return;
            }

            var minX = float.MaxValue;
            var maxX = float.MinValue;
            var minY = float.MaxValue;
            var maxY = float.MinValue;

            foreach (var point in points)
            {
                var position = point.position;
                minX = Mathf.Min(minX, position.x);
                maxX = Mathf.Max(maxX, position.x);
                minY = Mathf.Min(minY, position.y);
                maxY = Mathf.Max(maxY, position.y);
            }
            
            var centerX = (minX + maxX) / 2f;
            var centerY = (minY + maxY) / 2f;
            var width = maxX - minX + padding * 2f;
            var height = maxY - minY + padding * 2f;

            var screenRatio = (float)Screen.width / Screen.height;
            var targetSize = Mathf.Max(height / 2f, width / (2f * screenRatio));

            var targetPosition = new Vector3(centerX, centerY, camera.transform.position.z);

            var animationSequence = DOTween.Sequence();

            animationSequence.Join(
                camera.transform.DOMove(targetPosition, animationDuration)
                    .SetEase(moveEase)
            );

            animationSequence.Join(
                DOTween.To(
                    () => camera.orthographicSize,
                    x => camera.orthographicSize = x,
                    targetSize,
                    animationDuration
                ).SetEase(zoomEase)
            );

            if (onComplete != null)
            {
                animationSequence.OnComplete(() => onComplete());
            }

            animationSequence.Play();
        }
    }
}