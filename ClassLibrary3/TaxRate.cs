namespace ClassLibrary3;

/// <summary>
/// Представляет налоговые ставки для различных типов плательщиков.
/// </summary>
public class TaxRate
{
    /// <summary>Налоговая ставка для физических лиц.</summary>
    public double individualRate { get; set; }
    
    /// <summary>Налоговая ставка для юридических лиц.</summary>
    public double legalRate { get; set; }

    /// <summary>
    /// Возвращает налоговую ставку в зависимости от типа плательщика.
    /// </summary>
    /// <param name="type">Тип плательщика.</param>
    /// <returns>Налоговая ставка для указанного типа плательщика.</returns>
    public double getRate(PayerTypes type)
    {
        if (type == PayerTypes.Individual) return individualRate;
        if (type == PayerTypes.Legal) return legalRate;
        return 0;
    }
}