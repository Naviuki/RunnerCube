# Runner Cube

[English version](README.en.md)

Псевдо-3D раннер: игрок бежит по трём полосам, прыгает и подкатывается под брёвна, собирает монеты, щит и звезду. Есть меню со скинами, настройками, выбором языка и рекордом.

## Управление

- A или стрелка влево, свайп влево: полоса влево
- D или стрелка вправо, свайп вправо: полоса вправо
- W, стрелка вверх или пробел, свайп вверх: прыжок
- S или стрелка вниз, свайп вниз: подкат
- Esc: пауза во время забега

## Архитектура

Код разделён по слоям, каждый модуль в своём namespace:

- `RunnerCube.Core`: состояние забега (`GameLoop`), счёт (`ScoreModel`), прогресс (`PlayerProgress`), события для UI и звука (`GameEvents`), переходы между сценами (`SceneFlow`), сервис сцены (`SceneServices`).
- `RunnerCube.Data`: настройки в ScriptableObject (`RunSettings`, `TrackSettings`, `SkinCatalog`, `Localization`), типы предметов и их правила.
- `RunnerCube.Player`: ввод (`InputReader`), движение и позы (`PlayerMotor`, `PlayerState`, `PlayerBody`, `PlayerView`), бонусы (`PlayerBonuses`), столкновения (`PlayerContact`), эффекты (`GameEffects`).
- `RunnerCube.Track`: полосы, плитки дороги, расстановка рядов предметов, фабрика (`TrackFactory`).
- `RunnerCube.Pooling`: пул объектов и реестр пулов (`ObjectPool`, `PoolRegistry`).
- `RunnerCube.UI`: тонкие представления над логикой (HUD, меню, магазин, панели).
- `RunnerCube.Lifecycle`: запуск сцены.

Паттерны:

- State: `GameLoop` описывает состояния меню, игра, пауза, финал и разрешает только осмысленные переходы. `PlayerState` следит за позой и таймером подката.
- Observer: все изменения счёта, монет, бонусов и смерти идут через `GameEvents`, UI и звук подписываются на них.
- Object Pool: плитки дороги и предметы берутся из `ObjectPool` через `TrackFactory`, минуя Instantiate и Destroy в кадре.
- Factory/Spawner: `RowBuilder` собирает ряд, `TrackSpawner` выставляет его в мир.
- Dependency injection через инспектор: компоненты получают настройки и соседние системы через `[SerializeField]`-ссылки, интерфейсами в местах, где это имеет смысл (`InputReader` не знает, кто обрабатывает действие).

Настройки скорости, прыжка, шансов спавна и таймеров лежат в ассетах в `Assets/Scripts/Data/Settings`, их можно менять без пересборки кода.

## Структура папок

```
Assets/
  Scripts/
    Core/        состояние, счёт, прогресс, события, переходы сцен
    Data/        ScriptableObject-настройки и типы данных
    Player/      ввод, движение, бонусы, столкновения, эффекты
    Track/       полосы, спавн плиток и предметов, фабрика
    Pooling/     пул и реестр объектов
    UI/          HUD, меню, магазин, кнопки
    Lifecycle/   старт сцены
  Editor/        проверки сцен и логики, сборка
  Prefabs/       предметы, монеты, бонусы
  Tiles/         плитки дороги
  Particles/     эффекты
  _Scenes/       Menu.unity и Game.unity
```

## Запуск и сборка

Проект рассчитан на Unity 6000.5.1f1 (в `ProjectSettings/ProjectVersion.txt` записана эта версия).

1. Откройте папку проекта через Unity Hub.
2. Стартовая сцена: `Assets/_Scenes/Menu.unity`.
3. Сборка: меню `Tools > Runner Cube > Build Windows` или `File > Build Settings`. В Build Settings уже лежат обе сцены в нужном порядке.

## Проверки

В `Assets/Editor` лежат простые проверки, которые запускаются из command line:

```
Unity.exe -batchmode -projectPath <путь> -executeMethod RunnerCube.Tests.LogicChecks.Run -logFile logic.log
Unity.exe -batchmode -projectPath <путь> -executeMethod RunnerCube.Tests.SceneChecks.Run -logFile scene.log
Unity.exe -batchmode -projectPath <путь> -executeMethod RunnerCube.Tests.LinkChecks.Run -logFile link.log
Unity.exe -batchmode -projectPath <путь> -executeMethod RunnerCube.Tests.DataChecks.Run -logFile data.log
```

`LogicChecks` проверяет состояния, счёт, полосы и правила предметов, `SceneChecks` — собранные компоненты в обеих сценах, `LinkChecks` — заполненные ссылки и отсутствие пустых событий у кнопок, `DataChecks` — загрузку настроек.

Так же запускаются `RowChecks`, `RefChecks`, `NullChecks`, `AssetChecks`, `ButtonChecks`, `PanelChecks`, `MenuButtonChecks`, `PrefabChecks`, `ShopPanelChecks` и `MenuNavChecks`: они проверяют ряды предметов, ссылки в префабах и сценах, отсутствие пустых обработчиков, панели меню, магазин и навигацию.
