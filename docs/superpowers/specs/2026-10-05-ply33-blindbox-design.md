# PLY33 Blindbox machine — design

## Goal
PLY33 playable: the player gachas 3–5 blind-box balls from a machine into a basket, presses Done, then on the
live-stream table taps each ball open and eats the food inside. Only scene `Assets/PLY33/scenes/PLY33.unity` changes;
PLY31/PLY32 scenes and shared code stay untouched.

## Decisions (from the human)
| Question | Decision |
|---|---|
| Flow | Gacha 3–5 balls → basket → Done → live panel (no PayPanel) → tap ball open → eat |
| Basket display | Shows the ball (Spine skin), not the food icon — food stays hidden until opened |
| Duplicate `BlindBoxItemCtrl` | Write a new one in `PLY33.Blindbox`; PLY33 uses it, the global one stays for other scenes |
| Namespace | `PLY33.Blindbox` |
| UIParticle (Coffee) | Skipped |
| Other scenes | Must not be affected |

## Key facts found in code
- `Assets/PLY33/scripts/*.cs` were decompiled stubs (empty bodies) referencing missing types: `IState`,
  `BaseStateMachine`, `ShelfCtrl`, `SupermarketItemListSO`, `LeanTweenType`, `Coffee.UIParticle`.
- PLY33 scene already holds components by these files' guids (state machine + states + machine ctrl on
  `blindbox-machine`, `BlindboxSlotShelf` on `ball-cheese`) with no serialized values → keep file names and `.meta`.
- PLY33 has no PayPanel (`UIManager._payPanel` null) → stock `SupermarketPanel.OnPressDoneBtn` would NRE.
- Scene contains `Ply32DirectEatFlow`, which skips the machine and opens the live panel at start.
- `SupermarketItemMukbang.Init` hard-codes `isBlindBox = false`; global `BlindBoxItemCtrl.OpenBox` requires an
  Animator the table slots don't have.
- Needed hooks are already virtual: `SupermarketPanel.OnPickItem/OnTakeOut/OnPressDoneBtn`,
  `ItemSpmkIconCtrl.ActiveIcon`, `SupermarketItemMukbang.Init`, `ItemMukbang.OnPointerDown`, `SlotShelfCtrl.Init/Awake/GetFirstView`.
- Spine: machine `blindbox` anims `idle/act/gacha/gachagacha`; ball `hlw_qua_cau` anims `idle/pose/animation`, skins `1/2/3`.
- Luna does not render masked graphics reliably (comment in `BasketCtrl`) → the ball flies to the basket as an
  unmasked copy.

## Components (`Assets/PLY33/scripts`, namespace `PLY33.Blindbox`)
| Class | Responsibility |
|---|---|
| `IState` | State contract: name, enter/update/exit, suitability |
| `BaseStateMachine` | Collects `IState` components, runs string-keyed transitions in `Update` |
| `BlindboxState` (+ `Idle/Act/Gacha/Open`) | Machine sequence: idle loop → `act` → `gacha` → ball drops (random food assigned) and waits for pick |
| `BlindboxStateMachine` | Shared context: machine model, ball slot, food pool, play request, ball reset |
| `BlindboxMachineCtrl` | Play button: requests a round, disabled while busy or basket full |
| `BlindboxSlotShelf : SlotShelfCtrl` | Ball in the machine; pickable only after it dropped |
| `BlindboxSupermarketPanel : SupermarketPanel` | Pick → ball flies to basket; Done → live panel; take-out disabled |
| `BlindboxSpmkIconCtrl : ItemSpmkIconCtrl` | Basket slot shows a ball skin instead of the food icon |
| `BlindBoxItemCtrl` | Table ball: tap N times → spring + open anim → reveal food |
| `BlindboxItemMukbang : SupermarketItemMukbang` | Table slot: blocks eating until its ball is opened |

## Scene wiring (PLY33 only, via Unity MCP)
- Remove `Ply32DirectEatFlow` GameObject from PLY33.
- Replace `SupermarketPanel` with `BlindboxSupermarketPanel`, re-assign fields, `_requireAmount = 3`.
- `ball-cheese`: Button + raycast target; machine fields; fly ball (unmasked SkeletonGraphic) under SupermarketPanel.
- Basket: 5 `ItemSpmkIcon` → `BlindboxSpmkIconCtrl` with a ball SkeletonGraphic on their `Blindbox` child.
- Table: 5 slots → `BlindboxItemMukbang`, each with a ball child + `BlindBoxItemCtrl`; re-assign `_itemMukbangs`.

