
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using ClassLibrary3;

namespace ClassLibrary3.Tests
{
    public class AnalyticsTests
    {
        private IncomesManager CreateTestManager()
        {
            var manager = new IncomesManager();
            
            var taxRate = new TaxRate 
            { 
                individualRate = 0.13,  // 13% для физлиц
                legalRate = 0.20        // 20% для юрлиц
            };
            
            var individual = new Payer { payerType = PayerTypes.Individual, name = "Иванов И.И." };
            var legal = new Payer { payerType = PayerTypes.Legal, name = "ООО Ромашка" };
            
            // Добавляем тестовые данные
            manager.addIncome(new IncomeRecord(new DateTime(2024, 1, 15), 10000, individual, taxRate));
            manager.addIncome(new IncomeRecord(new DateTime(2024, 1, 20), 20000, legal, taxRate));
            manager.addIncome(new IncomeRecord(new DateTime(2024, 2, 10), 15000, individual, taxRate));
            manager.addIncome(new IncomeRecord(new DateTime(2024, 2, 25), 25000, legal, taxRate));
            manager.addIncome(new IncomeRecord(new DateTime(2023, 12, 5), 5000, individual, taxRate));
            
            return manager;
        }
        
        [Fact]
        public void AllMonthlyIncome_ShouldReturnCorrectSumForMonth()
        {
            // Arrange
            var manager = CreateTestManager();
            var analytics = new Analytics { manager = manager };
            
            // Act
            var result = analytics.allMonthlyIncome(2024, 1);
            
            // Assert
            // Доход: 10000 + 20000 = 30000
            // Налоги: 10000*0.13=1300, 20000*0.20=4000, итого 5300
            // Общая сумма с налогом: 30000 + 5300 = 35300
            Assert.Equal(35300, result);
        }
        
        [Fact]
        public void AllMonthlyIncome_WhenNoIncomes_ShouldReturnZero()
        {
            // Arrange
            var manager = new IncomesManager();
            var analytics = new Analytics { manager = manager };
            
            // Act
            var result = analytics.allMonthlyIncome(2024, 1);
            
            // Assert
            Assert.Equal(0, result);
        }
        
        [Fact]
        public void AllMonthlyIncome_ShouldOnlyIncludeSpecifiedMonthAndYear()
        {
            // Arrange
            var manager = CreateTestManager();
            var analytics = new Analytics { manager = manager };
            
            // Act
            var resultJanuary2024 = analytics.allMonthlyIncome(2024, 1);
            var resultFebruary2024 = analytics.allMonthlyIncome(2024, 2);
            var resultDecember2023 = analytics.allMonthlyIncome(2023, 12);
            
            // Assert
            Assert.Equal(35300, resultJanuary2024);  // Январь 2024
            Assert.Equal(53000, resultFebruary2024); // Февраль 2024
            Assert.Equal(5650, resultDecember2023);  // Декабрь 2023: 5000 + (5000*0.13=650)
        }
        
        [Fact]
        public void AllIndividualsTax_ShouldReturnSumOfIndividualTaxes()
        {
            // Arrange
            var manager = CreateTestManager();
            var analytics = new Analytics { manager = manager };
             
            // Act
            var result = analytics.allIndividualsTax();
            
            // Assert
            // Налоги с физлиц: 10000*0.13=1300 + 15000*0.13=1950 + 5000*0.13=650 = 3900
            Assert.Equal(3900, result);
        }
        
        [Fact]
        public void AllIndividualsTax_WhenNoIndividuals_ShouldReturnZero()
        {
            // Arrange
            var manager = new IncomesManager();
            var taxRate = new TaxRate { individualRate = 0.13, legalRate = 0.20 };
            var legal = new Payer { payerType = PayerTypes.Legal, name = "ООО Тест" };
            
            manager.addIncome(new IncomeRecord(DateTime.Now, 10000, legal, taxRate));
            
            var analytics = new Analytics { manager = manager };
            
            // Act
            var result = analytics.allIndividualsTax();
            
            // Assert
            Assert.Equal(0, result);
        }
        
