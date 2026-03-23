namespace ClassLibrary3;

/// <summary>
/// Представляет плательщика с указанием его типа и наименования.
/// </summary>
public class Payer
{
    /// <summary>
    /// Тип плательщика (физическое или юридическое лицо).
    /// </summary>
    public PayerTypes payerType { get; init; }
    
    /// <summary>
    /// Наименование плательщика.
    /// </summary>
    public string name { get; init; }
    
    /// <summary>
    /// Возвращает тип плательщика.
    /// </summary>
    /// <returns>Тип плательщика.</returns>
    public PayerTypes getType(){return payerType;}
}