# CLAUDE.md

## Environment

- **Engine:** Unity 2022.3.15f1
- **Type:** Playable ad (q07 mukbang ASMR)
- **Playable SDK:** Playworks UPP 7.2.0 (`com.unity.playworks.upp` → `D:/WaveZ/Package/scripts`). Not Luna 6.3.0 — check the 7.2.0 API before reusing code from other playable projects.
- **Plugins:** DOTween (`Assets/Plugins/Demigiant`), Spine 4.1 (`Assets/Spine`), TextMeshPro 3.0.6
- **Content:** one folder per playable variant — `Assets/PLY31/` (scenes, scripts, sprites), `Assets/PLY32/` (scenes), `Assets/PLY33/` (blindbox gacha machine, scripts in namespace `PLY33.Blindbox`). Build scenes: `Assets/Scenes/SampleScene.unity`, `Assets/PLY31/scenes/PLY31.unity`

## Git Workflow

- Only `git commit` locally. Never `git push` unless explicitly asked.

## Project Context

Game code compiles into `Assembly-CSharp` and lives in `Assets/Scripts`, `Assets/Usaki` (shared utilities) and
`Assets/PLY*/scripts`. Third-party code (`Assets/Plugins`, `Assets/Spine`, `Assets/TextMesh Pro`, the Playworks
package) is not ours — don't edit it.

