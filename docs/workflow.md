# Harness workflow — how a task runs

The process used on Capyboba 2. Say "theo quy trình harness" (or "follow the harness workflow") to get it.
It relies on the **superpowers** plugin skills (`brainstorming`, `writing-plans`, `verification-before-completion`)
and the `CLAUDE.md` + `.claude/` files in this kit.

## 1. Classify
| Path | When | Artifact |
|---|---|---|
| Spike | "can we / is it possible" — output is an answer | none, findings only |
| Bounded | small change to an existing flow (flag, one-file fix) | short design in chat, then approval |
| Architectural | new subsystem, new scene, cross-cutting refactor | spec + plan files, then approval |

When in doubt take the heavier path. Hidden complexity found mid-task upgrades the path.

## 2. Understand → design (lead)
1. Explore the code **before** asking (read-only).
2. Ask only questions whose answer changes the design — one at a time, multiple choice, recommended option first.
3. Propose 2–3 approaches with a recommendation.
4. Architectural: write `docs/superpowers/specs/YYYY-MM-DD-<topic>-design.md` — decisions table (what the human said),
   key facts found in code, components, small edits to existing code, risks, testing. Commit it.
5. Write `docs/superpowers/plans/YYYY-MM-DD-<topic>.md` — a table: step, role, task, verify. Commit it.
6. If the human said "spec xong cứ thực hiện luôn" / "make it till done", continue without stopping at each gate.

## 3. Roles
| Role | Who | Does | Never |
|---|---|---|---|
| Lead | main session | spec, plan, scene/prefab wiring via MCP, merging results, commits, final report | hand-edit `.unity`/`.prefab` YAML |
| Implementer | subagent | code + tests for the steps it is given (tests first) | commit, touch scenes, delete files |
| Auditor | subagent, read-only, **parallel** with Implementer | finds concrete risks the change exposes (file:line + minimal fix) | speculate without reading code |
| Reviewer | subagent, read-only | spec compliance + correctness of the diff | edit |
| Verifier | lead (or subagent) | compile, console, tests, Play Mode probe, restore state | claim success without output |

Prompt templates: `docs/prompts/`. Give each subagent: the spec/plan paths, the exact facts it needs (APIs,
file:line), what it must not touch, how to verify, and a word limit for its report.

## 4. Typical order
```
0  Verifier  baseline (when behaviour must not change): record current behaviour first
1  Implementer  tests first → code → edits            ║  Auditor in parallel (read-only)
2  Lead  apply real blockers from the audit; wire scenes/prefabs via MCP; save scenes
3  Reviewer  diff review
4  Lead  fix confirmed findings (verify each claim yourself before acting on it)
5  Verifier  compile → console → EditMode tests → Play Mode probe → restore save/prefs → delete temp files
6  Lead  update CLAUDE.md (pitfalls learned), spec "review outcomes", commit, report
```

## 5. Rules that saved us
- **Treat subagent reports as claims.** Re-check anything that drives a change (e.g. "IAP purchases are lost" was
  disproved by a Play Mode probe; "NRE when enabling cheat from Home" was confirmed by one).
- **Baseline before refactors.** Record the before-state (probe output or a table derived from code) so "nothing
  changed" is checkable.
- **Never edit protected SDK wrappers** — work around them (facade / existing flags) or ask.
- **Destructive actions** (delete outside temp zones, push, reset --hard, build settings) go through the human's
  permission prompt; never route around a denied action (no `mv`-to-temp-then-delete, no `git clean`).
- **Human changes are theirs.** If the working tree has a change you didn't make (e.g. a define added to Player
  Settings), don't commit it; explain the impact and ask.
- **Report honestly**: what was verified (with numbers), what wasn't and why, what needs a manual check.

## 6. Final report shape
1. One-line result. 2. What was built/changed (files, behaviour). 3. Verification with evidence (test counts, probe
output). 4. Review findings fixed. 5. Known limitations accepted. 6. Needs manual check. 7. Git state (branch, commit,
pushed or not).
