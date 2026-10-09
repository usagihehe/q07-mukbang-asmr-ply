#!/usr/bin/env bash
# Fast compile check for game scripts without opening Unity.
# Builds the Unity-generated game csproj with dotnet and reports C# errors.
# Exit 0 = compiles, 1 = compile errors, 2 = could not verify (see message).
#
# CONFIGURE for your project:
CSPROJ="Assembly-CSharp.csproj"   # Unity-generated game project
SRC_DIRS=(Assets/Scripts Assets/Usaki Assets/PLY31 Assets/PLY32 Assets/PLY33)  # where game code lives
# Errors matching this (case-insensitive) come from third-party references that never resolve outside Unity
# (e.g. Firebase). When ONLY these remain, dotnet stopped at declaration binding and never checked method
# bodies, so the result is reported as INCONCLUSIVE (exit 2) instead of OK. Empty = disabled.
UNRESOLVABLE_REFS=""

cd "$(dirname "$0")/../.." || exit 2

if ! command -v dotnet >/dev/null 2>&1; then
  echo "check-compile: dotnet SDK not found" >&2
  exit 2
fi
if [ ! -f "$CSPROJ" ]; then
  echo "check-compile: $CSPROJ missing - open Unity once to regenerate project files" >&2
  exit 2
fi

# Unity lists every source file explicitly in the csproj, so new files are invisible to dotnet
# until Unity regenerates the project. Warn instead of silently skipping them.
missing=""
while IFS= read -r f; do
  win="${f//\//\\}"
  grep -qF "Include=\"$win\"" "$CSPROJ" || missing="$missing\n  $f"
done < <(find "${SRC_DIRS[@]}" -name '*.cs' -not -path '*/Editor/*')
if [ -n "$missing" ]; then
  printf "check-compile: WARNING - not in %s (not compiled here; refresh Unity to regenerate):%b\n" "$CSPROJ" "$missing"
fi

# Unity csproj targets net471 but DOTween.dll is built for net472; without this dotnet drops the reference.
# --no-incremental: an incremental build reported OK on edited files that no longer compiled.
out=$(dotnet build "$CSPROJ" --no-incremental -nologo -v q -clp:ErrorsOnly -p:ResolveAssemblyReferenceIgnoreTargetFrameworkAttributeVersionMismatch=true 2>&1)
errors=$(printf '%s\n' "$out" | grep -E 'error CS[0-9]+' | sed -E 's/ \[[^]]*\.csproj\]$//' | sort -u)

if [ -z "$errors" ]; then
  echo "check-compile: OK"
  exit 0
fi
if [ -n "$UNRESOLVABLE_REFS" ]; then
  own=$(printf '%s\n' "$errors" | grep -viE "$UNRESOLVABLE_REFS")
else
  own="$errors"
fi
if [ -n "$own" ]; then
  echo "check-compile: FAILED"
  printf '%s\n' "$own"
  exit 1
fi
echo "check-compile: INCONCLUSIVE - '$UNRESOLVABLE_REFS' references do not resolve outside Unity, method bodies were not checked."
echo "Verify with Unity instead (MCP: refresh, then read Console errors)."
exit 2
