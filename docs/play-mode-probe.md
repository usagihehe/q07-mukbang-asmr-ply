# Play Mode probe — verifying runtime behaviour without a human clicking

EditMode tests can't reach scene wiring, singletons across scene loads, or startup flow. A **probe** is a temporary
Editor-only MonoBehaviour that boots itself in Play Mode, drives the game through code, and logs one-line results that
Claude reads from `Editor.log`. Template: `docs/templates/ClaudeTemp/ProbeTemplate.cs`.

## Steps
1. **Back up state the run will touch**
   - Save file: `cp "$USERPROFILE/AppData/LocalLow/<Company>/<Product>/<save file>" Temp/claude/<save>.backup`
     (`companyName` / `productName` are in `ProjectSettings/ProjectSettings.asset`).
   - PlayerPrefs the probe changes: record the original values in the probe (or `HasKey` = false → delete afterwards).
2. **Write the probe** in `Assets/_ClaudeTemp/` (git-ignored), wrapped in `#if UNITY_EDITOR`, namespace `*.ClaudeTemp`:
   - `[RuntimeInitializeOnLoadMethod(AfterSceneLoad)]` creates a `DontDestroyOnLoad` GameObject with the probe.
   - Unblock automation (e.g. disable ads so an Editor mock interstitial doesn't wait for a click).
   - `IEnumerator Start()`: wait for the target scene by **real time** (`Time.realtimeSinceStartup`), act, log
     `"[Probe] <tag> key=value ..."`, then restore prefs and log `"[Probe] DONE newErrors=N"`.
   - Collect errors via `Application.logMessageReceived`, skipping known pre-existing ones.
3. **Compile and confirm**: `refresh_unity(scope="all")`, `read_console(types=["error"])`, and
   `grep -c <ProbeClass> Library/ScriptAssemblies/Assembly-CSharp.dll` (> 0 = compiled in).
4. **Run**: load the bootstrap scene, record `wc -l < "$LOCALAPPDATA/Unity/Editor/Editor.log"`, clear console, `manage_editor(play)`.
5. **Wait** (Bash, `run_in_background`) for the marker:
   ```
   L="$LOCALAPPDATA/Unity/Editor/Editor.log"; s=<line count>
   for i in $(seq 1 600); do tail -n +$s "$L" | grep -aq "\[Probe\] DONE\|Exception:" && break; sleep 1; done
   tail -n +$s "$L" | grep -a "^\[Probe\]"
   ```
   If nothing arrives, check `mcpforunity://editor/state`: `playmode_transition` + `is_focused:false` = the Editor is
   throttled → ask the human to focus Unity (or set Interaction Mode = No Throttling). Don't assume the probe failed.
6. **Stop and clean up**: `manage_editor(stop)`, `read_console(types=["error"])`, restore the save (`cp` back, compare
   md5), verify prefs (Windows: `reg query "HKCU\Software\Unity\UnityEditor\<Company>\<Product>"`, prefix with
   `MSYS2_ARG_CONV_EXCL='*'` in Git Bash), delete the probe + backups with `bash .claude/scripts/rm-temp.sh`, refresh.

## Patterns that worked
- **Baseline vs after**: same probe before and after a refactor; outputs must match line for line.
- **Round trips**: switch scenes N times and log singleton counts / which scene owns `X.Instance` each time.
- **Reflection for private fields** when a probe must read UI state (`GetField(name, NonPublic | Instance)`).
- Make the probe test the failure the reviewer predicted (e.g. "enable from Home before data is loaded").
