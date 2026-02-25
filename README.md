# Vonder Games - Internship Test

## Project: 2D Side-Scroller Modular Systems
**Candidate:** Songwut Meesema (ทรงวุฒิ มีเสมา)
**Start Time:** Feb 26, 2026, 01:00 AM

---

## Worklog

### Day 1 - System 1: Time Hop System
- **01:30 AM - 02:15 AM: Project Initialization**
    - Set up folder structure and modular architecture.
    - Integrated **Cinemachine** for camera management.
- **02:15 AM - 02:43 AM: System 1 Implementation & Player States**
    - Developed `TimeManager` with a 3-period cycle (Morning, Afternoon, Evening) based on the **State Pattern**.
        - **Assumption:** Each day starts at 06:00 (Morning) for a consistent game loop.
        - **Explanations:** The system calculates the `DayOfWeek` dynamically based on total days to support infinite progression. 
    - Built a  **Animator Controller** for the player, handling transitions for Walk, Jump, Fall, Attack, and Damage states.
    - Integrated **Time Hop Triggers** using colliders to allow easy manual testing of time progression as per PDF requirements.

---

## Technical Explanation 
- **Observer Pattern:** Used `GameEvents` to decoupled the Time System from the UI/Visuals.
- **State Pattern:** Applied to the Time Manager to ensure the Morning -> Afternoon -> Evening cycle is followed strictly.
