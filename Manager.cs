using System.Reflection;
using HastySettings;

namespace HasteEffects;

public class Values
{
	private HastyFloat AirSpeed_Max;
	private HastyFloat AirSpeed_Min;
	private HastyFloat Boost_Max;
	private HastyFloat Boost_Min;

	private HastyBool BossLevel_Apply;
	private HastyBool ChallengeLevel_Apply;
	private HastyFloat Drag_Max;

	private HastyFloat Drag_Min;
	private HastyFloat FailChance;
	private HastyFloat FastFall_Max;
	private HastyFloat FastFall_Min;
	private HastyFloat Gravity_Max;
	private HastyFloat Gravity_Min;
	private HastyFloat Luck_Max;
	private HastyFloat Luck_Min;

	private HastyFloat MaxEffects;
	private HastyFloat MaxEnergy_Max;

	private HastyFloat MaxEnergy_Min;
	private HastyFloat PickupRange_Max;
	private HastyFloat PickupRange_Min;
	private HastyFloat RunSpeed_Max;
	private HastyFloat RunSpeed_Min;
	private HastyFloat SparkMulti_Max;
	private HastyFloat SparkMulti_Min;
	private HastyFloat TurnSpeed_Max;
	private HastyFloat TurnSpeed_Min;

	public Values()
	{
		HastySettings.HastySettings cfg = new(Main.NAME);

		new HastyButton(cfg, "Reset to default", "Resets all values to their default values <size=40%>(close and open to see the values update)</size>", "Reset", () =>
		{
			IEnumerable<FieldInfo> fields = Main.Values.GetType()
				.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
				.Where(f => new[] { typeof(HastyFloat), typeof(HastyBool), typeof(HastyInt) }.Contains(f.FieldType));

			foreach (FieldInfo field in fields)
			{
				object value = field.GetValue(Main.Values);
				value?.GetType()
				.GetMethod("Reset", BindingFlags.Public | BindingFlags.Instance)?.Invoke(value, null);
			}
		});

		HastyCollapsible additional = new HastyCollapsible(cfg, "Additional", "Additional settings you can change");
		MaxEffects = new HastyFloat(additional, "Max Effects", "Maximum number of effects to apply", 0, 11, 5, true);
		FailChance = new HastyFloat(additional, "Fail Chance", "The chance for an effect to not apply", 0.01f, 1, 0.25f);
		ChallengeLevel_Apply = new HastyBool(additional, "Challenges", "Apply to challenege levels", false);
		BossLevel_Apply = new HastyBool(additional, "Boss", "Apply to the boss level", false);

		HastyCollapsible gravity = new HastyCollapsible(cfg, "Gravity", "Gravity settings");
		Gravity_Min = new HastyFloat(gravity, "Min Gravity", "", 0f, 10f, 0.8f);
		Gravity_Max = new HastyFloat(gravity, "Max Gravity", "", 0f, 10f, 2f);

		HastyCollapsible runspeed = new HastyCollapsible(cfg, "Run Speed", "Run Speed settings");
		RunSpeed_Min = new HastyFloat(runspeed, "Min Run Speed", "", 0f, 10f, 0.4f);
		RunSpeed_Max = new HastyFloat(runspeed, "Max Run Speed", "", 0f, 10f, 3f);

		HastyCollapsible turnspeed = new HastyCollapsible(cfg, "Turn Speed", "Turn Speed settings");
		TurnSpeed_Min = new HastyFloat(turnspeed, "Min Turn Speed", "", 0f, 10f, 0.8f);
		TurnSpeed_Max = new HastyFloat(turnspeed, "Max Turn Speed", "", 0f, 10f, 3f);

		HastyCollapsible airspeed = new HastyCollapsible(cfg, "Air Speed", "Air Speed settings");
		AirSpeed_Min = new HastyFloat(airspeed, "Min Air Speed", "", 0f, 10f, 0.4f);
		AirSpeed_Max = new HastyFloat(airspeed, "Max Air Speed", "", 0f, 10f, 2f);

		HastyCollapsible drag = new HastyCollapsible(cfg, "Drag", "Drag settings");
		Drag_Min = new HastyFloat(drag, "Min Drag", "", 0f, 10f, 0.75f);
		Drag_Max = new HastyFloat(drag, "Max Drag", "", 0f, 10f, 1.25f);

		HastyCollapsible maxenergy = new HastyCollapsible(cfg, "Max Energy", "Max Energy settings");
		MaxEnergy_Min = new HastyFloat(maxenergy, "Min Max Energy", "", 0f, 10f, 0.8f);
		MaxEnergy_Max = new HastyFloat(maxenergy, "Max Max Energy", "", 0f, 10f, 3f);

		HastyCollapsible pickuprange = new HastyCollapsible(cfg, "Pickup Range", "Pickup Range settings");
		PickupRange_Min = new HastyFloat(pickuprange, "Min Pickup Range", "", 0f, 10f, 0.5f);
		PickupRange_Max = new HastyFloat(pickuprange, "Max Pickup Range", "mx", 0f, 10f, 2.5f);

		HastyCollapsible boost = new HastyCollapsible(cfg, "Boost", "Boost settings");
		Boost_Min = new HastyFloat(boost, "Min Boost", "", 0f, 10f, 0.75f);
		Boost_Max = new HastyFloat(boost, "Max Boost", "", 0f, 10f, 2.25f);

		HastyCollapsible fastfall = new HastyCollapsible(cfg, "Fast Fall", "Fast Fall settings");
		FastFall_Min = new HastyFloat(fastfall, "Min Fast Fall", "", 0f, 10f, 0.5f);
		FastFall_Max = new HastyFloat(fastfall, "Max Fast Fall", "", 0f, 10f, 3f);

		HastyCollapsible sparkmulti = new HastyCollapsible(cfg, "Spark Multiplier", "Spark Multiplier settings");
		SparkMulti_Min = new HastyFloat(sparkmulti, "Min Spark Multiplier", "", 0f, 10f, 0.95f);
		SparkMulti_Max = new HastyFloat(sparkmulti, "Max Spark Multiplier", "", 0f, 10f, 5f);

		HastyCollapsible luck = new HastyCollapsible(cfg, "Luck", "Luck settings");
		Luck_Min = new HastyFloat(luck, "Min Luck", "", 0f, 10f, 0.95f);
		Luck_Max = new HastyFloat(luck, "Max Luck", "", 0f, 10f, 5f);
	}

