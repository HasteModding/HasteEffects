namespace HasteEffects;

/// <summary>
/// Holds a single player stat, including UI and settings
/// </summary>
public class StatHolder
{
	private readonly HastyCollapsible _hastyCol;

	/// <summary>
	/// Initializes a new instance of the <see cref="StatHolder"/> class, creating all associated Hasty settings.
	/// </summary>
	/// <param name="stat">The stat this holder represents.</param>
	/// <param name="cfg">The parent HastySetting configuration.</param>
	/// <param name="defaultMinMax">The default minimum and maximum values for the stat.</param>
	/// <param name="enabled">Whether the stat is enabled by default.</param>
	public StatHolder(Stat stat, HastySetting cfg, Unity.Mathematics.float2 defaultMinMax, bool enabled = true)
	{
		// Store
		Stat = stat;

		// Create a new collapsible section for the stat
		_hastyCol = new HastyCollapsible(cfg, stat.ToString(), $"Settings for <b>{stat}</b>");

		// Create hasty floats for the min and max values
		HastyFloatMin = new HastyFloat(_hastyCol, stat.ToString(), "Minimum", new(0, 5, defaultMinMax.x));
		HastyFloatMax = new HastyFloat(_hastyCol, stat.ToString(), "Maximum", new(0, 5, defaultMinMax.y));

		// Create the hasty bool for enabling/disabling the stat
		HastyBoolEnabled = new HastyBool(_hastyCol, stat.ToString(), "Enable or disable this stat", new("Disabled", "Enabled", enabled));

		// Reset single stat button
		HastyButtonReset = new HastyButton(_hastyCol, "Reset", $"Reset <b>{stat}</b>'s stats", new() { ButtonText = "Reset", OnClicked = Reset });
	}

	/// <summary>
	/// Gets the HastyBool setting for enabling/disabling the stat.
	/// </summary>
	public HastyBool HastyBoolEnabled { get; private set; }

	/// <summary>
	/// Gets the HastyButton for resetting the stat.
	/// </summary>
	public HastyButton HastyButtonReset { get; private set; }

	/// <summary>
	/// Gets the collapsible group for this stat.
	/// </summary>
	public HastyCollapsible HastyCol => _hastyCol;

	/// <summary>
	/// Gets the HastyFloat for the maximum value.
	/// </summary>
	public HastyFloat HastyFloatMax { get; private set; }

	/// <summary>
	/// Gets the HastyFloat for the minimum value.
	/// </summary>
	public HastyFloat HastyFloatMin { get; private set; }

	/// <summary>
	/// Gets the maximum value, ensuring it is not less than the minimum.
	/// </summary>
	public float Max
	{
		get
		{
			if (HastyFloatMax.Value <= HastyFloatMin.Value)
			{
				Informer.Inform($"<b>{Stat}</b> max value is less than or equal to min value. Setting max to default value.");
				HastyFloatMax.Reset();
				return HastyFloatMax.Value;
			}
			return HastyFloatMax.Value;
		}
	}

	/// <summary>
	/// Gets the minimum value, ensuring it is not greater than the maximum.
	/// </summary>
	public float Min
	{
		get
		{
			if (HastyFloatMin.Value >= HastyFloatMax.Value)
			{
				Informer.Inform($"<b>{Stat}</b> min value is greater than or equal to max value. Setting min to default value.");
				HastyFloatMin.Reset();
				return HastyFloatMin.Value;
			}
			return HastyFloatMin.Value;
		}
	}

	/// <summary>
	/// Gets the display name for this stat.
	/// </summary>
	public string Name => Stat.ToString();

	/// <summary>
	/// Gets the associated PlayerStat instance for this stat.
	/// </summary>
	public PlayerStat PStat
	{
		get
		{
			return (PlayerStat)Player.localPlayer.stats.GetType()
				.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
				.Where(f => f.FieldType == typeof(PlayerStat))
				.FirstOrDefault(f => f.Name.Contains(Stat.ToString(), StringComparison.OrdinalIgnoreCase))
				.GetValue(Player.localPlayer.stats);
		}
	}

	/// <summary>
	/// Gets a random value between Min and Max for this stat.
	/// </summary>
	public float RandomVal => UnityEngine.Random.Range(Min, Max);

	/// <summary>
	/// Gets the stat type this holder represents.
	/// </summary>
	public Stat Stat { get; private set; }

	/// <summary>
	/// Resets all settings for this stat to their default values.
	/// </summary>
	public void Reset()
	{
		HastyFloatMax.Reset();
		HastyFloatMin.Reset();
		HastyBoolEnabled.Reset();
	}
}

/// <summary>
/// All supported player stats.
/// </summary>
public enum Stat
{
	MaxHealth,
	RunSpeed,
	AirSpeed,
	TurnSpeed,
	Drag,
	Gravity,
	FastFall,
	Dashes,
	Boost,
	Luck,
	MaxEnergy,
	SparkMulti,
	EnergyGain,
	DamageMulti,
	PickupRange
}
