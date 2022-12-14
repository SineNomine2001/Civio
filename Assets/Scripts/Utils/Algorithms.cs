using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Algorithms
{
	static Algorithms()
	{
		p = new int[512];
		for (int x = 0; x < 512; x++)
		{
			p[x] = permutation[x % 256];
		}
	}

	#region Perlin Noise
	private static readonly int[] permutation = { 151,160,137,91,90,15,
		131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
		190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
		88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
		77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
		102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
		135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
		5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
		223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
		129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
		251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
		49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
		138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
	};
	private static readonly int[] p;// Doubled permutation to avoid overflow

	public static float PerlinRun(float x, float y)
	{
		// Taking absolute value of the coordinates so negative values won't cause a glitch
		x = Mathf.Abs(x);
		y = Mathf.Abs(y);

		// Find the unit grid
		int xi = (int)x & 255;
		int yi = (int)y & 255;

		// Then find the distance
		float dx = x - Mathf.FloorToInt(x);
		float dy = y - Mathf.FloorToInt(y);

		//Prepare the values for interpolation
		//X-direction significance and Y-direction significance
		float xDirSig = Fade(dx);
		float yDirSig = Fade(dy);

		/* Then we need to get the gradient values for all 4 surrounding points
		The points are:
		(xi,yi)
		(xi,yi+1)
		(xi+1,yi)
		(xi+1,yi+1)
		First get the hash value, then interpret as vector

		Remember that, picking vector from hash and dot it with distance is integrated into a single function grad
		*/

		int hash_00, hash_10, hash_01, hash_11;
		hash_00 = p[p[xi] + yi];
		hash_10 = p[p[xi + 1] + yi];
		hash_01 = p[p[xi] + yi + 1];
		hash_11 = p[p[xi + 1] + yi + 1];

		float x0, x1;

		// The improved interpolation function is 6t^5 - 15t^4 + 10t^3
		// This function is used because it has 0 first and second order derivative at x=0 and x=1,
		// giving continuous values of first and second order derivatives
		x0 = Mathf.Lerp(GradientDotDistance(hash_00, dx, dy), GradientDotDistance(hash_10, dx - 1, dy), xDirSig);
		x1 = Mathf.Lerp(GradientDotDistance(hash_01, dx, dy - 1), GradientDotDistance(hash_11, dx - 1, dy - 1), xDirSig);

		return (Mathf.Lerp(x0, x1, yDirSig) + 1) / 2;
	}

	public static float[,] Perlin(Vector2Int mapSize, Vector2Int offset, int smoothness)
	{
		float[,] heights = new float[mapSize.x, mapSize.y];
		for (int x = 0; x < mapSize.x; x++)
		{
			for (int y = 0; y < mapSize.y; y++)
			{
				float xCoord = (float)x / mapSize.x * smoothness + offset.x;
				float yCoord = (float)y / mapSize.y * smoothness + offset.y;
				heights[x, y] = PerlinRun(xCoord, yCoord);
			}
		}
		return heights;
	}

	public static float Fade(float t)
	{
		return (float)(t * t * t * (t * (t * 6 - 15) + 10));         // 6t^5 - 15t^4 + 10t^3
	}

	public static float GradientDotDistance(int hash, float x, float y)
	{
		///<Summary>
		/// Returning the dot product of gradient vector and the distance vector
		///</Summary>
		switch (hash & 0x7) // bitwise AND operation of the binary value of hash and 7 (0000 0111) to give a value between 0 and 7
		{
			// gradient vectors are normalised to the same length
			case 0x0: return x * Mathf.Sqrt(2);
			case 0x1: return -x * Mathf.Sqrt(2);
			case 0x2: return x + y;
			case 0x3: return -x + y;
			case 0x4: return y * Mathf.Sqrt(2);
			case 0x5: return -y * Mathf.Sqrt(2);
			case 0x6: return x - y;
			case 0x7: return -x - y;
			default: return 0; // never happens
		}
	}
	#endregion

	#region Cellular Automata
	public static int[,] CellularRun(int[,] oldMap, Vector2Int mapSize, int[,][] setting)
	{
		int[,] newMap = new int[mapSize.x, mapSize.y];
		int neighb;
		BoundsInt myB = new BoundsInt(-1, -1, 0, 3, 3, 1);

		for (int x = 0; x < mapSize.x; x++)
		{
			for (int y = 0; y < mapSize.y; y++)
			{
				neighb = 0;
				foreach (var b in myB.allPositionsWithin)
				{
					if (b.x == 0 && b.y == 0) continue;

					if (x + b.x >= 0 && x + b.x < mapSize.x && y + b.y >= 0 && y + b.y < mapSize.y)
						neighb += oldMap[x + b.x, y + b.y];
					else neighb++;
				}

				if (oldMap[x, y] == 0)
				{
					if (neighb > setting[x,y][1])
						newMap[x, y] = 1;
					else newMap[x, y] = 0;
				}

				if (oldMap[x, y] == 1)
				{
					if (neighb < setting[x, y][2])
						newMap[x, y] = 0;
					else newMap[x, y] = 1;
				}
			}
		}
		return newMap;
	}

	public static int[,] Cellular(int[,] map, Vector2Int mapSize, int[,][] setting, int epoch)
	{
		for (int i = 0; i < epoch; i++)
		{
			map = CellularRun(map, mapSize, setting);
		}
		return map;
	}
	#endregion
}
