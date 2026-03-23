namespace ClassLibrary3;

/// <summary>
/// Управляет коллекцией записей о доходах, обеспечивая добавление и фильтрацию записей.
/// </summary>
public class IncomesManager
{
    /// <summary>
    /// Список всех записей о доходах.
    /// </summary>
    public List<IncomeRecord> incomes { get; set; } = new List<IncomeRecord>();

    /// <summary>
    /// Добавляет новую запись о доходе в коллекцию.
    /// </summary>
    /// <param name="income">Запись о доходе для добавления.</param>
    public void addIncome(IncomeRecord income)
    {
        incomes.Add(income);
    }

    /// <summary>
    /// Возвращает все записи о доходах.
    /// </summary>
    /// <returns>Список всех записей о доходах.</returns>
    public List<IncomeRecord> getAllIncomes() {return incomes;}

    /// <summary>
    /// Возвращает записи о доходах за указанный месяц и год.
    /// </summary>
    /// <param name="year">Год для фильтрации.</param>
    /// <param name="month">Месяц для фильтрации.</param>
    /// <returns>Список записей о доходах за указанный период.</returns>
    public List<IncomeRecord> getIncomesByMonth(int year, int month)
    {
        List<IncomeRecord> inc = new List<IncomeRecord>();
        foreach (var i in incomes)
        {
            if (i.date.Month == month && i.date.Year == year) {inc.Add(i);}
        }
        return inc;
    }

    /// <summary>
    /// Возвращает записи о доходах по типу плательщика.
    /// </summary>
    /// <param name="type">Тип плательщика для фильтрации.</param>
    /// <returns>Список записей о доходах для указанного типа плательщика.</returns>
    public List<IncomeRecord> getIncomesByPayerType(PayerTypes type)
    {
        List<IncomeRecord> inc = new List<IncomeRecord>();
        foreach (var i in incomes)
        {
            if (i.payer.getType() == type) inc.Add(i);
        }
        return inc;
    }
}