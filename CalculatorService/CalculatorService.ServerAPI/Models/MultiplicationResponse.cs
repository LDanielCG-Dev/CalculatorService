namespace CalculatorService.ServerAPI.Models
{
	public class MultiplicationResponse
	{
		public double? Product { get; set; }

		public static MultiplicationResponse FromMultiplication(double product)
		{
			return new MultiplicationResponse { Product = product };
		}
	}
}