**Shared systems (use these, don't invent parallel ones):** singletons via `Usaki.ZMonoSingleton<T>`; events via
`Usaki/Observer*` (always pair add with remove); popups via `Usaki/Panel`; UI tweens via `Usaki/UIAnim`.
Kill tweens on disable/destroy (`DOKill` / `SetLink`).

**Temporary files (Claude):** put every throwaway file you create — probes, one-off test scripts, flags, outputs — in
`Assets/_ClaudeTemp/` (when Unity must compile/import it) or `Temp/claude/` (everything else). Both are git-ignored.
Delete them only with `bash .claude/scripts/rm-temp.sh <path>...`; it refuses anything outside those two folders or
tracked by git, and removes `.meta` sidecars. Never use `rm`, `git clean` or Unity MCP asset deletion for cleanup.

**Harness workflow:** "theo quy trình harness" → follow `docs/workflow.md` (prompts in `docs/prompts/`, runtime
checks in `docs/play-mode-probe.md`, probe skeleton in `docs/templates/ClaudeTemp/`).

**Scope:** namespace, 200-line limit and SOLID rules below apply to **new files and new classes**. Existing code that
doesn't follow them stays as it is unless the task is to refactor it.

Behavioral guidelines to reduce common LLM coding mistakes. Merge with project-specific instructions as needed.

**Tradeoff:** These guidelines bias toward caution over speed. For trivial tasks, use judgment.

## Think Before Coding

**Don't assume. Don't hide confusion. Surface tradeoffs.**

Before implementing:
- State your assumptions explicitly. If uncertain, ask.
- If multiple interpretations exist, present them - don't pick silently.
- If a simpler approach exists, say so. Push back when warranted.
- If something is unclear, stop. Name what's confusing. Ask.

## Simplicity First

**Minimum code that solves the problem. Nothing speculative.**

- No features beyond what was asked.
- No abstractions for single-use code.
- No "flexibility" or "configurability" that wasn't requested.
- No error handling for impossible scenarios.
- If you write 200 lines and it could be 50, rewrite it.

Ask yourself: "Would a senior engineer say this is overcomplicated?" If yes, simplify.

## Surgical Changes

**Touch only what you must. Clean up only your own mess.**

When editing existing code:
- Don't "improve" adjacent code, comments, or formatting.
- Don't refactor things that aren't broken.
- Match existing style, even if you'd do it differently.
- If you notice unrelated dead code, mention it - don't delete it.

When your changes create orphans:
- Remove imports/variables/functions that YOUR changes made unused.
- Don't remove pre-existing dead code unless asked.

The test: Every changed line should trace directly to the user's request.

## Goal-Driven Execution

**Define success criteria. Loop until verified.**

Transform tasks into verifiable goals:
- "Add validation" → "Write tests for invalid inputs, then make them pass"
- "Fix the bug" → "Write a test that reproduces it, then make it pass"
- "Refactor X" → "Ensure tests pass before and after"

For multi-step tasks, state a brief plan:
```
1. [Step] → verify: [check]
2. [Step] → verify: [check]
3. [Step] → verify: [check]
```

Strong success criteria let you loop independently. Weak criteria ("make it work") require constant clarification.

## Naming
- Variable names must be descriptive and self-explanatory — the name alone says what it holds.
- No single letters or cryptic abbreviations: `dt` → `dropTarget`, `ownCol` → `ownCollider`, `t` → `elapsedTime`.
- Exception: loop counters/indices in `for` loops may be single letters (`i`, `j`, `k`).

## Comments

**The best comment is the one you did not write. Explain yourself in code first.**

- Write all comments in English.
- Always try to replace a comment by refactoring the code into self-documenting functions and variables.
- Never comment *what* the code does — the code already says it. A comment earns its place only when it states *why*: a constraint, a non-obvious tradeoff, or a bug the code is working around.
- Keep it short: comments and XML `<summary>` blocks are 1–2 lines.
- Delete commented-out code. Git remembers it.

## Unity-Specific Rules
- Use DOTween for all tweening; do not write manual lerp loops in `Update()`
- Every script must have a namespace: `ProjectName.Feature` (e.g., `MyGame.Combat`, `MyGame.UI`)
- Favor composition over inheritance — max 2 levels of class hierarchy

## SOLID Principles — mandatory for all C# code

- **Single Responsibility**: Each class does one thing. No class exceeds 200 lines. If a class has "And" in its description ("handles input and spawns enemies"), split it.
- **Open/Closed**: Extend behavior through interfaces, abstract classes, or ScriptableObjects — never modify working classes to add new features. New enemy type = new class, not a new branch in a switch statement.
- **Liskov Substitution**: Any subclass must be a drop-in replacement for its base class. If overriding a method changes the expected behavior or throws unexpected exceptions, the inheritance is wrong — use composition instead.
- **Interface Segregation**: Keep interfaces small and focused. `IDamageable` has `TakeDamage()`. `IHealable` has `Heal()`. Never force a class to implement methods it doesn't use.
- **Dependency Inversion**: Depend on abstractions, not concrete classes. Use interfaces for cross-system communication. Inject dependencies via [SerializeField], constructor, or Service Locator — never use `new ConcreteClass()` inside another class for service dependencies.

<!-- code-review-graph MCP tools -->
## MCP Tools: code-review-graph

**IMPORTANT: This project has a knowledge graph. ALWAYS use the
code-review-graph MCP tools BEFORE using Grep/Glob/Read to explore
the codebase.** The graph is faster, cheaper (fewer tokens), and gives
you structural context (callers, dependents, test coverage) that file
scanning cannot.

### When to use graph tools FIRST

- **Exploring code**: `semantic_search_nodes` or `query_graph` instead of Grep
- **Understanding impact**: `get_impact_radius` instead of manually tracing imports
- **Code review**: `detect_changes` + `get_review_context` instead of reading entire files
- **Finding relationships**: `query_graph` with callers_of/callees_of/imports_of/tests_for
- **Architecture questions**: `get_architecture_overview` + `list_communities`

Fall back to Grep/Glob/Read **only** when the graph doesn't cover what you need.

### Key Tools

| Tool | Use when |
| ------ | ---------- |
| `detect_changes` | Reviewing code changes — gives risk-scored analysis |
| `get_review_context` | Need source snippets for review — token-efficient |
| `get_impact_radius` | Understanding blast radius of a change |
| `get_affected_flows` | Finding which execution paths are impacted |
| `query_graph` | Tracing callers, callees, imports, tests, dependencies |
| `semantic_search_nodes` | Finding functions/classes by name or keyword |
| `get_architecture_overview` | Understanding high-level codebase structure |
| `refactor_tool` | Planning renames, finding dead code |

### Workflow

1. The graph auto-updates on file changes (via hooks).
2. Use `detect_changes` for code review.
3. Use `get_affected_flows` to understand impact.
4. Use `query_graph` pattern="tests_for" to check coverage.

## Verification — required before saying a task is done

1. **Unity MCP (primary).** Requires Unity Editor open with the server started in the "MCP for Unity" window. After editing scripts:
   `refresh_unity(compile="request", mode="force", scope="scripts")` (`scope="all"` when you added new `.cs` files) → `read_console(types=["error"])` (`types` must be a list).
   If MCP is not connected, say so — don't claim the code compiles.
2. **`bash .claude/scripts/check-compile.sh` (fallback, no Editor needed).** Exit 0 = compiles, 1 = errors (listed), 2 = could not verify — treat 2 as "not verified". New `.cs` files are only compiled after Unity regenerates the csproj.
3. A Stop hook runs the fallback check automatically when `.cs` files changed and blocks finishing on real compile errors.
4. Things only a human can verify — game feel, animation timing, scene/prefab wiring, playable build in the ad network preview — list them explicitly at the end as "needs manual check".

Don't hand-edit `.unity` / `.prefab` YAML. If a change needs references wired in a scene or prefab, do it through Unity MCP, or tell the human exactly which object/field to assign.

## Keep this file true

When you learn something about this project the hard way — a wrong assumption, a tool quirk, a pitfall that cost time, a fact that contradicts this file — add or fix it here in the same task (one line, under Known pitfalls or the relevant section). Remove lines that turn out wrong. Don't log one-off task details.

### Known pitfalls
- UnityMCP is not in `.mcp.json`; it is registered per machine with `claude mcp add --scope local --transport http UnityMCP http://127.0.0.1:8080/mcp` (or "Configure" for Claude Code in the MCP for Unity window). Restart Claude Code afterwards.
- Unity's csproj targets net471 but `DOTween.dll` is built for net472; plain `dotnet build` drops the reference and reports hundreds of false DOTween errors (`check-compile.sh` passes `ResolveAssemblyReferenceIgnoreTargetFrameworkAttributeVersionMismatch=true`).
- UnityMCP `execute_code` may fail ("Operation is not supported on this platform"). To run one-off Editor code, write a temporary EditMode test in `Assets/_ClaudeTemp/Editor/`, run it with `run_tests`, then delete it with `rm-temp.sh`.
- `refresh_unity(scope="scripts")` does not import newly created `.cs` files — use `scope="all"` after adding scripts.
- UnityMCP `manage_gameobject(action="duplicate")` on a UI object gives the copy a wrong `anchoredPosition` — set it explicitly afterwards.
- When the Unity Editor window is not focused, Play Mode can hang in "playmode_transition" or run very slowly (editor throttling). Ask the human to focus Unity, or set Preferences > General > Interaction Mode = No Throttling. After changing that setting, a run already stuck stays stuck — stop Play Mode and start it again.
- Player Settings edited in the Unity UI are not on disk until `File > Save Project` (or Editor close); check the file, not the UI.
- Two `GameManager` classes exist (global and `PLY3.GameManager`); UnityMCP `manage_components` can't resolve `GameManager` — pass `component_type="GameManager, Assembly-CSharp"`.
- UnityMCP `manage_scriptable_object` can't set `Array.size`, but setting `List.Array.data[i]` grows the list.
- Scenes may reference sound SOs whose asset doesn't exist (dead guid) — the Inspector shows None and `CharSound` NREs in the eat states. The shared one is `Assets/Resources/so/CharacterSound.asset`.
- PLY32 foods live in two lists: `Ply32DirectEatFlow._foods` (runtime) and `SupermarketLivePanel._itemSets` (Edit Mode table preview, applied by `OnValidate`). UnityMCP `set_property` doesn't fire `OnValidate`, so call `Init` on each slot and save the scene.
- Probes left in `Assets/_ClaudeTemp/` boot themselves in every Play Mode run and drive the game (e.g. auto-feeding the character); delete them right after the run.
- UnityMCP `manage_gameobject`/`manage_components` by_id or by_path can't find objects under an inactive parent (e.g. `SupermarketLivePanel`); activate the parent temporarily, then restore it.
- UnityMCP: setting a `List<Component>` field to GameObject IDs stores nulls; set `field.Array.data[i]` to the component's instance ID.
- PLY33 blindbox states are keyed by the serialized `_StateName`, and a transition only fires if the target is listed in the current state's `_NextStates`/`_CrossStates` — a new state needs both set in the scene.
- `dotnet build` incremental once reported OK on edited files that no longer compiled; `check-compile.sh` now passes `--no-incremental`.
- DOTween safe mode logs only "An error inside a tween callback was taken care of" with no stack trace; set `DOTween.useSafeMode = false` in a probe's `Boot` to get the real exception and line.
- `Assets/Resources/so/supermarketitem-1/halloween/` was recreated outside Unity as `halloweenfood/` with new guids; refs to the old guids are dead (None in the Inspector) — repoint them to the `halloweenfood` assets.
- Grepping a scene for a script's guid misses components inside prefab instances (the scene only stores the prefab guid) — use UnityMCP `find_gameobjects(search_method="by_component")` before concluding a component is absent.
- PLY33 balls (machine, fly, table) must use `hlw_qua_cau_SkeletonData` (skins `1`/`2`/`3`); `BlindboxSkin.Apply` throws "Skin not found" on `blindbox_SkeletonData` (the machine) and aborts `SupermarketLivePanel.ShowPanel`.
- Table blindboxes only appear for foods listed in `SupermarketItemData._blindBoxItems`; an empty list shows every food already open. In PLY33 that list must contain every food in `Assets/PLY33/so/BlindboxGachaItems.asset`, or the missing ones land on the table already open.
- Spine `TrackEntry.Complete` never fires if another `SetAnimation` interrupts the entry; character states poll `CharacterCtrl.IsPlaying(entry)` instead of waiting on Complete.
- Switching git branches while Unity is open can fail with "unable to unlink" on scenes Unity holds, leaving a half-applied checkout; check `git status` + console after every switch.

---

**These guidelines are working if:** fewer unnecessary changes in diffs, fewer rewrites due to overcomplication, and clarifying questions come before implementation rather than after mistakes.