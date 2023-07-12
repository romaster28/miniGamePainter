using System;
using UnityEngine;

namespace Sources.Core.UserControl
{
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private GameObject _default;

        [SerializeField] private GameObject _active;

        public event Action Entered;

        public event Action Exit;

        public event Action Clicked;
        
        public bool IsShown => gameObject.activeSelf;
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show(bool chosenActive = false)
        {
            gameObject.SetActive(true);
            
            SetChosen(chosenActive);
        }
        
        public void SetChosen(bool isActive)
        {
            _default.SetActive(!isActive);
            
            _active.SetActive(isActive);
        }

        private void OnMouseEnter()
        {
            Entered?.Invoke();
        }

        private void OnMouseExit()
        {
            Exit?.Invoke();
        }

        private void OnMouseUpAsButton()
        {
            Clicked?.Invoke();
        }
    }
}