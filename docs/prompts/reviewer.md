# Reviewer prompt template (read-only)

```
You are the REVIEWER role (READ-ONLY: no edits, no Unity MCP). Project <PROJECT_PATH>, branch <BRANCH>.
Read CLAUDE.md and the spec <SPEC_PATH>. Review the uncommitted diff (`git status`, `git diff`,
`git diff --cached` for deletions; new files under <PATHS>). IGNORE <files changed by the human / re-serialization noise>.

Check, verifying by reading code (report only verified issues):
1. Spec compliance; protected files untouched (<LIST>); behaviour-identical where the spec says so
   (compare with `git show HEAD:<path>`; baseline: <BASELINE FILE>).
2. <Specific correctness questions for this change: gating, null singletons, idempotency, ordering, ...>
3. Leftover references to anything deleted (code, .unity, .prefab, .asset — by class name AND script GUID).
4. Tests: meaningful (not tautologies), leave no global state dirty.
Report ranked by severity with file:line, failure scenario, suggested fix. Under <N> words.
```
