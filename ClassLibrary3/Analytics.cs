namespace ClassLibrary3;

/// <summary>
/// Предоставляет методы для аналитического расчета сумм доходов и налогов.
/// </summary>
public class Analytics
{
    /// <summary>
    /// Менеджер доходов, содержащий все записи о доходах.
    /// </summary>
    public IncomesManager manager { get; init; }

    /// <summary>
    /// Вычисляет общую сумму дохода (включая налог) за указанный месяц и год.
    /// </summary>
    /// <param name="year">Год, за который выполняется расчет.</param>
    /// <param name="month">Месяц, за который выполняется расчет.</param>
    /// <returns>Общая сумма дохода с налогом за указанный период.</returns>
    public double allMonthlyIncome(int year, int month)
    {
        double income = 0;
        foreach (var i in manager.incomes)
        {
            if (i.date.Year == year && i.date.Month == month) {income += i.countTax() + i.amount; }
        }
        return income;
    }

    /// <summary>
    /// Вычисляет общую сумму налога, уплаченного физическими лицами.
    /// </summary>
    /// <returns>Общая сумма налога для всех физических лиц.</returns>
    public double allIndividualsTax()
    {
        double individualTax = 0;
        foreach (var i in manager.incomes)
        {
            if (i.payer.getType() == PayerTypes.Individual){ individualTax += i.countTax(); }
        }
        return individualTax;
    }

    /// <summary>
    /// Вычисляет общую сумму налога, уплаченного юридическими лицами.
    /// </summary>
    /// <returns>Общая сумма налога для всех юридических лиц.</returns>
    public double allLegalsTax()
    {
        double legalTax = 0;
        foreach (var i in manager.incomes)
        {
            if (i.payer.getType() == PayerTypes.Legal)
            {
                legalTax += i.countTax();
            }
        }
        return legalTax;
    }

    /// <summary>
    /// Вычисляет общую сумму всех налогов по всем доходам.
    /// </summary>
    /// <returns>Общая сумма налогов.</returns>
    public double allTax()
    {
        double tax = 0;
        foreach (var i in manager.incomes)
        {
            tax += i.countTax();
        }
        return tax;
    }
}