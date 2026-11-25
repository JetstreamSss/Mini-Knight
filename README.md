# Mini Knight

Unity 6000.6.0f1 · 2D mobile platformer

Open `Assets/Scenes/MainMenu.unity` to start from the menu. The build scene order is MainMenu, SampleScene, level2, level3 and WinScene.

## Sound sources and licenses

All files below are in `Assets/Resources/SoundPass/`. Each is original audio generated for Mini Knight using Python waveform synthesis. The effects use tone sweeps, amplitude envelopes and procedural noise. The themes and victory cue use original note sequences rendered as plucked tones. No external samples are used. All listed audio is released under [CC0 1.0](https://creativecommons.org/publicdomain/zero/1.0/).

| File | Event or use | Source | License |
| --- | --- | --- | --- |
| `Jump.wav` | Knight jumps | Mini Knight synthesis | CC0 1.0 |
| `Land.wav` | Knight lands | Mini Knight synthesis | CC0 1.0 |
| `Swing.wav` | Knight swings sword | Mini Knight synthesis | CC0 1.0 |
| `Hit.wav` | Knight, enemy or boss takes damage | Mini Knight synthesis | CC0 1.0 |
| `Pickup.wav` | Coin collected | Mini Knight synthesis | CC0 1.0 |
| `MeleeWarning.wav` | Patroller begins wind-up | Mini Knight synthesis | CC0 1.0 |
| `ChargeWarning.wav` | Charger begins wind-up | Mini Knight synthesis | CC0 1.0 |
| `ShotWarning.wav` | Shooter begins aiming | Mini Knight synthesis | CC0 1.0 |
| `BossWarning.wav` | Boss begins charging fireball | Mini Knight synthesis | CC0 1.0 |
| `EnemySwing.wav` | Patroller swings | Mini Knight synthesis | CC0 1.0 |
| `Charge.wav` | Charger starts the dash | Mini Knight synthesis | CC0 1.0 |
| `EnemyShot.wav` | Shooter or boss releases fireball | Mini Knight synthesis | CC0 1.0 |
| `Death.wav` | Knight or enemy dies | Mini Knight synthesis | CC0 1.0 |
| `Click.wav` | UI button activates | Mini Knight synthesis | CC0 1.0 |
| `Win.wav` | Coin goal reached or final victory scene opens | Mini Knight synthesis | CC0 1.0 |
| `Lose.wav` | Knight loses a life or falls out of the level | Mini Knight synthesis | CC0 1.0 |
| `MenuTheme.wav` | Menu and final victory music | Mini Knight synthesis | CC0 1.0 |
| `LevelTheme.wav` | Gameplay music across all three levels | Mini Knight synthesis | CC0 1.0 |

## Script map

| Caller | Audio behavior |
| --- | --- |
| `PlayerController`, `PlayerAttack`, `PlayerHealth` | Jump, land, sword swing, damage, death and lose |
| `Patroller`, `Charger`, `Shooter`, `BossAttack` | One warning as the wind-up begins, then a release sound when the attack starts |
| `EnemyHealth`, `BossHealth`, `CoinManager`, `VictoryManager` | Damage, enemy death, coin and win feedback |
| `VolumeController`, `VFXVolumeController` | Restore slider values and send changes to `MusicManager` |
| `PauseMenu` | Requests the Paused and Playing snapshots |
| `MusicManager` | Loads sounds, plays UI clicks, assigns sources to mixer groups, chooses music by scene, crossfades tracks and saves volume preferences |

`MusicManager` creates one persistent audio object. It uses two looping music sources and independent SFX voices. A scene that uses the same track keeps the same source and playback position. The 0.85-second crossfade and mixer snapshot transitions use unscaled time. Repeated effects vary pitch from 0.92 to 1.08.

The mixer routes `Master → Music → Music Volume` and `Master → SFX`. The Music parent stores the snapshot attenuation and low-pass filter. Its child stores the user's Music slider gain, so both gains apply when the game is paused. The sliders use Assignment 11’s `PlayerPrefs` settings alongside the existing JSON progress save. Music keeps the original `miniknight.musicVolume` key and SFX uses `miniknight.sfxVolume`. Reset Progress restores both audio defaults. The sliders convert normalized positions with `20 × log10(max(0.0001, value))`. The bottom of the slider maps to −80 dB and the top to 0 dB. Half volume is about −6 dB.

## Phone verification

1. Start on the menu, tap Play, select an unlocked level and listen to the music transition. Restart the level and check that the music continues. Return to the menu and listen to the reverse transition.
2. Jump, land, swing, hit an enemy and collect a coin. Approach a patroller with game sound audible. The warning starts about 0.16 seconds before the red flash and about 0.5 seconds before the swing. In level2, repeat with the shooter and charger. The boss has its own warning.
3. Pause and resume. Listen for the music becoming quieter and muffled while the menu clicks stay clear.
4. Let the knight die to hear death and then lose. Collect the three required coins to hear the level win cue, then advance.
5. In Settings, put Music low and SFX high. Close the game and relaunch it. Show both saved slider positions and confirm the sound balance is still audible.
6. Open Window → Audio → Audio Mixer. Select `MiniKnight` and show Master, Music, SFX and the Paused snapshot in the screenshot.

## Suggested 2–3 minute English video

Leave a gap in the narration while each sound plays. Record the game audio and microphone.

| Time | On screen | Suggested narration |
| --- | --- | --- |
| 0:00–0:20 | Menu → Play | “This is Mini Knight. Listen as the menu theme fades out and the level theme fades in.” |
| 0:20–0:50 | Jump, land, swing, hit and pick up a coin | “The jump, landing, sword swing, hit and coin all have separate sounds. Repeated effects have a small random pitch variation.” |
| 0:50–1:15 | Approach the patroller, stop talking during its attack, then let the knight die | “Listen for the warning before the enemy flashes red and swings. Here are the death and lose cues.” |
| 1:15–1:40 | Pause and resume, restart, then show the coin-goal panel and its win cue | “Pause makes the music quieter and muffles it. The music continues from the same position when I restart a level.” |
| 1:40–2:05 | Change sliders, close the app, relaunch and show saved values | “I set Music low and SFX high. The game restores both slider positions when I reopen it.” |
| 2:05–2:35 | Mixer and script-map table | “Gameplay scripts tell MusicManager which sound to play. MusicManager routes sources through the mixer. The settings scripts save the slider values, and PauseMenu switches the mixer's snapshot.” |
