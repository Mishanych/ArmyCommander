# Army Commander — Test Task

This repository contains my implementation of the core loop from **Army Commander**. It was created as a technical test, with the focus deliberately placed on code structure, configurability, and maintainability rather than final art polish.

## Implemented loop

1. The player moves with the on-screen joystick and is followed by a smooth camera.
2. Enemy groups spawn into the level.
3. The player commands an initial squad and can build a barracks to reinforce it.
4. Units acquire targets and use ranged combat.
5. Defeated units drop currency pickups.
6. Pickups are collected into a visible stack, then spent in zones to build the barracks and upgrade the player's shield.
7. Player death and the kill zone display a retry result popup.

## Running the project

- Open the project with **Unity 6.2** (`6000.2.2f1`).
- Ensure the project dependencies are available: **Zenject**, **UniTask**, **DOTween**, TextMeshPro, AI Navigation, and the Unity Input System.
- Open `Assets/_Project/Scenes/GameScene.unity` and press Play.

The runtime composition root is `Infrastructure/Scripts/GameInstaller.cs`. Scene references such as the player, camera, UI, and currency prefabs are intentionally configured through its inspector fields.

## Architecture

The code is organised by feature rather than by Unity component type:

```text
_Project/
├── Core/             Shared contracts, enums, and input abstraction
├── Infrastructure/   Zenject composition root and application startup
├── Modules/
│   ├── Units/        Units, combat, projectiles, spawning, commands
│   ├── Player/       Movement, health, collection, player composition
│   ├── Economy/      Currency ledger, pickups, visual stack, spending UI
│   ├── Buildings/    Building activation and barracks spawning
│   ├── Shield/       Configurable shield upgrades
│   ├── Level/        Level setup and result flow
│   ├── Common/       Reusable zones and pooled-factory base
│   └── Effects/, Animations/, Camera/, Popups/, Stacking/
└── Scenes/           Game scene and baked navigation data
```

### Main design choices

- **Dependency injection:** Zenject binds scene services and runtime services in one place. Gameplay code depends on contracts such as `IInputService`, `IUnitManager`, `IAttacker`, `IDamageable`, and `ICollectible` instead of finding global objects.
- **Data-driven tuning:** `UnitConfig`, `BarracksConfig`, `ShieldConfig`, and `SpendingZoneConfig` are ScriptableObjects, so designers can tune costs, stats, drops, and upgrade levels without changing code.
- **Feature modules:** Combat, economy, progression, player control, and presentation are separated into focused folders and namespaces.
- **Object reuse:** Units and projectiles use a reusable factory/pool abstraction; money pickups use Zenject memory pools.
- **Composable world interactions:** `BaseZone` supplies player-presence behaviour, while `ActionZone` and `SpendingZone` specialise it. Buildings initialise injected `IBuildingFunction` implementations when construction completes.
- **Presentation separated from state:** examples include `MoneyVisualizer`, `CharacterAnimation`, `HitFlash`, and UI views listening to service/zone events.

## Key extension points

| Requirement | Where to extend it |
| --- | --- |
| Add a unit or alter combat stats | Create/edit a `UnitConfig` asset |
| Add a currency type | Extend `CurrencyType`, configure a pool, and add its UI/prefab mapping |
| Add a constructed-building behaviour | Implement `IBuildingFunction` and configure it on the building |
| Add progression upgrades | Add levels to `ShieldConfig` or create another `SpendingZone` consumer |
| Change pickup stacking rules | Implement `IStackingStrategy` and provide it to `StackController` |

## Scope

This is a gameplay/architecture prototype, not a production-complete clone. The visual presentation is intentionally lightweight. The structure was designed to make it straightforward to replace assets, tune the economy, introduce more unit types, or add additional levels without entangling core gameplay systems.

## Third-party assets and packages

The project uses third-party Unity packages/assets for runtime support and prototype presentation. Review their respective licences before redistributing the repository or using it commercially.

