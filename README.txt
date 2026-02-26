# COG Interactive Case Study

A 2D grid-based puzzle game built with Unity 2022.3.62f2 featuring dynamic pathfinding, tile painting mechanics, and a custom level editor.

## Tech Stack

- **Unity Version:** 2022.3.62f2
- **Render Pipeline:** Universal Render Pipeline (URP) 14.0.12
- **Architecture:** SOLID principles with modular system design
- **Key Packages:**
  - DOTween (Animation)
  - Tri-Inspector (Enhanced Inspector)
  - Unity Toolbar Extender (Editor Tools)

## Project Structure

```
Assets/_Game/
├── Scripts/
│   ├── Audio/              # Audio management system
│   ├── BallSystem/         # Ball movement and pathfinding
│   ├── ColorSystem/        # Color management for tiles
│   ├── Core/               # Singleton base classes
│   ├── Editor/             # Custom editor tools
│   ├── Input/              # Input handling
│   ├── LevelSystem/        # Level data and management
│   ├── ObstacleSystem/     # Obstacle placement
│   ├── Particle/           # Particle effects
│   ├── ProgressionSystem/  # Level progression tracking
│   ├── SaveSystem/         # Save/load functionality
│   ├── TileGridSystem/     # Grid and tile management
│   ├── UI/                 # User interface
│   └── Utilities/          # Helper classes
├── Resources/
│   └── _Data/
│       └── Levels/         # Level ScriptableObjects
└── Scenes/                 # Game scenes
```

## Core Systems

### Grid System
- Dynamic grid generation based on level data
- Tile-based pathfinding with A* algorithm
- Automatic tile painting on ball movement
- Obstacle detection and collision handling

### Ball System
- Smooth DOTween-based movement
- Automatic pathfinding to grid boundaries
- Multi-ball support with collision avoidance
- Visual feedback with scale animations

### Level System
- ScriptableObject-based level configuration
- Custom visual editor for level design
- Undo/redo support in editor
- Drag-and-drop tile placement

## Level Editor Guide

### Opening the Level Editor

**Option 1: Create New Level**
1. Navigate to `Assets/_Game/Resources/_Data/Levels/`
2. Right-click → `Create > Game > Level Data`
3. Name it: `LevelData_XX` (e.g., `LevelData_02`)
4. Select the created asset to open the editor in Inspector

**Option 2: Edit Existing Level**
1. Navigate to `Assets/_Game/Resources/_Data/Levels/`
2. Click on any existing `LevelData_XX` asset
3. The visual editor appears in the Inspector panel

### Using the Level Editor

**Basic Configuration:**
- **Grid Size:** Set width and height (e.g., 5x5, 7x7)
- **Ball Speed:** Movement speed (default: 5.0)
- **Level Color:** Choose from predefined color palette
- **Camera Ortho Size:** Adjust camera zoom for different grid sizes
- **Floor Size:** Set floor dimensions for visual appearance

**Visual Grid Editor:**
- **● Button:** Ball placement mode (click tiles to add/remove balls)
- **■ Button:** Obstacle placement mode (click tiles to add/remove obstacles)
- **Click & Drag:** Quick placement across multiple tiles
- **← Button:** Undo last action
- **→ Button:** Redo last undone action
- **Reset to Default:** Clear all placements and restore base settings
- **Check Solvability:** Verify if the level can be completed (all tiles painted)

**Adding Level to Game:**
1. Open `Assets/_Game/Scenes/MainScene`
2. Select `LevelManager` GameObject in Hierarchy
3. In Inspector, add your level to the `Level List` array
4. Set `Editor Start Level Index` to test your level

### Method 2: Manual Configuration

Edit the ScriptableObject directly in the Inspector:

```
Level Configuration:
├── Grid Size: Vector2Int (width, height)
├── Ball Speed: Float (movement speed)
└── Level Color: ColorType enum

Positions:
├── Ball Positions: List<Vector2Int>
└── Obstacle Positions: List<Vector2Int>
```

