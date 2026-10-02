using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Usaki
{
    public class CoinBarCtrl : MonoBehaviour
    {
        public const float DefaultTime = 0.7f;

        [SerializeField]
        private Image _coinIcon;

        [SerializeField]
        private TextMeshProUGUI _coinTxt;

        [SerializeField]
        private bool _loadCoinFromData;

        private int _curCoin;

        public Image CoinIcon => _coinIcon;

        public int CurrentCoin => _curCoin;

        private void OnEnable()
        {
            SetCoinTxt(4000);
        }

        public void SetCoinTxt(int coin)
        {
            StartCoroutine(ChangeCoin(_curCoin, coin, 1));
        }

        public void ResetCoin(int coin)
        {
            StopAllCoroutines();
            _curCoin = 0;
            StartCoroutine(ChangeCoin(0, coin, 1));
        }

        public void AddCoin(int coin, float timeIncr = 1f)
        {
            StartCoroutine(ChangeCoin(_curCoin, _curCoin + coin, timeIncr));
        }

        public void TakeCoin(int coin, float timeIncr = 1f)
        {
            StartCoroutine(ChangeCoin(_curCoin, _curCoin - coin, timeIncr));
        }

        private IEnumerator ChangeCoin(int startValue, int endValue, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                _curCoin = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, elapsed / duration));
                _coinTxt.text = _curCoin.ToString();
                yield return null;
            }
            _curCoin = endValue;
            _coinTxt.text = _curCoin.ToString();
        }
    }
}