        [Fact]
        public void AllLegalsTax_ShouldReturnSumOfLegalTaxes()
        {
            // Arrange
            var manager = CreateTestManager();
            var analytics = new Analytics { manager = manager };
            
            // Act
            var result = analytics.allLegalsTax();
            
            // Assert
            // Налоги с юрлиц: 20000*0.20=4000 + 25000*0.20=5000 = 9000
            Assert.Equal(9000, result);
        }
        
        [Fact]
        public void AllLegalsTax_WhenNoLegals_ShouldReturnZero()
        {
            // Arrange
            var manager = new IncomesManager();
            var taxRate = new TaxRate { individualRate = 0.13, legalRate = 0.20 };
            var individual = new Payer { payerType = PayerTypes.Individual, name = "Иванов И.И." };
            
            manager.addIncome(new IncomeRecord(DateTime.Now, 10000, individual, taxRate));
            
            var analytics = new Analytics { manager = manager };
            
            // Act
            var result = analytics.allLegalsTax();
            
            // Assert
            Assert.Equal(0, result);
        }
        
        [Fact]
        public void AllTax_ShouldReturnSumOfAllTaxes()
        {
            // Arrange
            var manager = CreateTestManager();
            var analytics = new Analytics { manager = manager };
            
            // Act
            var result = analytics.allTax();
            
            // Assert
            // Налоги с физлиц: 3900 + налоги с юрлиц: 9000 = 12900
            Assert.Equal(12900, result);
        }
        
        [Fact]
        public void AllTax_WhenEmptyCollection_ShouldReturnZero()
        {
            // Arrange
            var manager = new IncomesManager();
            var analytics = new Analytics { manager = manager };
            
            // Act
            var result = analytics.allTax();
            
            // Assert
            Assert.Equal(0, result);
        }
        
        [Fact]
        public void Analytics_WithDifferentTaxRates_ShouldCalculateCorrectly()
        {
            // Arrange
            var manager = new IncomesManager();
            var taxRate1 = new TaxRate { individualRate = 0.13, legalRate = 0.20 };
            var taxRate2 = new TaxRate { individualRate = 0.15, legalRate = 0.25 };
            
            var individual = new Payer { payerType = PayerTypes.Individual, name = "Иванов И.И." };
            var legal = new Payer { payerType = PayerTypes.Legal, name = "ООО Ромашка" };
            
            manager.addIncome(new IncomeRecord(new DateTime(2024, 1, 1), 10000, individual, taxRate1));
            manager.addIncome(new IncomeRecord(new DateTime(2024, 1, 1), 20000, legal, taxRate2));
            
            var analytics = new Analytics { manager = manager };
            
            // Act
            var individualTax = analytics.allIndividualsTax();
            var legalTax = analytics.allLegalsTax();
            var totalTax = analytics.allTax();
            
            // Assert
            Assert.Equal(1300, individualTax);  // 10000 * 0.13
            Assert.Equal(5000, legalTax);       // 20000 * 0.25
            Assert.Equal(6300, totalTax);
        }
    }
    
    public class IncomeRecordTests
    {
        [Fact]
        public void CountTax_ForIndividual_ShouldCalculateCorrectly()
        {
            // Arrange
            var taxRate = new TaxRate { individualRate = 0.13, legalRate = 0.20 };
            var payer = new Payer { payerType = PayerTypes.Individual, name = "Тестовый" };
            var income = new IncomeRecord(DateTime.Now, 1000, payer, taxRate);
            
            // Act
            var tax = income.countTax();
            
            // Assert
            Assert.Equal(130, tax);
        }
        
