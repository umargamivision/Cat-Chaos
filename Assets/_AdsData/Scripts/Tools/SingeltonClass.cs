﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//GenericSingletonClass
// Reference: http://www.unitygeek.com/unity_c_singleton 
// Excellent ideas on singelton must read from there. 
public class GenericSingletonClass<T> : MonoBehaviour where T : Component
{
	private static T instance;
	public static T Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<T> ();
				if (instance == null) {
					GameObject obj = new GameObject ();
					obj.name = typeof(T).Name;
					instance = obj.AddComponent<T>();
				}
			}
			return instance;
		}
	}

	public virtual void Awake ()
	{
		DontDestroyOnLoad (this.gameObject);
		if (instance == null) {
			instance = this as T;
			//DontDestroyOnLoad (this.gameObject);
		} else {
			Destroy (gameObject);
		}
	}
}