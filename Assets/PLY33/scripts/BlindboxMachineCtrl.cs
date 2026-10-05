using UnityEngine;
using UnityEngine.UI;
using Usaki;

namespace PLY33.Blindbox
{
    /// <summary>Play button of the gacha machine: usable only while the machine is idle and the basket has room.</summary>
    public class BlindboxMachineCtrl : MonoBehaviour
    {
        [SerializeField] private BlindboxStateMachine _stateMachine;
        [SerializeField] private Button _playBtn;
        [SerializeField] private BasketCtrl _basket;

        private void Awake()
        {
            _playBtn.onClick.AddListener(OnPressPlayBtn);
        }

        private void Update()
        {
            _playBtn.interactable = _stateMachine.IsIdle && !_basket.IsFull;
        }

        private void OnPressPlayBtn()
        {
            AudioManager.Instance.PlayAudioClick();
            _stateMachine.RequestPlay();
        }
    }
}
