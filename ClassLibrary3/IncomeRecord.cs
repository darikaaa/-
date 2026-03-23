namespace ClassLibrary3;

/// <summary>
/// Представляет запись о доходе, содержащую информацию о сумме, плательщике, дате и налоговой ставке.
/// </summary>
public class IncomeRecord
{
    /// <summary>
    /// Уникальный идентификатор записи о доходе.
    /// </summary>
    public Guid id { get; init; }
    
    /// <summary>
    /// Дата получения дохода.
    /// </summary>
    public DateTime date { get; init; }
    
    /// <summary>
    /// Сумма дохода.
    /// </summary>
    public double amount { get; init; }
    
    /// <summary>
    /// Плательщик, от которого получен доход.
    /// </summary>
    public Payer payer { get; init; }
    
    /// <summary>
    /// Налоговая ставка, применяемая к данному доходу.
    /// </summary>
    public TaxRate taxRate { get; init; } 

    /// <summary>
    /// Вычисляет сумму налога для данной записи о доходе.
    /// </summary>
    /// <returns>Сумма налога, рассчитанная как произведение суммы дохода на соответствующую налоговую ставку.</returns>
    public double countTax()
    {
        double rate = taxRate.getRate(payer.getType());
        return amount * rate;
    }
    
    /// <summary>
    /// Инициализирует новый экземпляр записи о доходе.
    /// </summary>
    /// <param name="date">Дата получения дохода.</param>
    /// <param name="amount">Сумма дохода.</param>
    /// <param name="payer">Плательщик.</param>
    /// <param name="taxRate">Налоговая ставка.</param>
    public IncomeRecord(DateTime date, double amount, Payer payer, TaxRate taxRate)
    {
        id = Guid.NewGuid();
        this.date = date;
        this.amount = amount;
        this.payer = payer;
        this.taxRate = taxRate;
    }
}