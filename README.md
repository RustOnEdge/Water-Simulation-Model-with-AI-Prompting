<div align="center">

# 🌊 Water Simulation Model with AI Prompting

</div>

<div align="center">

![Unity Version](https://img.shields.io/badge/Unity-2022.3%2B-blue.svg)
![Status](https://img.shields.io/badge/status-v0.3.2-success.svg)
![License](https://img.shields.io/badge/license-MIT-green.svg)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](http://makeapullrequest.com)

An innovative fluid simulation framework that combines particle physics with AI-powered natural language control. Control your water simulation through intuitive text prompts to create and manipulate complex fluid behaviors in real-time.

[Features](#features) • [Requirements](#requirements) • [Setup](#setup) • [Documentation](#documentation) • [Roadmap](#roadmap)

<br>

**[Check out the demo video and latest builds →](https://github.com/RustOnEdge/Water-Simulation-Model-with-AI-Prompting/releases)**

</div>

## 🚀 Setup

1. Clone this repository
```bash
git clone https://github.com/RustOnEdge/Water-Simulation-Model-with-AI-Prompting.git

# NOTE: Development is still in progress.
```

2. Open the project in Unity
3. Open the main scene in `Assets/Scenes`
4. Press Play to start the simulation

### Key Features at a Glance
- Real-time particle-based fluid simulation
- AI-powered parameter optimization
- Interactive parameter controls
- Advanced debug visualization

### Latest Updates
- New Debug Manager with real-time visualization controls
- Simulation speed control (0.1x - 4.0x)
- Multiple visualization layers added
- Performance monitoring (FPS, particle count)
- Known issues documented for future fixes

---

<details>
<summary><h2 id="features">✨ Current Features</h2></summary>

**Core Mechanics**
- [x] Particle-based fluid simulation
- [x] Spatial partitioning for efficient neighbor search
- [x] Particle-particle collision handling
- [x] Container boundaries with continuous collision detection
- [x] Debug visualization tools
- [x] Adjustable simulation parameters
- [x] Enhanced particle stability
- [x] Optimized neighbor search

**AI Integration**
- [ ] AI-powered parameter optimization
- [ ] Natural language processing for simulation control
- [ ] Real-time feedback and adjustments
- [ ] Adaptive learning for improved simulation quality

**User Interface**
- [x] Interactive parameter controls
- [x] Real-time visualization
- [x] Debug tools and statistics
- [x] Performance monitoring
</details>

<details>
<summary><h2 id="requirements">📋 Requirements</h2></summary>

### Software
- Unity 2022.3 or higher
- C# development environment

### Knowledge Base
- Particle physics
- Fluid dynamics
- Spatial partitioning
- Unity development
</details>

<details>
<summary><h2 id="documentation">📚 Documentation</h2></summary>

### Components

<details>
<summary><b>Container</b></summary>

- Adjustable width and length
- Boundary collision detection and response
- Visual debugging with Gizmos
- Rotation support
- Local space transformation
</details>

<details>
<summary><b>Particle</b></summary>

- Physics properties (mass, radius, velocity)
- Force accumulation
- Semi-implicit Euler integration
- Density and pressure calculations
- Collision response
</details>

<details>
<summary><b>Simulation</b></summary>

- Particle spawning and lifecycle management
- Spatial partitioning for efficient neighbor search
- Discrete collision detection
- SPH fluid dynamics
- Debug visualization options
</details>

### Parameters

<details>
<summary><b>Core Parameters</b></summary>

| Parameter | Description | Current Value |
|-----------|-------------|---------------|
| Particle Radius | Radius of each particle | 0.12 |
| Particle Mass | Mass of each particle | 1.0 |
| Rest Density | Target density for the fluid | 1000 |
| Gas Constant | Pressure calculation constant | 1000 |
| Viscosity | Fluid viscosity coefficient | 0.01 |
| Kernel Radius | Smoothing kernel radius | 0.3 |
</details>

<details>
<summary><b>Advanced Parameters</b></summary>

| Parameter | Description | Current Value |
|-----------|-------------|---------------|
| Damping | Velocity damping coefficient | 0.995 |
| Bounce Coefficient | Collision response factor | 0.3 |
| Position Smoothing | Position correction factor | 0.5 |
| Spawn Rate | Particles spawned per second | 100 |
| Max Particles | Maximum particle count | 500 |
| Spawn Radius | Particle spawn area radius | 1.0 |
</details>
</details>

<details>
<summary><h2 id="roadmap">🗺️ Roadmap</h2></summary>

### 2D Implementation
- [x] Basic particle system
- [x] Spatial partitioning
- [x] Continuous collision detection
- [x] Container boundaries
- [x] Debug visualization
- [x] Particle stability improvements
- [ ] Pressure forces
- [ ] Surface tension
- [ ] Temperature effects
- [ ] Wave generation
- [ ] Multiple fluid types
- [ ] Viscosity simulation

### 3D Implementation
- [ ] 3D particle system
- [ ] Volumetric fluid rendering
- [ ] 3D container physics
- [ ] Buoyancy forces
- [ ] Fluid-solid interaction
- [ ] Splash and spray effects
- [ ] Dynamic mesh generation

### Optimizations
- [x] Spatial partitioning (O(n) neighbor search)
- [x] Efficient collision detection
- [x] Particle stability improvements
- [ ] GPU acceleration
- [ ] Multi-threading
- [ ] Dynamic particle resolution
- [ ] Adaptive time-stepping
- [ ] Memory pooling
- [ ] LOD system for particles

### AI Integration (Llama 3)
- [ ] Intelligent particle behavior
- [ ] Dynamic parameter optimization
- [ ] Real-time fluid property prediction
- [ ] Adaptive simulation settings
- [ ] Smart boundary handling
- [ ] Pattern recognition in fluid dynamics
- [ ] ML-based performance optimization

### Additional Features
- [ ] Real-time fluid analysis
- [ ] VFX integration
- [ ] Interactive fluid manipulation
- [ ] Physics-based sound generation
- [ ] Fluid-environment interaction
- [ ] Custom shader effects
- [ ] Advanced visualization tools
</details>

<details>
<summary><h2 id="version-history">📝 Version History</h2></summary>

Latest Release - v0.3.2 (15/04/2025)
- Added comprehensive Debug Manager implementation
- Introduced simulation speed control with 2.0x default
- Enhanced visualization system with multiple debug layers
- Added performance monitoring features
- Known issues documented for future improvements

Previous Releases
- v0.3.1 - SPH parameter optimization and visualization update
- v0.3.0 - Added collision detection and stability improvements
- v0.2.0 - Implemented spatial partitioning and enhanced container system
- v0.1.0 - Initial release with basic particle simulation

See [Version_History.md](Version_History.md) for the complete version history.
</details>

<details>
<summary><h2 id="license">⚖️ License</h2></summary>

```text
MIT License

Copyright © 2025 Kent L.

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, publish, distribute, 
sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is 
```
</details>

<details>
<summary><h2 id="acknowledgments">👏 Acknowledgments</h2></summary>

### Core Inspirations
- Particle physics simulation techniques
- Fluid dynamics research papers
- SPH algorithm based on Müller et al. (2003)

### Tools & Technologies
- Unity Game Engine
- Visual Studio Code
- GitHub for version control

### Community
- SPH research community
- Unity Forums contributors
- Open source developers

### Visual Design
- Scientific computing visualizations
- Natural water phenomena
- Debug visualization tools

</details>

---

## 🔮 Coming Soon

- GPU Acceleration
- Multi-threading Support
- Advanced Fluid Effects
- AI-Driven Optimization

---

<div align="center">

[Latest Release](https://github.com/RustOnEdge/Water-Simulation-Model-with-AI-Prompting/releases) | [Report Issues](https://github.com/RustOnEdge/Water-Simulation-Model-with-AI-Prompting/issues)

Made by Kent L.

[![GitHub](https://img.shields.io/badge/GitHub-RustOnEdge-181717?style=for-the-badge&logo=github&logoColor=white)](https://github.com/RustOnEdge)

</div>