	public float airSpeed => NumberUtils.Next(AirSpeed_Min.Value, AirSpeed_Max.Value);
	public float boost => NumberUtils.Next(Boost_Min.Value, Boost_Max.Value);
	public bool bossLevels => BossLevel_Apply.Value;
	public bool challengeLevels => ChallengeLevel_Apply.Value;
	public float drag => NumberUtils.Next(Drag_Min.Value, Drag_Max.Value);
	public float failChance => FailChance.Value;
	public float FastFall => NumberUtils.Next(FastFall_Min.Value, FastFall_Max.Value);
	public float gravity => NumberUtils.Next(Gravity_Min.Value, Gravity_Max.Value);
	public float luck => NumberUtils.Next(Luck_Min.Value, Luck_Max.Value);
	public int maxEffects => (int)MaxEffects.Value;
	public float maxEnergy => NumberUtils.Next(MaxEnergy_Min.Value, MaxEnergy_Max.Value);

	public float runSpeed => NumberUtils.Next(RunSpeed_Min.Value, RunSpeed_Max.Value);
	public float sparkMultiplier => NumberUtils.Next(SparkMulti_Min.Value, SparkMulti_Max.Value);
	public float sparkPickupRange => NumberUtils.Next(PickupRange_Min.Value, PickupRange_Max.Value);
	public float turnSpeed => NumberUtils.Next(TurnSpeed_Min.Value, TurnSpeed_Max.Value);
}

internal static class NumberUtils
{
	/// <summary>
	/// Represents a shared instance of a random number generator.
	/// </summary>
	internal static readonly System.Random random = new(GenerateTrulyRandomNumber());

	/// <summary>
	/// Generates a truly random number using cryptographic random number generation.
	/// </summary>
	/// <returns>A truly random number within a specified range.</returns>
	internal static int GenerateTrulyRandomNumber()
	{
		using System.Security.Cryptography.RNGCryptoServiceProvider rng = new();
		byte[] bytes = new byte[4]; // 32 bities :3c
		rng.GetBytes(bytes);

		// Convert the random bytes to an integer and ensure it falls within the specified range
		int randomInt = System.BitConverter.ToInt32(bytes, 0);
		return System.Math.Abs(randomInt % (50 - 10)) + 10;
	}

	/// <summary>
	/// Returns a random float number within the specified range.
	/// </summary>
	/// <param name="min">The inclusive lower bound of the random float number to be generated.</param>
	/// <param name="max">The exclusive upper bound of the random float number to be generated.</param>
	/// <returns>A random float number within the specified range.</returns>
	internal static float Next(float min, float max) => (float)((NextD() * (max - min)) + min);

	/// <summary>
	/// Returns a random double number between 0.0 and 1.0.
	/// </summary>
	/// <returns>A random double number between 0.0 and 1.0... Duh</returns>
	internal static double NextD() => random.NextDouble();
}

internal class Manager
{
	/// <summary>
	/// Determines if the current scene and settings indicate that the game is running in a special challenge or boss level.
	/// </summary>
	internal static bool IsRun
	{
		get
		{
			UnityEngine.SceneManagement.Scene curScn = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
			return (curScn.name.ToLower().Contains("challenge") && Main.Values.challengeLevels)
				|| (curScn.buildIndex == 27 && Main.Values.bossLevels)
				|| curScn.buildIndex == 7;
		}
	}

