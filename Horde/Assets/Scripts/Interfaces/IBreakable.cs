using UnityEngine;

public interface IBreakable 
{
	Transform Transform { get; }

	void Break();
	Vector3 GetPosition();
}
