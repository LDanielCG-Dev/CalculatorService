namespace CalculatorService.ServerAPI.Models
{
	public class SquareRootResponse
	{
		public double? Square { get; set; }

		public static SquareRootResponse FromSquareRoot(double square) 
		{
			return new SquareRootResponse { Square = square };
		}
	}
}
