using HarmonyLib;
using Zorro.Settings;
using Unity.Mathematics;
using UnityEngine.Localization;
using Landfall.Haste;
using UnityEngine;
using System.Runtime.CompilerServices;
using Zorro.Settings.UI;
using UnityEngine.UI;

// Based from https://github.com/NaokoAF/HastyControls, could not have of done it without guidance from their code.
namespace HastySettings;

public interface IHastySetting : IExposedSetting
{
	HastyData HastyData { get; set; }
	string Key { get; }
	HastyCollapsible ParentCollapsible { get; set; }
}

public class HastyBool : BoolSetting, IEnumSetting, IHastySetting
{
	public HastyBool(HastySettings config, string name, string description, bool defaultValue, string offChoice = "Off", string onChoice = "On")
	{
		_config = config;
		Key = $"{_config.ModName}.{name}.{description}";

		_displayName = _config!.CreateDisplayName(name, description);
		_defaultValue = defaultValue;
		_choices = new List<string> { offChoice, onChoice };
		_config.Add(this);

		HastyData = null!;
		ParentCollapsible = null!;
	}

	public HastyBool(HastyCollapsible config, string name, string description, bool defaultValue, string offChoice = "Off", string onChoice = "On")
	{
		_config = config._config;
		Key = $"{_config.ModName}.{name}.{description}";

		_displayName = _config!.CreateDisplayName(name, description);
		_defaultValue = defaultValue;
		_choices = new List<string> { offChoice, onChoice };
		_config.Add(this);

		HastyData = null!;
		(ParentCollapsible = config).Content.Add(this);
	}

	public event Action<bool>? Applied;

	public HastyData HastyData { get; set; }
	public string Key { get; }

	public override LocalizedString OffString => null!;

	public override LocalizedString OnString => null!;

	public HastyCollapsible ParentCollapsible { get; set; }

	private List<string> _choices { get; set; }

	private HastySettings _config { get; set; }

	private bool _defaultValue { get; set; }

	private LocalizedString _displayName { get; set; }

	public override void ApplyValue() => Applied?.Invoke(Value);

	public string GetCategory() => _config.ModName;

	public LocalizedString GetDisplayName() => _displayName;

	List<string> IEnumSetting.GetUnlocalizedChoices() => _choices;

	public override void Load(ISettingsSaveLoad loader) => Value = loader.TryLoadBool(Key, out bool value) ? value : GetDefaultValue();

	public void Reset()
	{
		Value = _defaultValue;
		ApplyValue();
	}

	public override void Save(ISettingsSaveLoad saver) => saver.SaveBool(Key, Value);

	protected override bool GetDefaultValue() => _defaultValue;
}

public class HastyButton : ButtonSetting, IHastySetting, IExposedSetting
{
	private string _buttonText;
	private HastySettings _config;
	private LocalizedString _displayName;

	public HastyButton(HastySettings config, string name, string description, string buttonText, Action clicked)
	{
		_config = config;
		Key = $"{_config.ModName}.{name}.{description}";

		_displayName = _config!.CreateDisplayName(name, description);
		_buttonText = buttonText;
		_config.Add(this);

		Clicked = clicked;
		HastyData = null!;
		ParentCollapsible = null!;
	}

	public HastyButton(HastyCollapsible config, string name, string description, string buttonText, Action clicked)
	{
		_config = config._config;
		Key = $"{_config.ModName}.{name}.{description}";

		_displayName = _config!.CreateDisplayName(name, description);
		_buttonText = buttonText;
		_config.Add(this);

		Clicked = clicked;
		HastyData = null!;
		(ParentCollapsible = config).Content.Add(this);
	}

	public event Action Clicked;

	public HastyData HastyData { get; set; }

	public string Key { get; private set; }

	public HastyCollapsible ParentCollapsible { get; set; }

	public override string GetButtonText() => _buttonText;

	public string GetCategory() => _config.ModName;

	public LocalizedString GetDisplayName() => _displayName;