        [Fact]
        public void CountTax_ForLegal_ShouldCalculateCorrectly()
        {
            // Arrange
            var taxRate = new TaxRate { individualRate = 0.13, legalRate = 0.20 };
            var payer = new Payer { payerType = PayerTypes.Legal, name = "Тестовый" };
            var income = new IncomeRecord(DateTime.Now, 1000, payer, taxRate);
            
            // Act
            var tax = income.countTax();
            
            // Assert
            Assert.Equal(200, tax);
        }
        
        [Fact]
        public void IncomeRecord_Constructor_ShouldGenerateUniqueId()
        {
            // Arrange & Act
            var taxRate = new TaxRate { individualRate = 0.13, legalRate = 0.20 };
            var payer = new Payer { payerType = PayerTypes.Individual, name = "Тестовый" };
            var income1 = new IncomeRecord(DateTime.Now, 1000, payer, taxRate);
            var income2 = new IncomeRecord(DateTime.Now, 2000, payer, taxRate);
            
            // Assert
            Assert.NotEqual(income1.id, income2.id);
            Assert.NotEqual(Guid.Empty, income1.id);
            Assert.NotEqual(Guid.Empty, income2.id);
        }
    }
    
    public class IncomesManagerTests
    {
        [Fact]
        public void AddIncome_ShouldAddToCollection()
        {
            // Arrange
            var manager = new IncomesManager();
            var taxRate = new TaxRate { individualRate = 0.13, legalRate = 0.20 };
            var payer = new Payer { payerType = PayerTypes.Individual, name = "Тестовый" };
            var income = new IncomeRecord(DateTime.Now, 1000, payer, taxRate);
            
            // Act
            manager.addIncome(income);
            
            // Assert
            Assert.Single(manager.incomes);
            Assert.Contains(income, manager.incomes);
        }
        
        [Fact]
        public void GetAllIncomes_ShouldReturnAllIncomes()
        {
            // Arrange
            var manager = new IncomesManager();
            var taxRate = new TaxRate { individualRate = 0.13, legalRate = 0.20 };
            var payer = new Payer { payerType = PayerTypes.Individual, name = "Тестовый" };
            var income1 = new IncomeRecord(DateTime.Now, 1000, payer, taxRate);
            var income2 = new IncomeRecord(DateTime.Now, 2000, payer, taxRate);
            
            manager.addIncome(income1);
            manager.addIncome(income2);
            
            // Act
            var result = manager.getAllIncomes();
            
            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(income1, result);
            Assert.Contains(income2, result);
        }
        
        [Fact]
        public void GetIncomesByMonth_ShouldFilterCorrectly()
        {
            // Arrange
            var manager = new IncomesManager();
            var taxRate = new TaxRate { individualRate = 0.13, legalRate = 0.20 };
            var payer = new Payer { payerType = PayerTypes.Individual, name = "Тестовый" };
            
            var income1 = new IncomeRecord(new DateTime(2024, 1, 15), 1000, payer, taxRate);
            var income2 = new IncomeRecord(new DateTime(2024, 1, 20), 2000, payer, taxRate);
            var income3 = new IncomeRecord(new DateTime(2024, 2, 10), 3000, payer, taxRate);
            
            manager.addIncome(income1);
            manager.addIncome(income2);
            manager.addIncome(income3);
            
            // Act
            var januaryIncomes = manager.getIncomesByMonth(2024, 1);
            var februaryIncomes = manager.getIncomesByMonth(2024, 2);
            var decemberIncomes = manager.getIncomesByMonth(2024, 12);
            
            // Assert
            Assert.Equal(2, januaryIncomes.Count);
            Assert.Single(februaryIncomes);
            Assert.Empty(decemberIncomes);
            Assert.Contains(income1, januaryIncomes);
            Assert.Contains(income2, januaryIncomes);
            Assert.Contains(income3, februaryIncomes);
        }
        
