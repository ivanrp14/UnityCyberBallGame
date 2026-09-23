# UnityCyberBallGame

An action game in Unity. The player controls **Roboto**: it walks, jumps, can switch into a faster ball mode, and has a dash. The level has shooting enemies, doors, buttons, and buildings that can be broken.

The repository versions the `Assets` folder (scripts and whatever else hangs off it), not a ready-to-ship package.

## Project scripts

```
Assets/Scripts/
├── Player/          RobotoController, PlayerInputManager
├── Hability/        Dash and abilities
├── Enemies/         Warrior, shots, and a bullet pool
├── Environment/     Door, Button, Building, and mesh destruction
└── Systems/         Health, Damage, Destroy, ObjectPool
```

`RobotoController` uses `CharacterController`. It separates on-foot speed (`moveSpeed`) and ball-mode speed (`ballSpeed`), and applies gravity and a jump.

## Stack

- Unity
- C#

## How to open it

1. Create an empty Unity project, or open an existing one.
2. Copy this repo's `Assets` over the project's `Assets` (or clone it and add a `ProjectSettings` folder from Unity Hub with **Add project from disk** if your local copy already has one).
3. Open the game scene and check that the player has `RobotoController` and `PlayerInputManager`.

If a clone only shows `Assets`, Unity Hub can create project settings the first time you open it. Pick a recent Unity 6 or 2022 LTS, the same line the scenes were saved with if Unity warns about an upgrade.