	public override void OnClicked(ISettingHandler settingHandler) => Clicked?.Invoke();
}

public class HastyCollapsible : ButtonSetting, IHastySetting, IExposedSetting
{
	public readonly HastySettings _config;

	private readonly LocalizedString _displayName;

	public HastyCollapsible(HastySettings config, string name, string description)
	{
		_config = config;
		Key = $"{config.ModName}.{name}.{description}"; // I'm not 100% sure but this might make it so it stays open when switching tabs and back.

		_displayName = _config.CreateDisplayName(name, description);
		_config.Add(this);

		HastyData = null!;
		ParentCollapsible = null!;
		Clicked = null!;
	}

	public event Action<bool> Clicked;

	public bool Collapsed { get; private set; } = true;
	public List<IHastySetting> Content { get; set; } = new List<IHastySetting>();

	public HastyData HastyData { get; set; }
	public string Key { get; }
	public HastyCollapsible ParentCollapsible { get; set; }

	public override string GetButtonText() => null!;

	public string GetCategory() => _config.ModName;

	public LocalizedString GetDisplayName() => _displayName;

	public override void OnClicked(ISettingHandler settingHandler) => Clicked?.Invoke(Collapsed = !Collapsed);
}

public class HastyData
{
	public HastyData(ButtonSettingUI buttonUI = null!, CanvasGroup canvas = null!, GameObject gameObject = null!, LayoutElement layout = null!, IExposedSetting exposed = null!, HastyCollapsible col = null!, IHastySetting hasty = null!)
	{
		ButtonSettingUI = buttonUI;
		CanvasGroup = canvas;
		GameObject = gameObject;
		LayoutElement = layout;
		ExposedSetting = exposed;
		HastyCollapsible = col;
		HastySetting = hasty;
	}

	public ButtonSettingUI ButtonSettingUI { get; set; }
	public CanvasGroup CanvasGroup { get; set; }
	public IExposedSetting ExposedSetting { get; set; }
	public GameObject GameObject { get; set; }
	public HastyCollapsible HastyCollapsible { get; set; }
	public IHastySetting HastySetting { get; set; }
	public LayoutElement LayoutElement { get; set; }
}

public class HastyFloat : FloatSetting, IHastySetting
{
	private HastySettings _config;
	private float _defaultValue;
	private LocalizedString _displayName;
	private float2 _minMax;

	public HastyFloat(HastySettings config, string name, string description, float min, float max, float defaultValue, bool whole = false)
	{
		_config = config;
		Key = $"{_config!.ModName}.{name}.{description}";

		_displayName = _config!.CreateDisplayName(name, description);
		_defaultValue = defaultValue;
		_minMax = new float2(min, max);
		_config.Add(this);

		IsWhole = whole;
		HastyData = null!;
		ParentCollapsible = null!;
	}

	public HastyFloat(HastyCollapsible config, string name, string description, float min, float max, float defaultValue, bool whole = false)
	{
		_config = config._config;
		Key = $"{_config!.ModName}.{name}.{description}";

		_displayName = _config!.CreateDisplayName(name, description);
		_defaultValue = defaultValue;
		_minMax = new float2(min, max);
		_config.Add(this);

		IsWhole = whole;
		HastyData = null!;
		(ParentCollapsible = config).Content.Add(this);
	}

	public event Action<float>? Applied;

	public HastyData HastyData { get; set; }

	public bool IsWhole { get; private set; }

	public string Key { get; }

	public HastyCollapsible ParentCollapsible { get; set; }

	public override void ApplyValue() => Applied?.Invoke(Value = (IsWhole ? math.round(Value) : Value));

	public string GetCategory() => _config.ModName;

	public LocalizedString GetDisplayName() => _displayName;

	public override void Load(ISettingsSaveLoad loader)
	{
		Value = loader.TryLoadFloat(Key, out float value) ? value : GetDefaultValue();
		if (MinValue == MaxValue) Debug.LogError($"Failed to load setting of type {Key} from PlayerPrefs.");
		MinValue = GetMinMaxValue().x;
		MaxValue = GetMinMaxValue().y;
	}

