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
			UnityEditor.Handles.Label(enemy.transform.position + heightOffset, enemy.ToString(), style); 
		}
	}
}
