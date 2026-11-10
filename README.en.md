# Runner Cube

[Русская версия](README.md)

A pseudo-3D runner: you run along three lanes, jump and slide under logs, and pick up coins, a shield and a star. The menu holds skins, settings, language choice and your record.

## Controls

- A or Left arrow, swipe left: lane left
- D or Right arrow, swipe right: lane right
- W, Up arrow or Space, swipe up: jump
- S or Down arrow, swipe down: slide
- Esc: pause while running

## Architecture

The code is split into layers, each one in its own namespace:

- `RunnerCube.Core`: run state (`GameLoop`), score (`ScoreModel`), progress (`PlayerProgress`), events for UI and audio (`GameEvents`), scene transitions (`SceneFlow`), the per-scene service (`SceneServices`).
- `RunnerCube.Data`: ScriptableObject settings (`RunSettings`, `TrackSettings`, `SkinCatalog`, `Localization`), item kinds and their rules.
- `RunnerCube.Player`: input (`InputReader`), movement and poses (`PlayerMotor`, `PlayerState`, `PlayerBody`, `PlayerView`), bonuses (`PlayerBonuses`), collisions (`PlayerContact`), effects (`GameEffects`).
- `RunnerCube.Track`: lanes, road tiles, row layout.
- `RunnerCube.Pooling`: object pool and factory (`ObjectPool`, `PoolRegistry`, `TrackFactory`).
- `RunnerCube.UI`: thin views on top of the logic (HUD, menu, shop, panels).
- `RunnerCube.Lifecycle`: scene start-up.

Patterns:

- State: `GameLoop` models the menu, playing, paused and game over states and allows only sensible transitions. `PlayerState` tracks the pose and the slide timer.
- Observer: score, coins, bonuses and death are published through `GameEvents`; UI and audio subscribe to them.
- Object Pool: road tiles and items come from `ObjectPool` via `TrackFactory`, so there is no Instantiate/Destroy churn every frame.
- Factory/Spawner: `RowBuilder` picks the row, `TrackSpawner` places it into the world.
- Injection through the inspector: components receive settings and neighbours through `[SerializeField]` references, with interfaces where they earn their keep (`InputReader` does not know who handles the action).

Speed, jump, spawn rates and timers live in the assets under `Assets/Scripts/Data/Settings` and can be tuned without touching code.

## Folder layout

```
Assets/
  Scripts/
    Core/        state, score, progress, events, scene transitions
    Data/        ScriptableObject settings and data kinds
    Player/      input, movement, bonuses, collisions, effects
    Track/       lanes, tile and item spawning
    Pooling/     object pool and factory
    UI/          HUD, menu, shop, buttons
    Lifecycle/   scene start-up
  Editor/        scene and logic checks, build
  Prefabs/       items, coins, bonuses
  Tiles/         road tiles
  Particles/     effects
  _Scenes/       Menu.unity and Game.unity
```

## Running and building

The project targets Unity 6000.5.1f1 (that is the version in `ProjectSettings/ProjectVersion.txt`).

1. Open the project folder through Unity Hub.
2. Start scene: `Assets/_Scenes/Menu.unity`.
3. Build: `Tools > Runner Cube > Build Windows`, or `File > Build Settings`. Both scenes are already listed there in the right order.

## Checks

`Assets/Editor` has small checks that can be run from the command line:

```
Unity.exe -batchmode -projectPath <path> -executeMethod RunnerCube.Tests.LogicChecks.Run -logFile logic.log
Unity.exe -batchmode -projectPath <path> -executeMethod RunnerCube.Tests.SceneChecks.Run -logFile scene.log
Unity.exe -batchmode -projectPath <path> -executeMethod RunnerCube.Tests.LinkChecks.Run -logFile link.log
Unity.exe -batchmode -projectPath <path> -executeMethod RunnerCube.Tests.DataChecks.Run -logFile data.log
```

`LogicChecks` covers states, scoring, lanes and item rules, `SceneChecks` looks at the assembled components in both scenes, `LinkChecks` verifies that references are filled and no button event is empty, and `DataChecks` loads the settings assets.
