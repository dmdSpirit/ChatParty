﻿using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Component for hiding text panel, when text field is empty.
/// </summary>
public class PanelController : MonoBehaviour {
	public Text messageText;
	Image image;

	void Start(){
		image = GetComponent<Image> ();
	}

	void Update(){
		if (messageText.text.Trim () == "" && image.enabled)
			image.enabled = false;
		if (messageText.text.Trim () != "" && image.enabled == false)
			image.enabled = true;
	}
}
