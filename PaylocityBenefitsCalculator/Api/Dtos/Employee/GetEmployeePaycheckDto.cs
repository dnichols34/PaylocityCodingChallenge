namespace Api.Dtos.Employee
{
    public class GetEmployeePaycheckDto
    {
        public string MonthyNetPay { get; set; }
        public string MonthyPayAfterBenefits { get; set; }
        public string MonthyBenefitCost { get; set; }
        public string DependentsUnderFifty { get; set; }
        public string DependentsOverFifty { get; set; }


    }
}