**Example Configuration:**
- Grid Size: (5, 5)
- Ball Positions: [(0,0), (4,4)]
- Obstacle Positions: [(2,2), (2,3)]

### Level Design Guidelines

**Grid Size:**
- Minimum: 2x2
- Recommended: 4x4 to 8x8
- Maximum: Limited by performance

**Ball Placement:**
- Place at least one ball per level
- Avoid placing balls on obstacle positions
- Consider symmetrical placement for aesthetic appeal

**Obstacle Placement:**
- Use obstacles to create interesting paths
- Avoid blocking all possible paths
- Test pathfinding to ensure level is solvable

**Difficulty Progression:**
- Early levels: Small grids (3x3 to 5x5), few obstacles
- Mid levels: Medium grids (5x5 to 7x7), strategic obstacles
- Late levels: Large grids (7x7+), complex obstacle patterns

## Editor Tools

### Level Manager Window
- Access via `Window > Level Manager` in Unity top menu
- View all levels in a list with quick selection
- Create new levels with auto-incrementing names
- Duplicate existing levels
- Delete levels with confirmation
- Reset selected level to default configuration
- Check solvability of selected level
- Clear PlayerPrefs for testing
- Full visual editor embedded in the window

### Level Selector Toolbar
Switch between levels in Play mode for rapid testing.

### Time Scale Toolbar
Adjust game speed for debugging and testing.

## Development Guidelines

All code follows these principles:
- **SOLID** architecture for maintainability
- **OOP** best practices with encapsulation
- Private fields use `_camelCase` naming
- Public properties use `PascalCase` with getters/setters
- Events/Actions for decoupled communication
- Minimal comments, self-documenting code
- English-only code and comments

## Getting Started

1. Clone the repository
2. Open project in Unity 2022.3.62f2
3. Open `Assets/_Game/Scenes/MainScene`
4. Press Play to test existing levels
5. Create new levels using the visual editor

## Special Controls & Features

### In-Game Controls
- **Mouse Click:** Select and move balls
- **ESC:** Pause menu / Return to main menu

### Editor Toolbar Features

**Level Manager Window:**
- Access via Unity top toolbar
- Quick level switching during development
- Jump to any level instantly

**Level Selector Toolbar:**
- Available in Play mode
- Switch between levels without stopping playback
- Rapid iteration testing

**Time Scale Toolbar:**
- Adjust game speed (0.5x, 1x, 2x, 3x)
- Debug slow-motion or fast-forward
- Test timing-sensitive mechanics

### Testing Levels

1. Select `LevelManager` in the scene
2. Set `Editor Start Level Index` to your level's index
3. Enter Play mode
4. Use toolbar buttons to switch between levels
5. Adjust time scale for detailed testing

## Performance Considerations

- Grid pathfinding is optimized with aggressive inlining
- DOTween animations use SetAutoKill and SetLink
- Object pooling for particles and audio
- Efficient tile lookup with 2D array structure

## Known Issues & Limitations

### Current Limitations
- Maximum grid size limited by screen resolution and performance
- Level editor undo/redo has a fixed history size
- No runtime level validation (ensure paths are not completely blocked)

### Incomplete Features
- No in-game level editor (editor-only functionality)
- No automatic pathfinding validation in editor
- No level difficulty rating system

### Workarounds
- Always test levels in Play mode to verify solvability
- Use smaller grid sizes (4x4 to 8x8) for optimal performance
- Manually verify that balls have valid paths to grid edges

## Development Time

**Total Time Spent:** ~40-50 hours

**Breakdown:**
- Core Systems (Grid, Ball, Pathfinding): ~15 hours
- Level System & Editor: ~10 hours
- UI & Progression: ~8 hours
- Audio & Particles: ~5 hours
- Polish & Optimization: ~7 hours
- Documentation: ~3 hours

---

**Note:** All level data is stored in `Assets/_Game/Resources/_Data/Levels/` and must be placed there to be loaded at runtime.
