namespace HasteEffects;

public class Config
{
	public Config(HastySetting cfg)
	{
		// Generic settings
		new HastyButton(cfg, "Reset", "Reset stat values to their default values", new("Reset", () =>
		{
			MaxEffects.Reset();
			FailChance.Reset();
			BossLevels.Reset();
			ChallengeLevels.Reset();
			statsHolder.ForEach(s => s.Reset());
		}));

		HastyCollapsible Additional = new(cfg, "Additional", "Additional settings");
		MaxEffects = new HastyFloat(Additional, "Max Effects", "Maximum number of effects to apply", new(0, 13, 4));
		FailChance = new HastyFloat(Additional, "Fail Chance", "Chance of failing to apply an effect", new(0, 1, 0.15f));
		BossLevels = new HastyBool(Additional, "Boss Levels", "Enable for boss level", new("Disabled", "Enabled", false));
		ChallengeLevels = new HastyBool(Additional, "Challenge Levels", "Enable for challenge levels", new("Disabled", "Enabled"));

		// Stats settings
		statsHolder.Add(new(Stat.AirSpeed, cfg, new(0.8f, 1.4f)));
		statsHolder.Add(new(Stat.Boost, cfg, new(0.5f, 2.5f)));
		statsHolder.Add(new(Stat.DamageMulti, cfg, new(0.25f, 3.85f)));
		statsHolder.Add(new(Stat.Dashes, cfg, new(0.5f, 2.5f)));
		statsHolder.Add(new(Stat.Drag, cfg, new(0.75f, 1.75f)));
		statsHolder.Add(new(Stat.EnergyGain, cfg, new(0.75f, 2.5f)));
		statsHolder.Add(new(Stat.FastFall, cfg, new(0.75f, 3.25f)));
		statsHolder.Add(new(Stat.Gravity, cfg, new(0.5f, 1.5f)));
		statsHolder.Add(new(Stat.Luck, cfg, new(0.15f, 4.85f)));
		statsHolder.Add(new(Stat.MaxEnergy, cfg, new(0.5f, 2.5f)));
		statsHolder.Add(new(Stat.MaxHealth, cfg, new(0.25f, 5f)));
		statsHolder.Add(new(Stat.PickupRange, cfg, new(0.75f, 3.75f)));
		statsHolder.Add(new(Stat.RunSpeed, cfg, new(0.75f, 2.5f)));
		statsHolder.Add(new(Stat.SparkMulti, cfg, new(0.5f, 1.5f)));
		statsHolder.Add(new(Stat.TurnSpeed, cfg, new(0.5f, 2.5f)));
	}

	public static HastyBool BossLevels { get; private set; } = null!;
	public static HastyBool ChallengeLevels { get; private set; } = null!;
	public static HastyFloat FailChance { get; private set; } = null!;
	public static HastyFloat MaxEffects { get; private set; } = null!;
	public static List<StatHolder> statsHolder { get; private set; } = new();
}
