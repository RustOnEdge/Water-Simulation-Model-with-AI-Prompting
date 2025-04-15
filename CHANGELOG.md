# Changelog

All notable changes to this project will be documented in this file.

## v0.3.1 (15/04/2025) - Current
- Switched from continuous collision detection (CCD) to discrete collision detection for improved stability
- Fine-tuned SPH parameters for more realistic water behavior:
  - Adjusted viscosity for more fluid movement (0.1 → 0.01)
  - Modified gas constant for softer water interaction (2000 → 1000)
  - Improved damping coefficients for better energy conservation
- Enhanced visualization system:
  - Density-based color gradients
  - Improved particle rendering
  - Better visual feedback for fluid compression
- Current development focus:
  - Ongoing parameter optimization for SPH simulation
  - Testing different combinations of fluid properties
  - Fine-tuning visual representation of water density

## v0.3.0 (14/04/2025)
- Added continuous collision detection (CCD)
- Major improvements to boundary handling
- Enhanced particle stability
- Fixed particle escape issues
- Optimized neighbor search
- Expanded debug visualization

## v0.2.0 (13/04/2025)
- Added spatial partitioning system for efficient collision detection
- Enhanced container system with:
  - Improved boundary collision handling
  - Position correction
  - Smooth particle interaction
- Expanded UI controls
- Performance optimizations
- Refined SPH implementation

## v0.1.0 (12/04/2025)
- Initial implementation of 2D particle-based water simulation
- Basic particle physics with:
  - Gravity
  - Collision response
  - Velocity damping
- Simple container system
- Basic UI controls
- First implementation of SPH
- Debug visualization features

Note: Version 1.0.0 will be released when all planned core features are implemented and thoroughly tested. 