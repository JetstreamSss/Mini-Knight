2026-11-11  Asked the agent for EnemyBase + Patroller FSM for my 2D game. My old EnemyDamage hit instantly with no wind-up, so I disabled it on the Patroller. Set Sight Mask to Ground + Player.
2026-11-11  Added a Player layer and the jump stopped working (my PlayerController checks ground by tag via collisions). Set the Player layer back to Default and changed Sight Mask to Default + Ground.
2026-11-12  Boss script is a dumb timer with no wind-up. Wrote Shooter FSM instead (Idle/Aim/Shoot/Cooldown) and reused Fireball.Launch(dir). Copied the fireball prefab so the boss stays untouched.
2026-11-12  Charger: Idle/WindUp/Charge/Recover. Set View Angle to 180 on Shooter and Charger because they did not see the player when facing the other way.

## Week 13 · Sound Pass

- 2026-11-24 — Asked Codex to add the Sound Pass to Mini Knight and keep the work in Git commits.
- 2026-11-24 — The old menu player persisted a single track and the SFX slider changed the listener volume. Codex replaced this with two mixer-routed music sources and separate saved Music and SFX gains.
- 2026-11-24 — The boss fired immediately. Codex added a wind-up and placed the warning at its start, ahead of the attack animation and projectile.
- 2026-11-24 — Codex separated the Music slider gain from the pause snapshot gain so pausing can lower and filter music while preserving the player's slider value.
