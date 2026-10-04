#!/usr/bin/env bash
# Delete Claude's temporary files — and nothing else.
#   usage: bash .claude/scripts/rm-temp.sh <path>...
# A path is deleted only if, after resolving "..", symlinks and the working directory,
# it lies strictly inside one of the temp zones below AND git tracks nothing at or under it.
# A Unity ".meta" sidecar next to a deleted path is removed too. Exit 1 if any path was refused.
set -u

root="$(cd "$(dirname "$0")/../.." && pwd -P)"
zones=("$(realpath -m -- "$root/Assets/_ClaudeTemp")" "$(realpath -m -- "$root/Temp/claude")")

[ $# -ge 1 ] || { echo "usage: rm-temp.sh <path>..." >&2; exit 2; }

status=0
for arg in "$@"; do
  p="$(realpath -m -- "$arg")"

  inside=0
  for z in "${zones[@]}"; do
    case "$p" in "$z"/?*) inside=1 ;; esac
  done
  if [ "$inside" -ne 1 ]; then
    echo "REFUSED (outside temp zones): $arg" >&2; status=1; continue
  fi
  if [ ! -e "$p" ]; then
    echo "skip (not found): $arg"; continue
  fi
  if [ -n "$(git -C "$root" ls-files -- "$p" 2>/dev/null)" ]; then
    echo "REFUSED (tracked by git): $arg" >&2; status=1; continue
  fi

  rm -rf -- "$p" && echo "deleted: $arg"
  [ -e "$p.meta" ] && rm -f -- "$p.meta" && echo "deleted: $arg.meta"
done
exit $status
