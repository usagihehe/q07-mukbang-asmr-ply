#!/usr/bin/env bash
# Stop hook: when C# files changed, refuse to finish while check-compile reports errors.
# Exit 2 sends stderr back to Claude so it keeps working; anything else lets it stop.

input=$(cat)
# Already continuing because of this hook once - don't loop forever.
printf '%s' "$input" | grep -q '"stop_hook_active": *true' && exit 0

cd "$(dirname "$0")/../.." || exit 0
git status --porcelain -- Assets | grep -q '\.cs"\?$' || exit 0

result=$(bash .claude/scripts/check-compile.sh 2>&1)
if [ $? -eq 1 ]; then
  printf 'C# compile errors in the working tree - fix them before finishing:\n%s\n' "$result" >&2
  exit 2
fi
exit 0
