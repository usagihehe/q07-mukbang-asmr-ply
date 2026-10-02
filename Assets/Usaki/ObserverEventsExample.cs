using System;
using UnityEngine;

public enum GameEvent
{
    PlayerScored,
    EnemyDefeated
}

public class ObserverEventsExample : MonoBehaviour
{
    private ObserverEvents<GameEvent, int> gameEvents = new ObserverEvents<GameEvent, int>();

    private void Start()
    {
        // Register observers for different game events
        gameEvents.RegisterEvent(GameEvent.PlayerScored, OnPlayerScored);
        gameEvents.RegisterEvent(GameEvent.EnemyDefeated, OnEnemyDefeated);

        // Notify observers with sample data
        gameEvents.Notify(GameEvent.PlayerScored, 10);
        gameEvents.Notify(GameEvent.EnemyDefeated, 100);

        // Unregister an observer and notify again
        gameEvents.UnRegisterEvent(GameEvent.PlayerScored, OnPlayerScored);
        gameEvents.Notify(GameEvent.PlayerScored, 20);
    }

    private void OnPlayerScored(int score)
    {
        
    }

    private void OnEnemyDefeated(int points)
    {
      
    }
}
