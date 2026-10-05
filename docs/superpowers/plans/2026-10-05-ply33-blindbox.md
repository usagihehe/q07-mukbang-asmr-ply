# PLY33 Blindbox — plan

Spec: `docs/superpowers/specs/2026-10-05-ply33-blindbox-design.md`

| # | Role | Task | Verify |
|---|---|---|---|
| 1 | Lead | Rewrite stubs + add new classes in `PLY33.Blindbox` (keep `.meta` guids) | `check-compile.sh` exit 0 |
| 2 | Lead | `refresh_unity(scope=all)`, read console | 0 errors; scene components resolve |
| 3 | Lead | Record field values of components to replace (SupermarketPanel, SupermarketItemMukbang ×5, ItemSpmkIconCtrl ×5) | values listed |
| 4 | Lead | Wire PLY33 via MCP (remove Ply32DirectEatFlow, swap components, add Spine balls, set fields), save scene | hierarchy/components read back |
| 5 | Reviewer | Diff review vs spec | findings list |
| 6 | Lead | Fix confirmed findings | compile clean |
| 7 | Verifier | Play Mode probe: gacha 3 → Done → open → eat; then delete probe | probe log, console clean |
| 8 | Lead | `git status`: only PLY33 scene + PLY33 scripts + docs changed; update CLAUDE.md pitfalls; commit code+docs | git output |
