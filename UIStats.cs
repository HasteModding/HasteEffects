namespace HasteEffects;

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
