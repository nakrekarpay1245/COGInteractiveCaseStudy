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

## How to Create Levels

### Method 1: Using the Visual Editor (Recommended)

1. **Create a New Level:**
   - Navigate to `Assets/_Game/Resources/_Data/Levels/`
   - Right-click → `Create > Game > Level Data`
   - Name it following the pattern: `LevelData_XX` (e.g., `LevelData_02`)

2. **Configure Level Settings:**
   - **Grid Size:** Set the grid dimensions (e.g., 5x5, 7x7)
   - **Ball Speed:** Adjust movement speed (default: 5.0)
   - **Level Color:** Choose from predefined color types

3. **Design the Level:**
   - Click the **●** button to enter Ball placement mode
   - Click the **■** button to enter Obstacle placement mode
   - Click on grid tiles to place/remove elements
   - Drag across tiles for quick placement
   - Use **←** and **→** buttons for undo/redo
   - Click **Reset to Default** to restore base configuration

4. **Add to Level List:**
   - Open the main scene
   - Select `LevelManager` GameObject
   - Add your new level to the `Level List` array

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
Access via Unity Toolbar for quick level switching during development.

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

## Testing Levels

1. Select `LevelManager` in the scene
2. Set `Editor Start Level Index` to your level's index
3. Enter Play mode
4. Use toolbar buttons to switch between levels

## Performance Considerations

- Grid pathfinding is optimized with aggressive inlining
- DOTween animations use SetAutoKill and SetLink
- Object pooling for particles and audio
- Efficient tile lookup with 2D array structure

---

**Note:** All level data is stored in `Assets/_Game/Resources/_Data/Levels/` and must be placed there to be loaded at runtime.
