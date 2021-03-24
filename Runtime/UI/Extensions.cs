using UnityEngine;
using System.Collections;

public static class Extensions
{
    public static Rect GetWorldRectInGUIForm(this RectTransform rectTransform) 
	{
		Vector3[] corners = new Vector3[4];

		rectTransform.GetWorldCorners(corners);

		return new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[1].y - corners[0].y);
	}

	public static Rect GetLocalRectInGUIForm(this RectTransform rectTransform) 
	{
		Vector3[] corners = new Vector3[4];

		rectTransform.GetLocalCorners(corners);
		/*
		for(int i = 0; i < 4; ++i) 
		{
			Debug.Log("local corners[" + i + "]" + corners[i]);
		}
		*/

		return new Rect(corners[0].x, corners[1].y, corners[2].x - corners[0].x, corners[1].y - corners[0].y);
	}
}
