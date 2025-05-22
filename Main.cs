namespace HasteEffects;

[Landfall.Modding.LandfallPlugin]
public class HasteEffects
{
	/// <summary>
	/// List to hold the UI stats that will be displayed.
	/// </summary>
	private static readonly List<UIStats> stats = new();

	static HasteEffects()
	{
		var menu = new HastySetting("HasteEffects");
		menu.OnConfig += () => new Config(menu);

		On.Player.Start += (orig, self) =>
		{
			orig(self);
			if (IsRun)
			{ Randomize(); }
		};
	}

	/// <summary>
	/// Determines if the current scene is a run where effects should be randomized.
	/// </summary>
	internal static bool IsRun
	{
		get
		{
			var curScn = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
			return
				(curScn.name.Contains("challenge", System.StringComparison.OrdinalIgnoreCase) && Config.ChallengeLevels.Value) ||
				(curScn.buildIndex == 27 && Config.BossLevels.Value) ||
				curScn.buildIndex == 7;
		}
	}

	/// <summary>
	/// Randomizes the enabled stats and updates the UI accordingly.
	/// </summary>
	private static void Randomize()
	{
		// Get all enabled stats.
		var availableStats = Config.statsHolder.Where(s => s.HastyBoolEnabled.Value).ToList();
		var selectedStats = new List<StatHolder>();

		// Add up to MaxEffects stats, with a random chance to include each one.
		for (int i = 0; i < Config.MaxEffects.Value && availableStats.Any(); i++)
		{
			// Select a random stat and remove it from the available list.
			var stat = availableStats[UnityEngine.Random.Range(0, availableStats.Count)];
			availableStats.Remove(stat);

			// The chance of adding a stat decreases as more are added.
			if (UnityEngine.Random.Range(0.0f, 1.0f) > (i * Config.FailChance.Value))
			{
				selectedStats.Add(stat);
			}
		}

		// Destroy all previous stats and clear the list.
		stats.ForEach(stat => stat.Destroy());
		stats.Clear();

		// Randomize and display the selected stats in the UI.
		foreach (var stat in selectedStats)
		{
			stat.PStat.multiplier = stat.RandomVal;
			stats.Add(new UIStats(
				$"{stat.Name}: {stat.PStat.multiplier:0.0}x",
				stats.Count
			));
		}
	}
}
