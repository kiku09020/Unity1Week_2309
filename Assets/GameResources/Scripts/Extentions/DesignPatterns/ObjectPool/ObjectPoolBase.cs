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

		/// <summary> �v�[���̏��������� </summary>
		protected abstract void Init(bool isCheck, int defaultCapacity, int maxSize);

		/// <summary> �I�u�W�F�N�g�𐶐�����Ƃ��̏��� </summary>
		protected abstract T OnCreate(T prefab, Transform parent);

		/// <summary> �I�u�W�F�N�g���v�[������擾����Ƃ��̏��� </summary>
		protected virtual void OnGetFromPool(T obj)
		{
			obj.OnGetted();
		}

		/// <summary> �I�u�W�F�N�g���v�[���ɕԂ��Ƃ��̏��� </summary>
		protected virtual void OnReleaseToPool(T obj)
		{
			obj.OnReleased();
		}

		/// <summary> �v�[�����̃I�u�W�F�N�g���폜����Ƃ��̏��� </summary>
		protected virtual void OnDestroyObject(T obj)
		{
			Destroy(obj.gameObject);
		}
	}
}