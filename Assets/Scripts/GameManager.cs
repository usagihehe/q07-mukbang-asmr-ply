using Usaki;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Singletons;

public class GameManager : ZMonoSingleton<GameManager>
{
    [SerializeField] private CharacterSoundSO _mukbangSound;
    [SerializeField] private GameSoundSO _gameSound;
    [SerializeField] private int countComplete = 0;
    [SerializeField] private int DoneValue;

    [Header("COIN")]
    [SerializeField] private int _coinAmount = 1400;
    [SerializeField] private bool _useCoinCost = true;
    [SerializeField] private int _foodCost = 200;
    [SerializeField] private CoinBarCtrl _coinBar;

    [Header("FOOD")]
    // When on: slots never mix, and a single add fills the slot to its full phase.
    [SerializeField] private bool _instantFullFood;
    [SerializeField] private bool _allowMixFood = true;

    [Header("IDLE HINT")]
    [SerializeField] private GameObject _firstTimeHint;
    [SerializeField] private float _idleHintDelay = 5f;
    private float _idleTimer;
    private bool _firstTimeHintShown;

    public bool IsCompleteGame { get; set; }

    public CharacterSoundSO CharSound => _mukbangSound;
    public GameSoundSO GameSound => _gameSound;

    public int CoinAmount => _coinAmount;
    public bool UseCoinCost { get => _useCoinCost; set => _useCoinCost = value; }
    public bool InstantFullFood { get => _instantFullFood; set => _instantFullFood = value; }
    public bool AllowMixFood { get => _allowMixFood; set => _allowMixFood = value; }

    /// True when the add may proceed; false when blocked by insufficient funds.
    public bool TrySpend(int amount)
    {
        if (!_useCoinCost) return true;
        if (_coinAmount < amount) return false;
        _coinAmount -= amount;
        _coinBar?.TakeCoin(amount);
        return true;
    }

    public void PayFoodCost()
    {
        TrySpend(_foodCost);
    }

    private void Start()
    {
        IsCompleteGame = false;
        countComplete = 0;
        SetCountComplete(4);
        _coinBar?.ResetCoin(_coinAmount);
        Luna.Unity.LifeCycle.GameStarted();
        Luna.Unity.LifeCycle.GameLoaded();
    }

    public void SetCountComplete(int value)
    {
        DoneValue = value;
    }

    private void Update()
    {
        if (IsCompleteGame && Input.GetMouseButtonDown(0))
        {
            Luna.Unity.Playable.InstallFullGame();
        }

        HandleIdleHint();
    }

    private void HandleIdleHint()
    {
        if (IsCompleteGame)
        {
            _idleTimer = 0f;
            return;
        }

        _idleTimer += Time.deltaTime;
        if (_idleTimer < _idleHintDelay) return;

        if (_firstTimeHint != null && !_firstTimeHintShown)
        {
            _firstTimeHint.SetActive(true);
            _firstTimeHintShown = true;
        }
    }

    public void SetLiveScreen(bool isLive)
    {
        NotifyPlayerInteracted();
    }

    public void NotifyPlayerInteracted()
    {
        _idleTimer = 0f;
        DeactivateHint(_firstTimeHint);
    }

    private static void DeactivateHint(GameObject hint)
    {
        if (hint != null && hint.activeSelf)
        {
            hint.SetActive(false);
        }
    }

    public void InstallFullGame()
    {
        Luna.Unity.Playable.InstallFullGame();
    }

    public void UpdateCountComplete()
    {
        countComplete++;
        if (!IsCompleteGame && countComplete >= DoneValue)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        IsCompleteGame = true;
        Observer.Instance.Notify(ObserverTopic.OnEndLiveGamePlay);
        Luna.Unity.LifeCycle.GameEnded();
    }


}