using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using NaughtyAttributes;

namespace Chess_Client
{
	public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
	{
		[Header("Singleton - Config")]
		[SerializeField]
		private bool activeInAllScenes;

		[Header("Singleton - Status")]
		[SerializeField, ReadOnly]
		private string spawnedSceneName;
		
		private static T instance;

		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindFirstObjectByType<T>();

					if (instance == null)
					{
						instance = new GameObject("Singleton_" + typeof(T).Name).AddComponent<T>();
					}
				}

				return instance;
			}
		}

		/// <summary>
		/// Singleton 패턴 적용을 위해 base.Awake() 메소드가 먼저 실행되어야 합니다.
		/// </summary>
		protected virtual void Awake()
		{
			T[] duplicate = FindObjectsByType<T>(FindObjectsSortMode.None);

			if (duplicate.Length > 1)
			{
				Debug.LogError($"[{typeof(T)}] Another script is already running!");
				Destroy(gameObject);
				return;
			}

			DontDestroyOnLoad(gameObject);
			
			spawnedSceneName = SceneManager.GetActiveScene().name;
		}

		protected virtual void Update()
		{
			if (!activeInAllScenes)
			{
				if (!spawnedSceneName.Equals(SceneManager.GetActiveScene().name))
				{
					Destroy(gameObject);
					return;
				}
			}
		}
	}
}
