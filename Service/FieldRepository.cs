using Minesweeper.Models;

namespace Minesweeper.Service
{
	public class FieldRepository
	{
		public string [,] CreateField(int width, int height)
		{
			string[,] array = new string[width, height];
			for(int i = 0; width > i; i++)
			{
				for(int j = 0; height > j; j++)
				{
					array[i, j] = " ";
				}
			}
			return array;
		}
		public int[,] CreateFieldInt(int width, int height)
		{
			int[,] array = new int[width, height];
			for (int i = 0; width > i; i++)
			{
				for (int j = 0; height > j; j++)
				{
					array[i, j] = 0;
				}
			}
			return array;
		}
		public int[,] GenerateMines(int[,] array,int mines_count, int width, int height)
		{
			int x = 0;
			int y = 0;
			for (int i = 0; mines_count > i; i++)
			{
				x = Random.Shared.Next(width);
				y = Random.Shared.Next(height);
				if (array[x, y] !=9)
				{
					array[x, y] = 9;
				}
				else
				{
					i--;
				}
				for (int v = Math.Max(x - 1, 0); v <= Math.Min(x + 1, width - 1); v++)
				{
					for (int j = Math.Max(y - 1, 0); j <= Math.Min(y + 1, height - 1); j++)
					{
						if (array[v, j]!=9)
						{
							array[v, j] ++;
						}
					}
				}
			}
			return array;
		}
		public void UncoverField(int width, int height, int[,] array, string[,] field)
		{
			if (width >= 0 && height >= 0 &&
				width < field.GetLength(0) && height < field.GetLength(1))
			{
				if (array[width, height] == 0)
				{
					array[width, height] = -10;
					field[width, height] = "0";
					for (int y = -1; y <= 1; y++)
						for (int x = -1; x <= 1; x++)
							UncoverField(width + y, height + x, array, field);
				}
				else if (array[width, height] > 0 && array[width, height] < 9)
				{
					array[width, height] *= -1;
					field[width, height] = Math.Abs(array[width,height]).ToString();
				}
					
			}
		}
		public string[,] ArrayProjection(string[,] array1, int[,] array2)
		{
			int witdh = array1.GetLength(0);
			int height = array1.GetLength(1);

			for (int x = 0; x < witdh; x++)
			{
				for (int y = 0; y < height; y++)
				{
					if(array2[x, y] == 9)
					{
						array1[x, y] = "X";
					}
					else if(array2[x, y] == -10)
					{
						array1[x, y] = "0";
					}
					else if(array2[x, y] < 0)
					{
						array2[x, y] *= -1;
						array1[x, y] = array2[x, y].ToString();
					}
					else
					{
						array1[x, y] = array2[x, y].ToString();
					}
					
				}
			}
			return array1;
		}
		public bool isEqual(GameInfoResponse gameInfoResponse)
		{
			bool isEqual = false;
			int count = 0;
			for (int x = 0; x < gameInfoResponse.field.GetLength(0); x++)
			{
				for (int y = 0; y < gameInfoResponse.field.GetLength(1); y++)
				{
					if (gameInfoResponse.field[x, y].Contains(" "))
					{
						count++;
					}
				}
			}
			if (gameInfoResponse.mines_count == count)
			{
				isEqual = true;
			}
			
			return isEqual;
		}
	}
}
