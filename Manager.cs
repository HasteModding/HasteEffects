using System.Reflection;

namespace HasteEffects;

public class Values
{
	internal HastyFloat AirSpeed_Max;

	internal HastyFloat AirSpeed_Min;

	internal HastyFloat Boost_Max;

	internal HastyFloat Boost_Min;

	internal HastyBool BossLevel_Apply;

	internal HastyBool ChallengeLevel_Apply;

	internal HastyFloat Drag_Max;

	internal HastyFloat Drag_Min;

	internal HastyFloat FastFall_Max;

	internal HastyFloat FastFall_Min;

	internal HastyFloat Gravity_Max;

	internal HastyFloat Gravity_Min;

	internal HastyFloat Luck_Max;

	internal HastyFloat Luck_Min;

	internal HastyFloat MaxEnergy_Max;

	internal HastyFloat MaxEnergy_Min;

	internal HastyFloat PickupRange_Max;

	internal HastyFloat PickupRange_Min;

	internal HastyFloat RunSpeed_Max;

	internal HastyFloat RunSpeed_Min;

	internal HastyFloat SparkMulti_Max;

	internal HastyFloat SparkMulti_Min;

	internal HastyFloat TurnSpeed_Max;

	internal HastyFloat TurnSpeed_Min;

	public Values()
	{
		HastySetting cfg = new($"<size=80%>{Main.NAME}", Main.GUID);

		ChallengeLevel_Apply = new HastyBool(cfg, "Challenges", "Apply to challenege levels", false);
		BossLevel_Apply = new HastyBool(cfg, "Boss", "Apply to the boss level", false);

		Gravity_Min = new HastyFloat(cfg, "Gravity", "min", 0f, 10f, 0.8f);
		Gravity_Max = new HastyFloat(cfg, "Gravity", "max", 0f, 10f, 2f);

		RunSpeed_Min = new HastyFloat(cfg, "Run Speed", "min", 0f, 10f, 0.4f);
		RunSpeed_Max = new HastyFloat(cfg, "Run Speed", "max", 0f, 10f, 3f);

		TurnSpeed_Min = new HastyFloat(cfg, "Turn Speed", "min", 0f, 10f, 0.8f);
		TurnSpeed_Max = new HastyFloat(cfg, "Turn Speed", "max", 0f, 10f, 3f);

		AirSpeed_Min = new HastyFloat(cfg, "Air Speed", "min", 0f, 10f, 0.4f);
		AirSpeed_Max = new HastyFloat(cfg, "Air Speed", "max", 0f, 10f, 2f);

		Drag_Min = new HastyFloat(cfg, "Drag", "min", 0f, 10f, 0.75f);
		Drag_Max = new HastyFloat(cfg, "Drag", "max", 0f, 10f, 1.25f);

		MaxEnergy_Min = new HastyFloat(cfg, "Max Energy", "min", 0f, 10f, 0.8f);
		MaxEnergy_Max = new HastyFloat(cfg, "Max Energy", "max", 0f, 10f, 3f);

		PickupRange_Min = new HastyFloat(cfg, "Pickup Range", "min", 0f, 10f, 0.5f);
		PickupRange_Max = new HastyFloat(cfg, "Pickup Range", "max", 0f, 10f, 2.5f);

		Boost_Min = new HastyFloat(cfg, "Boost", "min", 0f, 10f, 0.75f);
		Boost_Max = new HastyFloat(cfg, "Boost", "max", 0f, 10f, 2.25f);

		FastFall_Min = new HastyFloat(cfg, "Fast Fall", "min", 0f, 10f, 0.5f);
		FastFall_Max = new HastyFloat(cfg, "Fast Fall", "max", 0f, 10f, 3f);

		SparkMulti_Min = new HastyFloat(cfg, "Spark Multiplier", "min", 0f, 10f, 0.95f);
		SparkMulti_Max = new HastyFloat(cfg, "Spark Multiplier", "max", 0f, 10f, 5f);
	}

	public float airSpeed => NumberUtils.Next(AirSpeed_Min.Value, AirSpeed_Max.Value);
	public float boost => NumberUtils.Next(Boost_Min.Value, Boost_Max.Value);
	public float drag => NumberUtils.Next(Drag_Min.Value, Drag_Max.Value);
	public float FastFall => NumberUtils.Next(FastFall_Min.Value, FastFall_Max.Value);
	public float gravity => NumberUtils.Next(Gravity_Min.Value, Gravity_Max.Value);
	public float luck => NumberUtils.Next(Luck_Min.Value, Luck_Max.Value);
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
			return (curScn.name.ToLower().Contains("challenge") && Main.Values.ChallengeLevel_Apply.Value)
				|| (curScn.buildIndex == 27 && Main.Values.BossLevel_Apply.Value)
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
		FieldInfo field = Player.localPlayer.stats.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance)
			.FirstOrDefault(f => f.Name.Equals(stat.ToString(), StringComparison.OrdinalIgnoreCase));

		// If the field exists, return the PlayerStat; otherwise, log a debug message.
		if (field?.GetValue(Player.localPlayer.stats) is PlayerStat playerStat) return playerStat;
		else { UnityEngine.Debug.Log($"Stat '{stat.ToString()}' not found in PlayerStats."); }
		return null!;// Return null if not found (this shouldn't happen in most cases).
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
		if (property != null) return (float)property.GetValue(Main.Values);
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
		InfoObject.transform.position = new(InfoObject.transform.position.x, InfoObject.transform.position.y - (50 * shift), InfoObject.transform.position.z);
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
	FastFall,
	SparkMulti,
	Luck
}