	public void Reset()
	{
		Value = _defaultValue;
		ApplyValue();
	}

	public override void Save(ISettingsSaveLoad saver) => saver.SaveFloat(Key, Value);

	protected override float GetDefaultValue() => _defaultValue;

	protected override float2 GetMinMaxValue() => _minMax;
}

public class HastyInt : IntSetting, IHastySetting
{
	private HastySettings _config;
	private int _defaultValue;
	private LocalizedString _displayName;

	public HastyInt(HastySettings config, string name, string description, int min, int max, int defaultValue)
	{
		_config = config;
		Key = $"{_config.ModName}.{name}.{description}";

		_displayName = _config!.CreateDisplayName(name, description);
		_defaultValue = defaultValue;
		_config.Add(this);

		HastyData = null!;
		ParentCollapsible = null!;
	}

	public HastyInt(HastyCollapsible config, string name, string description, int min, int max, int defaultValue)
	{
		_config = config._config;
		Key = $"{_config.ModName}.{name}.{description}";

		_displayName = _config!.CreateDisplayName(name, description);
		_defaultValue = defaultValue;
		_config.Add(this);

		HastyData = null!;
		(ParentCollapsible = config).Content.Add(this);
	}

	public event Action<int>? Applied;

	public HastyData HastyData { get; set; }
	public string Key { get; }
	public HastyCollapsible ParentCollapsible { get; set; }

	public override void ApplyValue() => Applied?.Invoke(Value);

	public string GetCategory() => _config.ModName;

	public LocalizedString GetDisplayName() => _displayName;

	public override void Load(ISettingsSaveLoad loader) => Value = loader.TryLoadInt(Key, out var value) ? value : GetDefaultValue();

	public void Reset()
	{
		Value = _defaultValue;
		ApplyValue();
	}

	public override void Save(ISettingsSaveLoad saver) => saver.SaveInt(Key, Value);

	protected override int GetDefaultValue() => _defaultValue;
}

public class HastyPatch
{
	private static AccessTools.FieldRef<SettingsUICell, CanvasGroup> canvasGroupRef = AccessTools.FieldRefAccess<SettingsUICell, CanvasGroup>("m_canvasGroup");

	private static ConditionalWeakTable<HastyData, IHastySetting> settingsMap = new ConditionalWeakTable<HastyData, IHastySetting>();

	[HarmonyPatch(typeof(ButtonSettingUI), "Setup")]
	[HarmonyPostfix]
	private static void ButtonSettingSetup(ButtonSettingUI __instance, Setting setting)
	{
		if (setting is IHastySetting hastySetting && setting is HastyCollapsible collapsible)
		{
			HastyData hastyData = GetTempoByHastySetting(hastySetting) ?? new HastyData(buttonUI: __instance, col: collapsible, hasty: hastySetting);

			__instance.Label.text = (collapsible.Collapsed ? "► Expand" : "▼ Collapse");
			collapsible.HastyData = hastyData;

			collapsible.Clicked += (bool collapsed) =>
			{
				__instance.Label.text = (collapsible.Collapsed ? "► Expand" : "▼ Collapse");
				collapsible.Content.ForEach(c =>
				{
					c.HastyData.LayoutElement.ignoreLayout = collapsed;
					c.HastyData.CanvasGroup.blocksRaycasts = !collapsed;
					c.HastyData.CanvasGroup.alpha = 0f;
					c.HastyData.GameObject.GetComponent<SettingsUICell>().enabled = !collapsed;

					Debug.LogError($"==========================================");
					Debug.LogError($"Is it collapsed? " + collapsible.Collapsed);
					Debug.LogError($"Ignoring Layout? " + c.HastyData.LayoutElement.ignoreLayout);
					Debug.LogError($"Blocks Raycasts? " + c.HastyData.CanvasGroup.blocksRaycasts);
				});
			};

			SetTempoForHastySetting(hastyData, hastySetting);
		}
	}

