using static System.Net.Mime.MediaTypeNames;

namespace Minesweeper.Models
{
	public class ErrorResponse
	{
		public ErrorResponse(string text)
		{
			error = text;
	}
		public string error;
	}
}
