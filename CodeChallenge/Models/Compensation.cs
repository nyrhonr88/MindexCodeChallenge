using System;

namespace CodeChallenge.Models;

public class Compensation
{
    //removing this because it's easier to test than having to pass a fully formed Employee 
    public Employee Employee { get; set; }

    public decimal Salary { get; set; }

    public DateTime EffectiveDate { get; set; }

    public string EmployeeId { get; set; }

    public string Id { get; set; }
}