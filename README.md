# Vonder Games - Internship Test

## Project: 2D Side-Scroller Modular Systems
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
    - Developed `TimeManager` with a 3-period cycle (Morning, Afternoon, Evening) using the State Pattern. Each day starts at 06:00 (Morning) to ensure a consistent game loop.
    - The system calculates the `DayOfWeek` dynamically, allowing for infinite day progression (Day 100+ support).
    - Integrated Time Hop Triggers (Colliders) to satisfy the requirement for manual testing of time progression.
    - Finalized the Player Animator Controller, handling transitions for Walk, Jump, Fall, Attack, and Hurt states.
