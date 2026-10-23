using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

namespace Pet
{
    /// <summary>
    /// Di chuyển pet theo pointer khi kéo; thả tay thì trả thẳng về vị trí đã lưu.
    /// Input dùng EventSystem giống <see cref="ItemMukbang"/> — thêm component <see cref="PetDragPointerInput"/> trên object có Graphic raycast.
    /// </summary>
    [DisallowMultipleComponent]
    public class PetDragCtrl : MonoBehaviour
    {
        [SerializeField] private Vector2 _dragPositionOffset;
        [SerializeField] private Canvas _liftCanvas;
        [SerializeField] private int _liftSortOrder = 1;
        [SerializeField] private PetCtrl _character;
        [SerializeField] private PetStateMachine _stateMachine;
        [SerializeField] private RectTransform _rect;
        [SerializeField] private AudioClip _touchPet;

        private Vector2 _homeAnchored;
        private bool _dragging;

        public bool IsDragging => _dragging;

        private void Awake()
        {
            if (_character == null)
                _character = GetComponent<PetCtrl>();
            if (_stateMachine == null)
                _stateMachine = GetComponent<PetStateMachine>();
            if (_rect == null && _character != null)
                _rect = _character.GetComponent<RectTransform>();
            if (_rect == null)
                _rect = GetComponent<RectTransform>();
            if (_liftCanvas == null && _character != null && _character.Skeleton != null)
                _liftCanvas = _character.Skeleton.GetComponent<Canvas>();

            _homeAnchored = _rect.anchoredPosition;
        }
        private void OnEnable()
        {
            _rect.anchoredPosition = _homeAnchored;
        }

        /// <summary>Giống ItemMukbang: gọi từ OnPointerDown.</summary>
        public virtual void HandlePointerDown(PointerEventData eventData)
        {
            if (_rect == null || _stateMachine == null)
                return;
            if (!CanPick())
                return;


            var parentRect = _rect.parent as RectTransform;
            if (parentRect == null)
                return;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentRect, eventData.position, eventData.pressEventCamera, out var localPoint))
                return;

            _rect.anchoredPosition = localPoint + _dragPositionOffset;
            if (!_stateMachine.TrySetState(PetStateMachine.DragState))
            {
                _rect.anchoredPosition = _homeAnchored;
                return;
            }

            _dragging = true;
            AudioManager.Instance.PlaySoundEffect(_touchPet);
            SetLiftSorting(true);
        }

        /// <summary>Giống ItemMukbang: gọi từ OnDrag.</summary>
        public virtual void HandleDrag(PointerEventData eventData)
        {
            if (!CanDrag())
                return;

            var parentRect = _rect.parent as RectTransform;
            if (parentRect == null)
                return;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentRect, eventData.position, eventData.pressEventCamera, out var localPoint))
                return;

            _rect.anchoredPosition = localPoint + _dragPositionOffset;
        }

        /// <summary>Thả pointer — trả thẳng về home.</summary>
        public virtual void HandlePointerUp(PointerEventData eventData)
        {
            EndDrag();
        }

        /// <summary>Kết thúc kéo; trả thẳng về vị trí đã lưu lúc pointer down.</summary>
        public void EndDrag()
        {
            if (!_dragging)
                return;
            _dragging = false;
            if (_rect == null)
                return;
            _rect.anchoredPosition = _homeAnchored;
            _character?.PlayAnimByName(PetCtrl.IDLE, true, null);
            SetLiftSorting(false);
        }

        protected virtual bool CanPick()
        {
            if (ItemMukbang.BlockPick)
                return false;
            if (_stateMachine.GetState(PetStateMachine.DragState) == null)
                return false;
            if (_stateMachine.CurrentState != PetStateMachine.IdleState)
                return false;
            if (_dragging)
                return false;
            return true;
        }

        protected virtual bool CanDrag()
        {
            if (ItemMukbang.BlockPick)
                return false;
            return _dragging;
        }

        private void OnDisable()
        {
            _dragging = false;
            SetLiftSorting(false);
        }

        private void SetLiftSorting(bool isLifted)
        {
            if (_liftCanvas == null)
                return;
            _liftCanvas.overrideSorting = isLifted;
            if (isLifted)
                _liftCanvas.sortingOrder = _liftSortOrder;
        }

    }
}
