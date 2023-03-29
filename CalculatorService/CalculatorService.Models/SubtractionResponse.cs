namespace CalculatorService.Models
{
	public class SubtractionResponse
	{
		public double Difference { get; set; }

		public static SubtractionResponse FromSubtraction(double difference)
		{
			return new SubtractionResponse { Difference = difference };
		}
	}
}
