using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolFreeByParticle : MonoBehaviour
{

	private void OnDisable()
	{
		ObjectPoolManager.Instance?.Free(gameObject);
	}

}
