# AGENTS.md — IronSightMR V1

## 1. Project Overview

IronSightMR is a Mixed Reality (MR) training system built for Meta Quest.

V1 goal:
- Use Meta Quest built-in room scanning (Space Setup)
- Validate environment readiness
- Generate a valid extraction point
- Create a basic navigation loop (no hazards yet)

Core philosophy:
- Real-world environment is the training ground
- Keep systems modular and scalable
- Build MVP first, then layer complexity

---

## 2. Tech Stack

- Unity (XR / URP)
- OpenXR (Meta Quest Support)
- Meta XR Core SDK
- MRUK (Mixed Reality Utility Kit)
- Target Platform: Meta Quest 3 / 3S

---

## 3. Scene Structure

Scenes are minimal and purpose-driven.

### BootScene
Purpose:
- App initialization
- Transition to MainScene

Contains:
- AppBootstrap
- Loading UI

---

### MainScene
Purpose:
- Main menu
- Room readiness flow
- Training loop (V1)
- Results UI

Contains:
- GameManager
- UI Router
- RoomReadinessController
- XR Rig
- Runtime objects

---

## 4. Core Flow

Start IronSight  
→ BootScene  
→ MainScene  

MainScene:

1. User selects "Prepare Room"
2. App checks room readiness
3. If not ready:
   - Guide user to Meta Space Setup
4. User returns to app
5. App rechecks readiness
6. User selects "Start Training"
7. App:
   - Reads environment data
   - Gets player position
   - Generates extraction point
8. Player reaches extraction zone
9. Show results screen

---

## 5. Architecture Principles

### 5.1 Separation of Responsibility
- Meta handles environment scanning
- IronSight handles:
  - validation
  - interpretation
  - gameplay logic

---

### 5.2 Modular Systems

Modules:

- Core (App lifecycle)
- UI (Panels, routing)
- Room (Scene / MRUK integration)
- Gameplay (Extraction logic)
- Later: Hazards, Evaluation

---

### 5.3 MVP First

DO:
- Build simple working loop
- Use placeholders
- Validate environment integration early

DO NOT:
- Add fire/smoke early
- Overbuild systems
- Optimize prematurely

---

## 6. Folder Structure

Assets/_Project/
├── Scenes/
├── Scripts/
│ ├── Core/
│ ├── UI/
│ ├── Room/
│ ├── Gameplay/
│ └── Data/
├── Prefabs/
├── Art/
├── Materials/
└── Data/

---

## 7. Coding Guidelines

### General
- Use clean, readable C# (no overengineering)
- Prefer small, focused classes
- Avoid tight coupling between systems

---

### Naming

Scripts:
- PascalCase (e.g., `GameManager`, `RoomReadinessController`)

Private variables:
- `_camelCase`

Public fields:
- `camelCase`

---

### Structure

Each system should:
- Have a single responsibility
- Avoid direct dependencies where possible
- Use manager/controller pattern

---

## 8. Key Systems (V1)

### Core
- AppBootstrap
- SceneLoader
- GameManager

---

### UI
- UIScreenRouter
- MainMenuUI
- RoomSetupUIController

---

### Room
- RoomReadinessController
- SceneDataController (MRUK integration later)

---

### Gameplay
- PlayerTracker
- ExtractionPointCalculator
- ExtractionZoneController
- ScenarioFlowController

---

## 9. Room Readiness Logic

Agent MUST follow:

1. Check if Scene Model exists
2. If not:
   - Prompt user to complete Space Setup
3. Re-check after return
4. Only allow gameplay when ready

Do NOT:
- Fake environment readiness
- Assume room exists without validation

---

## 10. Extraction Point Rules (V1)

- Must be on valid floor
- Must be within room bounds
- Must be far from player start
- Must not overlap obstacles

Use:
- sampled valid points
- distance scoring

---

## 11. UI Principles

- Minimal
- Clean
- Slightly dull / low contrast tone
- Avoid clutter
- Focus on clarity over decoration

---

## 12. What NOT to Build Yet

- Fire systems
- Smoke systems
- Health systems
- Complex physics
- AI systems

These belong to later phases.

---

## 13. Development Workflow

Follow this order:

1. Project setup (done)
2. BootScene → MainScene transition
3. UI shell
4. Room readiness flow
5. Scene data access
6. Extraction point logic
7. Playable loop
8. Results screen

---

## 14. Agent Behavior Rules

When modifying code:

- Do NOT restructure entire project without reason
- Do NOT introduce unnecessary abstractions
- Always respect current architecture
- Prefer incremental changes
- Keep scripts short and focused

---

## 15. Future Extensions (Do NOT implement now)

- Fire simulation
- Smoke density system
- Burn / inhalation mechanics
- Adaptive difficulty
- Multi-room navigation

---

## 16. Philosophy

IronSight is not a game first.

It is:
> A system that turns real-world space into a training environment.

Every decision should support:
- clarity
- realism
- stability
- scalability