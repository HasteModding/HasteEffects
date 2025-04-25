namespace HasteEffects;

[Landfall.Modding.LandfallPlugin]
public class Main
{
	// The mod's unique identifier (GUID) for internal use and patching.
	public static string GUID = "com.github.ignoredsoul.hasteeffects";

	// The name of the mod, used for the config menu.
	public static string NAME = "HasteEffects";

	// The Harmony instance
	private static HarmonyLib.Harmony harmony;

	// List to hold the UI stats that will be displayed.
	private static List<UIStats> stats = new List<UIStats>();

	static Main()
	{
		harmony = new(GUID);
		harmony.PatchAll(typeof(Patching));
		Values = null!;

		On.HasteSettingsHandler.RegisterPage += (orig, self) => Values = new Values();
	}

	// Accessor property
	public static Values Values { get; set; }

	/// <summary>
	/// Randomizes stats and updates the UI accordingly.
	/// </summary>
	internal static void Randomize()
	{
		// Get all available stats from the Stat enum and convert to a list.
		List<Stat> availableStats = Enum.GetValues(typeof(Stat)).Cast<Stat>().ToList();
		List<Stat> selectedStats = new();

		// Add up to X stats, with a random chance to include each one.
		for (int i = 0; (i < Values.maxEffects && availableStats.Any()); i++)
		{
			// Select a stat from the available stats list and remove it so it cannot be picked again.
			Stat stat = availableStats[NumberUtils.random.Next(0, availableStats.Count)];
			availableStats.Remove(stat);

			// The chance of adding a stat decreases as we add more.
			if (NumberUtils.NextD() > (i * Values.failChance)) selectedStats.Add(stat);
		}

		// Destroy all previous stats and clear the list.
		stats.ForEach(stat => stat.Destroy());
		stats.Clear();

		// Randomize and display the selected stats in the UI.
		selectedStats.ForEach((stat) =>
		{
			// Apply a randomized value to the selected stat
			Manager.RandomizeStat(stat);

			// Add the stat to the UI
			UIStats newStat = new(
				stat.ToString() + ": " + Manager.GetStat(stat).multiplier.ToString("0.0") + "x", // I can use the string format but that rounds.
				stats.Count
			);

			// Then add statui to the list
			stats.Add(newStat);
		});
	}
}

[HarmonyLib.HarmonyPatch]
public class Patching
{
	/// <summary>
	/// Patches the "RestartPlayer_Launch" method to randomize stats.
	/// </summary>
	/// <param name="spawnPoint">The new spawn point for the player. Redundant</param>
	/// <param name="minVel">Optional minimum velocity for the restart. Redundant</param>
	[HarmonyLib.HarmonyPatch(typeof(PlayerCharacter), "RestartPlayer_Launch", [typeof(UnityEngine.Transform), typeof(float)])]
	[HarmonyLib.HarmonyPostfix]
	private static void OnRestartPlayer(UnityEngine.Transform spawnPoint, float minVel = 0f)
	{ if (Manager.IsRun) Main.Randomize(); }

	/// <summary>
	/// Patches the "RestartPlayer_Still" method to randomize stats.
	/// </summary>
	/// <param name="spawnPoint">The new spawn point for the player. Redundant</param>
	[HarmonyLib.HarmonyPatch(typeof(PlayerCharacter), "RestartPlayer_Still", [typeof(UnityEngine.Transform)])]
	[HarmonyLib.HarmonyPostfix]
	private static void OnRestartPlayer(UnityEngine.Transform spawnPoint)
	{ if (Manager.IsRun) Main.Randomize(); }

	/// <summary>
	/// Patches the "RestartPlayerWithAnim" method to randomize stats.
	/// </summary>
	/// <param name="spawnPoint">The new spawn point for the player. Redundant</param>
	/// <param name="animId">Optional animation ID. Redundant</param>
	[HarmonyLib.HarmonyPatch(typeof(PlayerCharacter), "RestartPlayerWithAnim", [typeof(UnityEngine.Transform), typeof(int)])]
	[HarmonyLib.HarmonyPostfix]
	private static void OnRestartPlayer(UnityEngine.Transform spawnPoint, int animId = 1)
	{ if (Manager.IsRun) Main.Randomize(); }
}
