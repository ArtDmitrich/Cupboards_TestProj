using UnityEngine;

namespace UI
{
    public class ViewUI : MonoBehaviour, IViewUI
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
    }
}