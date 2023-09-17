using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extention.ObjectPool
{
    public abstract class ObjectPoolBase<T> : MonoBehaviour where T: PooledObject<T>
    {
		[Header("ObjectPool")]
		[SerializeField] bool checkCollection = true;
		[SerializeField] int defaultCapacity = 20;
		[SerializeField] int maxSize = 100;

		//--------------------------------------------------
		protected virtual void Awake()
		{
			Init(checkCollection, defaultCapacity, maxSize);
		}

		/// <summary> プールの初期化処理 </summary>
		protected abstract void Init(bool isCheck, int defaultCapacity, int maxSize);

		/// <summary> オブジェクトを生成するときの処理 </summary>
		protected abstract T OnCreate(T prefab, Transform parent);

		/// <summary> オブジェクトをプールから取得するときの処理 </summary>
		protected virtual void OnGetFromPool(T obj)
		{
			obj.OnGetted();
		}

		/// <summary> オブジェクトをプールに返すときの処理 </summary>
		protected virtual void OnReleaseToPool(T obj)
		{
			obj.OnReleased();
		}

		/// <summary> プール内のオブジェクトを削除するときの処理 </summary>
		protected virtual void OnDestroyObject(T obj)
		{
			Destroy(obj.gameObject);
		}
	}
}