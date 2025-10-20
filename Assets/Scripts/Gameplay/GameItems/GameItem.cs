using UnityEngine;

namespace Gameplay.GameItems
{
    public class GameItem: MonoBehaviour
    {
        public int Id { get; private set; }
        public GameItemType ItemType { get; private set; }
        
        public Collider2D Collider { get { return _collider ??= GetComponent<Collider2D>(); } }
        private Animator Animator { get { return _animator ??= GetComponent<Animator>(); } }
        
        private Collider2D _collider;
        private Animator _animator;
        
        private const string ISSELECT_BOOL_ANIM = "IsSelect";
        protected void Initialize(int id, GameItemType itemType)
        {
            Id = id;
            ItemType = itemType;   
        }
        
        public void Select() => Animator.SetBool(ISSELECT_BOOL_ANIM, true);
        
        public void Unselect() => Animator.SetBool(ISSELECT_BOOL_ANIM, false);
    }
}