namespace HasteEffects;

[Landfall.Modding.LandfallPlugin]
public class Main
{
	// The mod's unique identifier (GUID) for internal use and patching.
	public static string GUID = "com.github.ignoredsoul.hastyeffects";

	// The name of the mod, used for the config menu.
	public static string NAME = "HastyEffects";

	// The Harmony instance
	private static HarmonyLib.Harmony harmony;

	// List to hold the UI stats that will be displayed.
	private static List<UIStats> stats = new List<UIStats>();

	static Main()
	{
		harmony = new(GUID);
		harmony.PatchAll(typeof(Patching));
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

		// Pick a random stat and add it to the selected stats.
		Stat gstat = availableStats[NumberUtils.random.Next(0, availableStats.Count)];
		availableStats.Remove(gstat);
		selectedStats.Add(gstat);

		// Add up to 5 more stats, with a random chance to include each one.
		for (int i = 0; (i < 5 && availableStats.Any()); i++)
		{
			Stat stat = availableStats[NumberUtils.random.Next(0, availableStats.Count)];
			availableStats.Remove(stat);

			// The chance of adding a stat decreases as we add more.
			if (NumberUtils.NextD() > (i * 0.25)) selectedStats.Add(stat);
		}

		// Destroy all previous stats and clear the list.
		stats.ForEach(stat => stat.Destroy());
		stats.Clear();

		// Check if the game is in endless mode. Probs a better way but it works
		bool isEndless = UnityEngine.GameObject.Find("GAME/UI_Gameplay/LeftCorner/Distance").activeSelf;
		if (isEndless) stats.Add(new("", 0));

		// Randomize and display the selected stats in the UI.
		selectedStats.ForEach((stat) =>
		{
			// Apply a randomized value to the selected stat
			Manager.RandomizeStat(stat);

			// Add the stat to the UI
			UIStats newStat = new(
				stat.ToString() + ": " + Manager.GetStat(stat).multiplier.ToString("0.0") + "x",
				(stats.Count + 1)
			);

			// Then add statui to the list
			stats.Add(newStat);
		});

		// Hide the first stat in endless mode since it's a blank stat and is used for making space.
		if (isEndless) stats[0].InfoObject.SetActive(false);
	}
}

[HarmonyLib.HarmonyPatch]
public class Patching
{
	// Think I can use 'On.Player.ResetPlayer' but honestly, don't care to try.

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

	/// <summary>
	/// Patches the "OnStringChanged" method to update the localized text if it belongs to this mod's GUID.
	/// </summary>
	/// <param name="__instance">The instance of the LocalizeUIText being patched.</param>
	[HarmonyLib.HarmonyPatch(typeof(Zorro.Localization.LocalizeUIText), "OnStringChanged")]
	[HarmonyLib.HarmonyPostfix]
	private static void OnStringChangedPostfix(Zorro.Localization.LocalizeUIText __instance)
	{
		if (__instance.String?.TableReference.TableCollectionName != Main.GUID) return;
		__instance.Text.text = __instance.String.TableEntryReference.Key;
	}

	/// <summary>
	/// Patches the "RegisterPage" method to initialize the Values object when the settings page is registered.
	/// </summary>
	/// <param name="__instance">The instance of HasteSettingsHandler being patched.</param>
	[HarmonyLib.HarmonyPatch(typeof(HasteSettingsHandler), "RegisterPage")]
	[HarmonyLib.HarmonyPrefix]
	private static void RegisterPagePrefix(HasteSettingsHandler __instance) => Main.Values = new Values();
}
