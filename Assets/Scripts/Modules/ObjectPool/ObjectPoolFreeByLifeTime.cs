using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolFreeByLifeTime : MonoBehaviour
{
	protected float currentLifeTime;
	public float lifeTime;

	private void Update()
	{
		currentLifeTime += Time.deltaTime;
		if(currentLifeTime >= lifeTime)
		{
			currentLifeTime = 0;
			ObjectPoolManager.Instance?.Free(gameObject);
		}
	}

	private void OnDisable()
	{
		currentLifeTime = 0;
	}

}
