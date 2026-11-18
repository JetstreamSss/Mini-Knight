2026-11-11  Asked the agent for EnemyBase + Patroller FSM for my 2D game. My old EnemyDamage hit instantly with no wind-up, so I disabled it on the Patroller. Set Sight Mask to Ground + Player.
2026-11-11  Added a Player layer and the jump stopped working (my PlayerController checks ground by tag via collisions). Set the Player layer back to Default and changed Sight Mask to Default + Ground.
2026-11-12  Boss script is a dumb timer with no wind-up. Wrote Shooter FSM instead (Idle/Aim/Shoot/Cooldown) and reused Fireball.Launch(dir). Copied the fireball prefab so the boss stays untouched.
2026-11-12  Charger: Idle/WindUp/Charge/Recover. Set View Angle to 180 on Shooter and Charger because they did not see the player when facing the other way.