	[HarmonyPatch(typeof(SettingsUICell), "Setup")]
	[HarmonyPostfix]
	private static void CellSetupPostfix(SettingsUICell __instance, Setting setting)
	{
		if (setting is IHastySetting hastySetting && setting is IExposedSetting exposedSetting)
		{
			HastyData hastyData = GetTempoByHastySetting(hastySetting) ?? new HastyData(hasty: hastySetting);

			hastyData.CanvasGroup = canvasGroupRef.Invoke(__instance);
			hastyData.GameObject = __instance.gameObject;
			hastyData.LayoutElement = __instance.gameObject.AddComponent<LayoutElement>();
			hastyData.ExposedSetting = exposedSetting;
			hastyData.HastyCollapsible = (setting is HastyCollapsible col) ? col : null!;

			if (hastySetting.ParentCollapsible != null)
			{
				hastyData.LayoutElement.ignoreLayout = hastySetting.ParentCollapsible.Collapsed;
				hastyData.CanvasGroup.blocksRaycasts = !hastySetting.ParentCollapsible.Collapsed;
				hastyData.CanvasGroup.alpha = 0f;
				hastyData.GameObject.GetComponent<SettingsUICell>().enabled = !hastySetting.ParentCollapsible.Collapsed;
				hastyData.GameObject.transform.GetChild(0).GetComponent<Image>().color = new Color(0.0161f, 0.0576f, 0.0615f, 0.6157f);
			}

			SetTempoForHastySetting(hastyData, hastySetting);
			hastySetting.HastyData = hastyData;

			if (hastySetting is HastyFloat hastyFloat && hastyFloat.IsWhole)
			{
				hastySetting.HastyData.GameObject.GetComponentInChildren<Slider>().wholeNumbers = true;
			}
		}
	}

	private static HastyData GetTempoByHastySetting(IHastySetting hs)
	{
		try
		{
			return settingsMap.OfType<KeyValuePair<HastyData, IHastySetting>>().FirstOrDefault((KeyValuePair<HastyData, IHastySetting> kvp) => kvp.Key.HastySetting.Key == hs.Key).Key;
		}
		catch (Exception ex)
		{
			Debug.LogError($"Error in GetTempoByHastySetting: {ex}");
			return null!;
		}
	}

	private static void SetTempoForHastySetting(HastyData tempo, IHastySetting setting)
	{
		try
		{
			if (settingsMap.TryGetValue(tempo, out _))
			{ settingsMap.Remove(tempo); }
			settingsMap.Add(tempo, setting);
		}
		catch (Exception ex)
		{ Debug.LogError($"Error in SetTempoForHastySetting: {ex}"); }
	}
}

public class HastySettings
{
	private static AccessTools.FieldRef<HasteSettingsHandler, List<Setting>> settingsRef =
			AccessTools.FieldRefAccess<HasteSettingsHandler, List<Setting>>("settings");

	private static AccessTools.FieldRef<HasteSettingsHandler, ISettingsSaveLoad> settingsSaveLoadRef =
		AccessTools.FieldRefAccess<HasteSettingsHandler, ISettingsSaveLoad>("_settingsSaveLoad");

	public HastySettings(string modName)
	{
		if (string.IsNullOrEmpty(modName)) throw new ArgumentException("Mod name cannot be null or empty.", nameof(modName));
		ModName = modName;

		HarmonyLib.Harmony harmony = new(Guid.NewGuid().ToString());
		harmony.PatchAll(typeof(HastyPatch));
	}

	public string ModName { get; private set; }

	public void Add<T>(T setting) where T : Setting
	{
		HasteSettingsHandler handler = GameHandler.Instance.SettingsHandler;
		settingsRef(handler).Add(setting);
		setting.Load(settingsSaveLoadRef(handler));
		setting.ApplyValue();
	}

	internal LocalizedString CreateDisplayName(string name, string description = "") =>
		new UnlocalizedString(name + ((description == string.Empty) ? "" : ("\n<size=60%><alpha=#50>" + description)));
}
