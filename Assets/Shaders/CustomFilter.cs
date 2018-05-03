using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CustomFilter : MonoBehaviour {

	public Material materialToUse;

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit (source, destination,materialToUse);
	}
}
