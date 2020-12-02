using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class TransformExtensions {

	public static List<Transform> FindChildrenByTag(this Transform transform, params string[] tags)
	{
		List<Transform> list = new List<Transform>();
		foreach (var tran in transform.Cast<Transform>().ToList())
			list.AddRange(tran.FindChildrenByTag(tags)); // recursively check children
		if (tags.Any(tag => tag == transform.tag))
			list.Add(transform); // we matched, add this transform
		return list;
	}
	public static List<GameObject> FindChildrenByTag(this GameObject gameObject, params string[] tags)
	{
		return FindChildrenByTag(gameObject.transform, tags)
			//.Cast<GameObject>() // Can't use Cast here :(
			.Select(tran => tran.gameObject)
			.Where(gameOb => gameOb != null)
			.ToList();
	}
}
