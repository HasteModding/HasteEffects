# HasteEffects

### 🧐 How It Works

At key moments (starting a run, entering a shard, or losing a life), the mod:
1. Randomly selects a set number of stats.
2. For each stat, rolls a configurable chance (0.0–1.0) to determine if it gets modified.
3. Applies a random multiplier to each chosen stat, within a configurable range.

Each stat can only be selected once per run, though I could add stackable modifiers.

<details>
<summary>🎯 Affected Stats (click to expand)</summary>

- Max Health  
- Run Speed  
- Air Speed  
- Turn Speed  
- Drag  
- Gravity  
- Fast Fall  
- Dashes  
- Boost  
- Luck  
- Max Energy  
- Spark Multiplier  
- Energy Gain  
- Damage Multiplier  
- Pickup Range  

</details>

---
<!--------------------------------------------------------------------------------------->

### 🛠️ Configuration

A dedicated tab in the **Settings Menu** allows you to configure:
- Number of stats affected
- Min/max multiplier values
- Selection roll chance
- Toggle for disabling changes during boss/challenge levels

> [!CAUTION]
> Due to unity being so AMAZING, having a lot of UI elements causes lag...<br/>
> I've tried my best to reduce the lag, but it's not perfect.<br/>
I'd recommend configuring the mod in the main menu.

---
<!--------------------------------------------------------------------------------------->

### 🖥️ UI

A new UI element appears in the **top-left of the screen**, showing:
- The stat name
- Its current multiplier (e.g., `1.01x`, `0.91x`, `2.51x`)

---
<!--------------------------------------------------------------------------------------->

### 🎲 Gameplay Impact

- **Stat effects are applied every time you start a run, enter a shard, or lose a life.**
- **Each stat has an equal chance to be selected**: but must pass a "roll chance" before being applied.
- **No safeguards are in place**: a bad roll can seriously mess up your run. Deal with it.
- **Surprises only**: you won't know which stats were modified until you're already in the run.
- **Stats interact emergently**:
  - High gravity + high runspeed = extreme momentum
  - Low gravity + high airspeed = flight simulator
  - High drag + low runspeed = prepare to suffer
- **Encourages dynamic play**:
  - More spark multiplier? Chase sparks.
  - Higher max energy? Plan your landings.
  - Fast ground speed? Stay grounded.
- **No rerolls mid-run**: die or beat the shard to get a new roll.

---
<!--------------------------------------------------------------------------------------->

### 📦 Installation

Subscribe to the mod on the [Steam Workshop](https://steamcommunity.com/sharedfiles/filedetails/?id=3461609248). That's it.

---
<!--------------------------------------------------------------------------------------->

### Final Words
```
░▒▓████████▓▒░    ░▒▓██████▓▒░     ░▒▓██████▓▒░     ░▒▓██████▓▒░     ░▒▓██████▓▒░    ░▒▓██████████████▓▒░  
       ░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░ 
     ░▒▓██▓▒░    ░▒▓█▓▒░░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░ 
   ░▒▓██▓▒░      ░▒▓█▓▒░░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░ 
 ░▒▓██▓▒░        ░▒▓█▓▒░░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░ 
░▒▓█▓▒░          ░▒▓█▓▒░░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░   ░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░ 
░▒▓████████▓▒░    ░▒▓██████▓▒░     ░▒▓██████▓▒░     ░▒▓██████▓▒░     ░▒▓██████▓▒░    ░▒▓█▓▒░░▒▓█▓▒░░▒▓█▓▒░ 
```

</br>
<h3>Anyway, here's the update journey...</h3></br>
</br>

# Updates

### v1.3.0
- Replaced HastySettings to internal SettingsLib.
- Overhauled README
- Completely changed how settings and stats are created
- Removed HarmonyX as a dependancy. Moved to Hooks.

### v1.2.0
- Revamped HastySettings
- Fixed saving your settings made them reset to the last changed value of any setting.

### v1.1.0
- Added a new button to reset values to their defaults.

### v1.0.2
- Fixed ui once again.

### v1.0.1
- Fixed for new Haste version, it broke the UI elements.

### v1.0.0
- Added a logo :3c
- Added scene checks.
- Some general cleanup.
- Zooming.
