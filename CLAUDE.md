# CLAUDE.md

## Environment

- **Engine:** Unity 2022.3.15f1
- **Type:** Playable ad (q07 mukbang ASMR)
- **Playable SDK:** Playworks UPP 7.2.0 (`com.unity.playworks.upp` → `../../7.2.0/scripts`). Not Luna 6.3.0 — check the 7.2.0 API before reusing code from other playable projects.
- **Plugins:** DOTween (`Assets/Plugins/Demigiant`), Spine (`Assets/Spine`), TextMeshPro 3.0.6
- **Content:** `Assets/PLY1/`, scenes in `Assets/Scenes/PLY1/`

## Git Workflow

- Only `git commit` locally. Never `git push` unless explicitly asked.

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

---

**These guidelines are working if:** fewer unnecessary changes in diffs, fewer rewrites due to overcomplication, and clarifying questions come before implementation rather than after mistakes.