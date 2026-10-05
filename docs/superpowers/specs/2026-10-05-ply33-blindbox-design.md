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