	/// <summary>
	/// Retrieves the PlayerStat object associated with a specific stat.
	/// </summary>
	/// <param name="stat">The stat to get the PlayerStat for.</param>
	/// <returns>The PlayerStat associated with the given stat.</returns>
	internal static PlayerStat GetStat(Stat stat)
	{
		// Find the corresponding field in the player's stats using reflection.
		// I am doing contains instead of just equal 'cause I want my enum to contain "sparkMulti" instead of "sparkMultiplier"
		FieldInfo field = Player.localPlayer.stats.GetType()
			.GetFields(BindingFlags.Public | BindingFlags.Instance)
			.Where(f => f.FieldType == typeof(PlayerStat))
			.FirstOrDefault(f => f.Name.Contains(stat.ToString(), StringComparison.OrdinalIgnoreCase));

		// If the field exists, return the PlayerStat; otherwise, log a debug message.
		if (field?.GetValue(Player.localPlayer.stats) is PlayerStat playerStat) return playerStat;
		else { UnityEngine.Debug.LogError($"Stat '{stat.ToString()}' not found in PlayerStats."); }
		return null!; // Return null if not found (this shouldn't happen in most cases).
	}

	/// <summary>
	/// Retrieves the value of a specific stat from the `Values` object.
	/// </summary>
	/// <param name="stat">The stat to retrieve the value for.</param>
	/// <returns>The value of the stat, or 1 if it couldn't be found.</returns>
	internal static float GetValue(Stat stat)
	{
		// Try to find the corresponding property in the Values object using reflection.
		PropertyInfo property = Main.Values.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
			.FirstOrDefault(p => p.Name.Equals(stat.ToString(), StringComparison.OrdinalIgnoreCase));

		// If found, return the value of the property.
		if (property != null)
		{
			float v = (float)property.GetValue(Main.Values);
			if (v.ToString("0.0") == "1.0") v += NumberUtils.Next(-0.1f, 0.1f); // If the value is 1, we add a small random value to it. Fuck you.
			return v;
		}
		else { UnityEngine.Debug.Log($"Stat '{stat.ToString()}' not found in Values."); }
		return 1; // If we cannot find it for some reason, we just return 1 which is default. Oh well.
	}

	/// <summary>
	/// Randomizes the specified stat by setting its multiplier to the corresponding value from the `Values` object.
	/// </summary>
	/// <param name="stat">The stat to randomize.</param>
	internal static void RandomizeStat(Stat stat) => GetStat(stat).multiplier = GetValue(stat);
}

internal class UIStats
{
	public UnityEngine.GameObject InfoObject;

	public TMPro.TextMeshProUGUI InfoText;

	/// <summary>
	/// Creates a new UIStats element and positions it in the left corner of the screen.
	/// </summary>
	/// <param name="Text">The text to display in the UI element.</param>
	/// <param name="shift">The vertical offset for the UI element.</param>
	public UIStats(string Text, int shift)
	{
		UnityEngine.GameObject speedUI = UnityEngine.GameObject.Find("GAME/UI_Gameplay/LeftCorner/Speed/");

		// Create the UI element from the existing prefab and parent it to the UI container.
		InfoObject = UnityEngine.GameObject.Instantiate(speedUI, speedUI.transform.parent);

		// Remove the default UI_Speed component (we don't need it for this stat).
		UnityEngine.Object.Destroy(InfoObject.GetComponent<UI_Speed>());

		// Adjust the position of the UI element, applying the vertical shift.
		InfoObject.transform.position = new(InfoObject.transform.position.x + 109, InfoObject.transform.position.y - (50 * shift), InfoObject.transform.position.z);
		InfoObject.GetComponentInChildren<UnityEngine.UI.Image>().color = new UnityEngine.Color(0.067f, 0.878f, 0.537f);

		// Get the TextMeshProUGUI component and configure the text.
		InfoText = InfoObject.GetComponent<TMPro.TextMeshProUGUI>();
		InfoText.text = Text;
		InfoText.richText = true;
		InfoText.enableAutoSizing = true;
		InfoText.fontSizeMax = 20;
		InfoText.color = new UnityEngine.Color(0, 1, 0.58f);
	}

	/// <summary>
	/// Destroys the UI element when it's no longer needed.
	/// </summary>
	public void Destroy() => UnityEngine.GameObject.Destroy(InfoObject);
}

public enum Stat
{
	RunSpeed,
	Gravity,
	AirSpeed,
	TurnSpeed,
	Drag,
	MaxEnergy,
	PickupRange,
	Boost,
	FallSpeed,
	SparkMulti,
	Luck
}
