# Vonder Games - Internship Test

## Project: 2D Side Scroller Modular Systems
**Candidate:** Songwut Meesema (ทรงวุฒิ มีเสมา)

**Start Time:** Feb 26, 2026, 01:00 AM

---

## Worklog

### Day 1 - System 1: Time Hop System
- **01:30 AM - 02:15 AM: Project Initialization**
    - Planning  
    - Set up folder structure and modular architecture using the Observer Pattern.
    - Integrated Cinemachine for smooth camera management and frame tracking.
- **02:15 AM - 03:00 AM: Time System & Player Animation**
    - Developed `TimeManager` with a 3 period cycle (Morning, Afternoon, Evening) using the State Pattern. Each day starts at 06:00 (Morning) to ensure a consistent game loop.
    - The system calculates the `DayOfWeek` dynamically, allowing for infinite day progression (Day 100+ support).
    - Integrated Time Hop Triggers (Colliders) to satisfy the requirement for manual testing of time progression.
    - Finalized the Player Animator Controller, handling transitions for Walk, Jump, Fall, Attack, and Hurt states.

### Day 1 - System 2 & 4: Inventory, Crafting, and Interaction
- **06:00 PM - 08:30 PM: Inventory Foundation & Data Structure**
    - Developed a **Data Driven item system** using ScriptableObjects (`ItemData`) to support scalability.
    - Built the `InventoryManager` with core logic for item addition, removal, and stacking (max 10 items per slot).
    - Created the UI for the **6 slot Hotbar** and **12 slot Main Inventory**.
    - Implemented smooth **Selection Highlights** using Lerp interpolation to improve Game Feel when switching slots.

- **08:30 PM - 10:30 PM: Crafting System & Station Integration**
    - Built the `CraftingManager` to handle both **Instant Crafting** (from bag) and **Station Crafting** (at tables).
    - Designed `RecipeData` to allow designers to add new recipes without touching the code.
    - Developed a UI filtering logic: the personal bag only shows basic recipes, while the crafting station shows advanced ones.
    - Integrated  triggers for the Crafting Station, allowing players to open specialized menus with the 'E' key.

### Day 2 - Advanced Interaction & Storage
- **12:00 AM - 03:30 AM: Drag & Drop and Item Usage**
    - Developed a  **Drag & Drop engine** using Unity's `EventSystems` and Raycasting.
    - Built logic for swapping items between slots and merging stacks across different containers.
    - Integrated **Item Usage (Q key)**:
        - **Usables:** Consumed upon use.
        - **Equippables:** Toggles a red background highlight to show "Equipped" status.
        - **Placeables:** Spawns the object prefab in the world (e.g. placing the Storage Chest).
    - Fixed a bug where UI overlays were blocking mouse input by adjusting **Raycast Target** settings.

- **03:30 AM - 05:30 AM: Storage Chest System**
    - Developed the **30 slot Storage Chest** component, fulfilling the technical requirement for expanded storage.
    - Created a dynamic UI link: approaching a world object and pressing 'E' now correctly swaps the UI context to show only the storage contents.
    - Implemented a "Drop to World" feature where dragging items outside the UI boundary spawns their physical prefabs.

- **05:30 AM++ : System Stability & Debugging**
    - Solved `MissingReferenceException` and `NullReference` issues by implementing strict null-checks for external data lists.
    - Fixed a critical **Memory Leak** issue by managing static event subscriptions using `OnEnable` and `OnDisable`.
    - Standardized all interactable objects to use **Physics 2D (Box Collider 2D)** to ensure consistent trigger behavior across all systems.

### Day 2 - System 3: Combat System & AI
- **03:30 PM - 04:30 PM: Combat Foundation**
    - Built the `PlayerStatus` system to track Health and AP.
    - Developed the **Wand & Projectile** system. Players can now aim with the mouse and fire shots.
    - Integrated randomized damage (3-5) and AP cost per shot.


- **05:30 PM - 07:15 PM: Slime AI, UI, and Polish**
    - Developed the **Slime AI** with simple chase and attack states. 
    - Implemented the **Split logic**: Large slimes (20 HP) play a death animation before splitting into two small slimes (5 HP).
    - Created a **Combat Zone**: Walking out of this area automatically resets Player HP and AP to 100.
    - Integrated the Day Cycle with Combat: Health and AP fully restore at the start of a new day (Day 0 Morning).
    - Implemented the logic where leaving the combat zone or dying resets HP and AP back to 100 immediately
    - Added **Sprite Flash** effect (white tint).
 
  - **07:30 PM - 08:00 PM: Final UX Polish & Bug Fixes**
    - Developed a **Double Right-Click** feature to remove items from any slot one by one. 
    - Fixed the "Shoot through UI" bug. Combat and projectiles are now automatically blocked whenever any inventory, chest, or crafting menu is open.

---

### Design Decisions
I focused on making the code **Modular** and **Scalable** so it’s easy to add more features later:
- **Observer Pattern:** I used a static `GameEvents` class to handle communication. For example, when time changes, the `TimeManager` just sends a message, and other systems (like UI or Player Status) listen and update themselves. This keeps scripts independent.
- **Data Driven Design:** Used **ScriptableObjects** for Items and Recipes. If I want to add 10 new items, I just create new assets in the editor without changing any code.
- **Interface** Used an `IDamageable` interface for the combat system. This allows the Wand to damage anything (Player, Slime, or even objects) as long as they have the interface.
- **Singleton Pattern:** Used for core managers like `InventoryManager` and `PlayerStatus` for easy global access while keeping logic organized.

### Challenges & Solutions
- **Projectile Aiming Bug:** Initially, projectiles fired downwards. I fixed this by correctly calculating the camera's Z-depth when converting mouse positions to world space.
- **2D vs 3D Physics:** I accidentally used 3D Box Colliders at first, which didn't trigger `OnTriggerEnter2D`. I fixed it by switching everything to **Box Collider 2D**.
- **Memory Leaks:** Experienced `MissingReferenceException` when restarting the game. I fixed this by using `OnEnable` and `OnDisable` to properly unsubscribe from static events.
- **UI Syncing:** Fixed an issue where UI showed "New Text" at the start by calling an initial refresh in the `Start` method of the UI managers.
- **AI Scaling:** Slimes used to shrink when chasing. I solved this by saving the **original scale** in a variable and only flipping the X-axis for direction.


### Controls

| Action | Keyboard / Mouse |
| :--- | :--- |
| **Move** | `A` `D` |
| **Jump** | `Space` |
| **Attack (Aim with Mouse)** | `Left Click` |
| **Select Hotbar Slot 1-6** | `1` - `6` |
| **Cycle Hotbar Slots** | `Mouse Scroll` |
| **Use / Equip / Place Item** | `Q` |
| **Remove 1 Item from Slot** | `Double Right Click` |
| **Open Inventory & Crafting** | `I` |
| **Interact (Chest / Station)** | `E` |