## Risks
- Replacing a component drops its serialized values → record them first and re-assign.
- `SupermarketLivePanel.OnValidate` calls `Init` in Edit Mode → Spine calls guarded by `Application.isPlaying`.
- PLY33 scene has uncommitted human changes → scene is not committed without the human's OK.

## Testing
No test assembly in the project. Verify: compile + console clean, Play Mode probe (gacha 3, Done, open, eat),
`git status` shows no other scene changed. Manual: game feel, anim timing, ball/slot positions, Luna build.

## Review outcomes
- Reviewer: no blocking issues; shared code diff empty. Accepted as-is: Act → Gacha uses a fixed `_delayToAct`
  instead of the anim end; the Edit Mode table preview shows balls instead of foods; the eat tutorial hand starts on
  a closed ball.
- Fixed during verify: the probe resolved the global `BlindBoxItemCtrl` (namespace lookup), not a product bug.
- Play Mode probe: 3 gacha rounds (ball drops, can't be picked mid-drop, flies to basket showing a ball skin, Done
  enabled at 3) → Done → live panel with 3 slots matching the basket → 3 taps open the ball and reveal the food → eat
  path runs (`OpenItem` + `Consume`); 0 errors.

## Rewrite on the human's stubs (2026-10-06)
The human replaced the first implementation with decompiled stubs and asked to implement them **without declaring
any field beyond the stub**.

| Question | Decision |
|---|---|
| `LeanTweenType` | DOTween `Ease` |
| `Coffee.UIParticle` field | Dropped |
| `ShelfCtrl` base | New empty class |
| `SupermarketItemListSO` | Added by the human; asset `Assets/PLY33/so/BlindboxGachaItems.asset` |
| `BlindboxMachineCtrl` | Idle hint: plays `act` every `_actInterval` s while idle; pressing the catcher restarts the countdown |
| Who starts a round | `BlindboxStateMachine.CatcherBtn` (Act becomes suitable once it is pressed) |

- States are keyed by serialized `_StateName`; transitions need the target in `_NextStates`/`_CrossStates`
  (`idle→act→gacha→open→idle`, set in the scene) and `IsSuitable()`.
- The ball drops with `DOAnchorPosY(StartPosY).From()`: its scene position is the landing spot.
- Table ball: tap → `SpringCtrl.PlaySpring`; `_waitOpen` locks taps for `_delayTap`; open reveals `_itemSlot.IconParent`.
- Members added only to compile: `IState.NextStates/CrossStates`, `InitialState` override, `GachaItemList` property,
  `BlindboxActState.Awake` override. No new fields or constants.
- Unused stub fields: `BlindboxOpenState._mask` (disabling a Mask would show its Image), `BlindboxSlotShelf._blindboxSkins`
  (skins come from `BlindboxSkin` so machine, basket and table show the same ball).
- Play Mode probe: 3 rounds `idle>act>gacha>open`, early pick blocked, basket 1→3, Done enabled at 3 → 3 live slots →
  3 taps open the ball and show the food; 0 errors.
- Machine anim sequence (human, 2026-10-06): press → `act` once (Act ends on its Complete, `_delayToAct` = wait
  before it, 0 in scene) → `gacha` once (`_gachaTime` = extra wait after it, 0 in scene) → `gachagacha` once with the ball dropping, then `idle`.
  Idle and the hint never cut a one-shot that has `idle` queued. Probe timeline: act 0.01s, gacha 0.47s (1 Complete each),
  gachagacha + drop 0.81s; Idle and the hint do not cut gachagacha. `SupermarketPanel.blackBackground` removed (human).
- Ball goes to the basket on its own once it lands (human, 2026-10-06): `BlindboxOpenState` calls
  `BlindboxSlotShelf.Pick()`; the ball is no longer tapped. Probe: 4 catcher presses, no tap → basket 1,2,3,4.
- Final machine flow (human, 2026-10-06): press → `act` once → `gacha` once and held on its last frame (gate open)
  → ball drops → flies to the basket → machine returns to `idle` only when the ball arrives. `gachagacha` unused.
  The basket clears `CanPick` on arrival; `BlindboxOpenState` waits for it. Probe: act 0.01s, gacha 0.48s, drop
  0.83s, ball leaves machine 1.32s, idle 1.94s; 0 errors.
