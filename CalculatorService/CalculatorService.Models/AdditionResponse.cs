namespace CalculatorService.Models
{
	public class AdditionResponse
	{
		public double Sum { get; set; }

		public static AdditionResponse FromAddition(double sum)
		{
			return new AdditionResponse { Sum = sum };
		}
	}
}
