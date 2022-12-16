using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RGN.Sample.UI
{
    public abstract class AbstractPanel : MonoBehaviour
    {
        /// <summary>
        /// Instantly hides the game object, override for flexibility
        /// </summary>
        public virtual void Show(bool isInstant, System.Action onComplete)
        {
            gameObject.SetActive(true);
            onComplete?.Invoke();
        }

        /// <summary>
        /// Instantly shows the game object, override for flexibility
        /// </summary>
        public virtual void Hide(bool isInstant, System.Action onComplete)
        {
            gameObject.SetActive(false);
            onComplete?.Invoke();
        }

        private void OnEnable()
        {
            //seal this one from override
        }

        private void OnDisable()
        {
            //seal this one from override
        }
    }
}
