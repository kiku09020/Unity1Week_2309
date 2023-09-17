using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Extention.ObjectPool
{
    public class PooledObject<T> : MonoBehaviour where T: PooledObject<T>
    {
		protected IObjectPool<T> pool;      // 自身が格納されるオブジェクトプール

		bool isReleased;

		public event System.Action OnGettedEvent;
		public event System.Action OnReleasedEvent;

		//--------------------------------------------------
		/// <summary> 初期化 </summary>
		public virtual void OnCreated(IObjectPool<T> pool)
		{
			this.pool = pool;
			OnGetted();
		}

		/// <summary> 取得されたときの処理 </summary>
		public virtual void OnGetted()
		{
			OnGettedEvent?.Invoke();

			gameObject.SetActive(true);
			isReleased = false;
		}

		public virtual void OnReleased()
		{
			OnReleasedEvent?.Invoke();

			gameObject.SetActive(false);
			isReleased = true;
		}

		public void Release()
		{
			if (!isReleased) {
				pool?.Release(this as T);
			}
		}
	}
}