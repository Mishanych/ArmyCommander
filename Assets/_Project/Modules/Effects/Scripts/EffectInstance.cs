using UnityEngine;
using Zenject;

namespace ArmyCommander.Modules.Effects
{
    public class EffectInstance : MonoBehaviour
    {
        public class Factory : PlaceholderFactory<Object, EffectInstance> { }

        public void PlayAt(Transform parent)
        {
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
        }
        public void PlayAt(Vector3 position)
        {
            transform.SetParent(null);
            transform.position = position;
        }
        
    }
}