        [Fact]
        public void GetIncomesByPayerType_ShouldFilterCorrectly()
        {
            // Arrange
            var manager = new IncomesManager();
            var taxRate = new TaxRate { individualRate = 0.13, legalRate = 0.20 };
            var individual = new Payer { payerType = PayerTypes.Individual, name = "Иванов" };
            var legal = new Payer { payerType = PayerTypes.Legal, name = "ООО" };
            
            var income1 = new IncomeRecord(DateTime.Now, 1000, individual, taxRate);
            var income2 = new IncomeRecord(DateTime.Now, 2000, legal, taxRate);
            var income3 = new IncomeRecord(DateTime.Now, 3000, individual, taxRate);
            
            manager.addIncome(income1);
            manager.addIncome(income2);
            manager.addIncome(income3);
            
            // Act
            var individualIncomes = manager.getIncomesByPayerType(PayerTypes.Individual);
            var legalIncomes = manager.getIncomesByPayerType(PayerTypes.Legal);
            
            // Assert
            Assert.Equal(2, individualIncomes.Count);
            Assert.Single(legalIncomes);
            Assert.Contains(income1, individualIncomes);
            Assert.Contains(income3, individualIncomes);
            Assert.Contains(income2, legalIncomes);
        }
        
        [Fact]
        public void GetIncomesByPayerType_WhenNoMatches_ShouldReturnEmptyList()
        {
            // Arrange
            var manager = new IncomesManager();
            var taxRate = new TaxRate { individualRate = 0.13, legalRate = 0.20 };
            var individual = new Payer { payerType = PayerTypes.Individual, name = "Иванов" };
            
            var income = new IncomeRecord(DateTime.Now, 1000, individual, taxRate);
            manager.addIncome(income);
            
            // Act
            var result = manager.getIncomesByPayerType(PayerTypes.Legal);
            
            // Assert
            Assert.Empty(result);
        }
    }
    
    public class TaxRateTests
    {
        [Fact]
        public void GetRate_ForIndividual_ShouldReturnIndividualRate()
        {
            // Arrange
            var taxRate = new TaxRate { individualRate = 0.13, legalRate = 0.20 };
            
            // Act
            var rate = taxRate.getRate(PayerTypes.Individual);
            
            // Assert
            Assert.Equal(0.13, rate);
        }
        
        [Fact]
        public void GetRate_ForLegal_ShouldReturnLegalRate()
        {
            // Arrange
            var taxRate = new TaxRate { individualRate = 0.13, legalRate = 0.20 };
            
            // Act
            var rate = taxRate.getRate(PayerTypes.Legal);
            
            // Assert
            Assert.Equal(0.20, rate);
        }
        
        [Fact]
        public void GetRate_WhenRatesAreChanged_ShouldReturnUpdatedRates()
        {
            // Arrange
            var taxRate = new TaxRate { individualRate = 0.13, legalRate = 0.20 };
            
            // Act
            taxRate.individualRate = 0.15;
            taxRate.legalRate = 0.25;
            
            // Assert
            Assert.Equal(0.15, taxRate.getRate(PayerTypes.Individual));
            Assert.Equal(0.25, taxRate.getRate(PayerTypes.Legal));
        }
    }
    
    public class PayerTests
    {
        [Fact]
        public void GetType_ShouldReturnCorrectType()
        {
            // Arrange
            var individual = new Payer { payerType = PayerTypes.Individual, name = "Иванов" };
            var legal = new Payer { payerType = PayerTypes.Legal, name = "ООО" };
            
            // Act & Assert
            Assert.Equal(PayerTypes.Individual, individual.getType());
            Assert.Equal(PayerTypes.Legal, legal.getType());
        }
        
        [Fact]
        public void Payer_Properties_ShouldBeSetCorrectly()
        {
            // Arrange & Act
            var payer = new Payer 
            { 
                payerType = PayerTypes.Individual, 
                name = "Петров П.П." 
            };
            
            // Assert
            Assert.Equal(PayerTypes.Individual, payer.payerType);
            Assert.Equal("Петров П.П.", payer.name);
        }
    }
}