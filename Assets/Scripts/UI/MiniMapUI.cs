using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MiniMapUI : ViewUIWithCloseButton, IMiniMapUI
    {
        [SerializeField] private RawImage _miniMap;
    
        public void SetRenderTexture(RenderTexture rt)
        {
            _miniMap.texture = rt;
        }
    }
}