namespace CalculatorService.ServerAPI.Models
{
	public class DivisionResponse
	{
		public int? Quotient { get; set; }
		public int? Remainder { get; set; }

		public static DivisionResponse FromDivision(int quotient, int remainder)
		{
			return new DivisionResponse { Quotient = quotient, Remainder = remainder };
		}
	}

}
