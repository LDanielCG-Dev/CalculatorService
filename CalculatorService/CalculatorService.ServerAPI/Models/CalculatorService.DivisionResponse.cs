namespace CalculatorService.ServerAPI.Models
{
	public class DivisionResponse
	{
		public double? Quotient { get; set; }
		public double? Remainder { get; set; }

		public static DivisionResponse FromDivision(double quotient, double remainder)
		{
			return new DivisionResponse { Quotient = quotient, Remainder = remainder };
		}
	}

}
