#if UNITY_EDITOR
// TEMPLATE — copy to Assets/_ClaudeTemp/<Name>Probe.cs, rename the class, fill in the TODOs. Delete it after the run
// with `bash .claude/scripts/rm-temp.sh Assets/_ClaudeTemp/<Name>Probe.cs`.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TODO_Company.ClaudeTemp
{
    public class ProbeTemplate : MonoBehaviour
    {
        private const string TargetScene = "TODO_MainScene";
        private static readonly string[] KnownErrors = { "TODO known pre-existing error text" };
        private static readonly string[] TouchedPrefs = { "TODO_PrefKey" }; // every PlayerPrefs key the run changes
        private static readonly Dictionary<string, int?> SavedPrefs = new Dictionary<string, int?>();
        private readonly List<string> _errors = new List<string>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Boot()
        {
            foreach (var k in TouchedPrefs) SavedPrefs[k] = PlayerPrefs.HasKey(k) ? PlayerPrefs.GetInt(k) : (int?)null;
            // TODO: unblock automation, e.g. PlayerPrefs.SetInt("AdsRemoved", 1) to skip an Editor mock interstitial.
            var go = new GameObject(nameof(ProbeTemplate));
            DontDestroyOnLoad(go);
            go.AddComponent<ProbeTemplate>();
        }

        private void OnEnable() => Application.logMessageReceived += OnLog;
        private void OnDisable() => Application.logMessageReceived -= OnLog;
        private void OnApplicationQuit() => RestorePrefs(); // also restores when the run is stopped early

        private void OnLog(string msg, string stack, LogType type)
        {
            if (type != LogType.Error && type != LogType.Exception && type != LogType.Assert) return;
            if (msg.StartsWith("[Probe]")) return;
            foreach (var known in KnownErrors) if (msg.Contains(known)) return;
            _errors.Add($"{type}: {msg}");
        }

        private IEnumerator Start()
        {
            float end = Time.realtimeSinceStartup + 600f;
            while (SceneManager.GetActiveScene().name != TargetScene && Time.realtimeSinceStartup < end) yield return null;
            if (SceneManager.GetActiveScene().name != TargetScene) { Log("DONE aborted: never reached " + TargetScene); yield break; }
            yield return new WaitForSecondsRealtime(3f);

            // TODO: act and observe, one Log line per observation, e.g.
            // Log($"start scene={SceneManager.GetActiveScene().name} uiManagers={FindObjectsByType<UIManager>(FindObjectsInactive.Include, FindObjectsSortMode.None).Length}");

            RestorePrefs();
            Log($"DONE newErrors={_errors.Count}\n" + string.Join("\n", _errors));
        }

        private static void RestorePrefs()
        {
            foreach (var k in TouchedPrefs)
                if (SavedPrefs.TryGetValue(k, out var v) && v.HasValue) PlayerPrefs.SetInt(k, v.Value); else PlayerPrefs.DeleteKey(k);
            PlayerPrefs.Save();
        }

        private static void Log(string m) => Debug.Log("[Probe] " + m);
    }
}
#endif
