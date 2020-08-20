using System;
using UnityEngine;

namespace Actor_Components
{
    public class MoveBasedSprite : MonoBehaviour
    {
        [SerializeField] private Sprite leftSprite;
        [SerializeField] private Sprite rightSprite;
        private Sprite _defaultSprite;

        private SpriteRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _defaultSprite = _renderer.sprite;
            
            GetComponentInParent<MovementController>().SubscribeToHorizontalDirectionChangedEvent(SwitchSprite);
        }

        private void SwitchSprite(int direction)
        {
            if (direction == 0)
                _renderer.sprite = _defaultSprite;
            else if (direction > 0)
                _renderer.sprite = rightSprite;
            else if (direction < 0)
                _renderer.sprite = leftSprite;
        }
    }
}
