using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace FrenzyFactory.UI
{
    public class GamePortal : MonoBehaviour
    {
        public static GamePortal Instance => instance;
        private static GamePortal instance;

        private bool gameInProgress;

		private void Awake()
		{
			if (instance != null && instance != this)
			{
				Destroy(gameObject);
				return;
			}

			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}
}
