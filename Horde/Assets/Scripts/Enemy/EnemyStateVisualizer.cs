using UnityEngine;

public class EnemyStateVisualizer : MonoBehaviour 
{
	[SerializeField] private Vector3 heightOffset = Vector3.up;
	[SerializeField] GUIStyle style;

	private Enemy[] enemies;

	private void Start()
	{
		//style = new GUIStyle();
	}

	private void OnDrawGizmos()
	{
		if(enemies == null)
			enemies = FindObjectsOfType<Enemy>();
		
		foreach(var enemy in enemies)
		{
			string stateString = "NONE";
			if(enemy.GetCurrentState() != null)
			{
				stateString = enemy.GetCurrentState().ToString();
			}

#if UNITY_EDITOR
            UnityEditor.Handles.Label(enemy.transform.position + heightOffset, stateString, style);
#endif
        }
	}
}
