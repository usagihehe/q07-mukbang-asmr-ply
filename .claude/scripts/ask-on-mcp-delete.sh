#!/usr/bin/env bash
# PreToolUse hook for UnityMCP manage_asset/manage_gameobject/manage_components.
# Permission rules can't match MCP arguments, so ask here only for destructive actions.

input=$(cat)
printf '%s' "$input" | grep -qiE '"action" *: *"(delete|remove)"' || exit 0

printf '%s\n' '{"hookSpecificOutput":{"hookEventName":"PreToolUse","permissionDecision":"ask","permissionDecisionReason":"UnityMCP delete/remove action - confirm before running"}}'
exit 0
