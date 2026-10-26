---
name: Hint Hand
description: Add or change tutorial hand hints (HandHint) in a playable scene — tap hints, drag hints, hide-after-first-success flows. Use when the user asks for "hint", "tay hint", "hand hint", "HintHand".
---

## Hint Hand

Tutorial hands in this project are one `Usaki.HandHint` per scene (`Assets/Scripts/HandHint.cs`), driven through the
scene's UI manager. Each hint flow is its own small class that listens to Observer topics — never put hint logic inside
panels or gameplay classes.

### Building blocks

| Need | Call |
|------|------|
| Tap loop on a target | `Ply34UIManager.Instance.ClickHandAt(target)` |
| Drag loop from A to B | `Ply34UIManager.Instance.MoveHand(from, to)` |
| Hide | `Ply34UIManager.Instance.StopHand()` |

- PLY34/PLY345 scenes: `PLY34.UI.Ply34UIManager` (on `CanvasGamePlay`, fields `_handHint`, `_lockImage`).
  Other scenes: `Usaki.UIManager` has the same calls (plus `StartEatTutorial`/`OnEatSlotPress`/`OnEatSlotRelease`).
- Tap hint: the hand starts backed off at `target + _clickOffset` and yoyos in to the target (`_duration`).
  PLY34 uses `_clickOffset` (76, -92) and `_moveDuration` 0.75 s for drag hints.
- The HandHint object must be the last sibling under the canvas (PLY34: `CanvasGamePlay/TopMostGroup/HandHint`).
- Hint classes live in `Assets/PLY34/scripts/`, namespace `PLY34.Hint`, one flow per class, observers added in
  `OnEnable` and removed in `OnDisable`.

### Events to listen to

| Topic | Sent by | Data |
|-------|---------|------|
| `OnCanteenFoodAdded` | `FoodController.CheckDropFood` | `BaseSlot` that received the food |
| `OnCanteenFoodMixed` | `FoodController.CheckDropFood` | `BaseSlot` that mixed |
| `OnCanteenStartLive` | end of `CanteenLivePanel.ShowPanel` | — |
| `OnSelectItem` / `OnDropItem` | `BaseSlotMukbang` / `CanteenDrinkMukbang` press / release | the slot |
| `OnCanteenConsumeItem` | `CanteenLivePanel.OnConsume` | the slot |
| `OnEndLiveGamePlay` | `GameManager.EndGame` | — |

Need a new moment? Add a constant to `Assets/Usaki/ObserverTopic.cs` and one `Notify` line at the source.

### Existing flows (PLY34)

- `Ply34TrayHint` (on `Gameplay`): drag `_firstFood` (bread bowl) → first empty tray slot. After any food lands in a
  slot, drag the first bowl in `CanteenManager.BowlFoods` where `slot.CanMixFood(id)` → that slot; no mix → hide.
  First successful mix → hidden for good. While the `ScrollRect` scrolls the hand is hidden; 0.3 s after the last
  `onValueChanged` it reappears aimed at the bowl's new position.
- `Ply34EatHint` (on `CanteenLivePanel`): tap the first active `_bigSlots`; press it → drag to `CharacterCtrl.mPoint`;
  release → tap again; first `OnCanteenConsumeItem` or game end → hidden.

### Pitfalls

- `HandHint` captures the target position once per call. If the target moves (scroll, tween, layout), call again.
- Canvases are Screen Space – Overlay; positions are converted with `RectTransformExtension.CanvasLocalPoint`.
- `BaseSlotMukbang.Consume()` invokes `onConsume` before its own `OnDropItem`; clear the hint state on consume so the
  release handler doesn't re-show it.
- Big eat slots only appear for food dropped into big tray slots (`BigSlot`, tray IDs 3–4 in PLY34).
- Verify with a Play Mode probe (`docs/play-mode-probe.md`): set `FoodController._currentFood/_currentSlot` by
  reflection and invoke `CheckDropFood`, call `CanteenLivePanel.ShowPanel(..., null)` to skip the water step, then
  `OnPointerDown/Up` and `Consume()` on the target; log `HandHint._hand` active state and position.
