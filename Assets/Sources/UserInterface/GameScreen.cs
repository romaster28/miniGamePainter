using System;
using System.Collections;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.UserInterface
{
    public class GameScreen : MonoBehaviour
    {
        [SerializeField] private Button _skip;

        [SerializeField] private Button _reset;

        [Min(0)] [SerializeField] private int _waitActiveSkipMilliSeconds = 60;

        public event Action OnResetClicked;

        public event Action OnSkipClicked;

        private IEnumerator Start()
        {
            _skip.onClick.AddListener(delegate { OnSkipClicked?.Invoke(); });

            _reset.onClick.AddListener(delegate { OnResetClicked?.Invoke(); });

            _skip.gameObject.SetActive(false);

            yield return new WaitForSeconds(_waitActiveSkipMilliSeconds);

            _skip.gameObject.SetActive(true);
        }
    }
}