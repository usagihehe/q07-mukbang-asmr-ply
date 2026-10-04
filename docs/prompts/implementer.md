# Implementer prompt template

```
You are the IMPLEMENTER role. Unity project at <PROJECT_PATH>, branch <BRANCH> (already checked out).
Read CLAUDE.md first and follow it strictly (namespaces, <200 lines/class, SOLID, temp-file convention,
NEVER edit <PROTECTED FILES>). Read the spec <SPEC_PATH> (sections <N>) and plan <PLAN_PATH> step <N>.

Do plan step(s) <N> ONLY:
1. Write the tests first: <TEST FILE PATH> (namespace <NS>.Tests, style like <EXISTING TEST>), covering <CASES>.
2. Implement: <FILES / CLASSES> as the spec describes.
3. Edits to existing code: <EXACT file:line → change>. Leave <THINGS TO LEAVE ALONE> untouched.
Do NOT delete files, do NOT touch scenes/prefabs, do not commit.

Facts you need (verified):
- <API signatures, file:line, enum values, save field names, gotchas>

Verify via Unity MCP (deferred tools: ToolSearch "select:mcp__UnityMCP__refresh_unity,mcp__UnityMCP__read_console,
mcp__UnityMCP__run_tests,mcp__UnityMCP__get_test_job"):
refresh_unity(compile="request", mode="force", scope="all") → read_console(types=["error"]) (ignore <KNOWN ERRORS>)
→ run_tests(mode="EditMode", group_names=["<NS>.Tests"]) → get_test_job(job_id, wait_timeout=60, include_failed_tests=true).
All tests must pass. If MCP is not responding, say so; do not claim it compiles.

Report under <N> words: files created/edited, test counts, anything in the spec you couldn't follow and why.
```
