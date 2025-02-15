using UnityEngine;

namespace SVS
{
	[CreateAssetMenu(menuName = "ProceduralCity/Rule")]
	public class Rule : ScriptableObject
	{
		public string letter;
		public string[] results = null;
		public bool randomResult = false;

		public string GetResult()
		{
			if (randomResult)
			{
				int randomIndex = UnityEngine.Random.Range(0, results.Length);
				return results[randomIndex];
			}
			return results[0];
		}

	}
}

