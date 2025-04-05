namespace IntegrationService.Services
{
    using IntegrationService.DTO;
    using IntegrationService.Messaging.Http;
    using System.Globalization;
    using System.Text;

    public class ImportService : IImportService
    {
        private readonly ICategoryAccountClient _categoryAccountClient;
        private readonly ITransactionClient _transactionClient;

        public ImportService(ICategoryAccountClient categoryAccountClient, ITransactionClient transactionClient)
        {
            _categoryAccountClient = categoryAccountClient;
            _transactionClient = transactionClient;
        }

        public async Task<ImportResult> ImportTransactionsAsync(IFormFile csvFile, long userId, CancellationToken cancellationToken)
        {
            var result = new ImportResult();
            var csvTransactions = new List<CSVTransactionDTO>();

            var headerMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {

                { "Name", "Title" },
                { "TransactionName", "Title" },
                { "Transaction Name", "Title" },
                { "Title", "Title" },

                { "Category", "CategoryName" },
                { "Category Name", "CategoryName" },
                { "CategoryName", "CategoryName" },
            
                { "Account", "AccountName" },
                { "Account Name", "AccountName" },
                { "AccountName", "AccountName" },
            
                { "Transaction Date", "TransactionDate" },
                { "Date", "TransactionDate" },
                { "TransactionDate", "TransactionDate" },

                { "Value", "TransactionValue" },
                { "Transaction Value", "TransactionValue" },
                { "TransactionValue", "TransactionValue" },

                { "Currency", "Currency" },
                { "Description", "Description" },
                { "Merchant", "Merchant" },

                { "Type", "TransactionType"},
                { "TransactionType", "TransactionType" },
                { "Transaction Type", "TransactionType" },
            };

            using (var stream = csvFile.OpenReadStream())
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                var headerLine = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(headerLine))
                {
                    result.ErrorMessages.Add("Пустой заголовок CSV");
                    return result;
                }

                var headers = headerLine.Split(',').Select(h => h.Trim()).ToArray();
                
                var indexMapping = new Dictionary<int, string>();
                for (int i = 0; i < headers.Length; i++)
                {
                    if (headerMapping.TryGetValue(headers[i], out var fieldName))
                        indexMapping[i] = fieldName;
                }

                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var columns = line.Split(',');
                    var csvTransaction = new CSVTransactionDTO();

                    for (int i = 0; i < columns.Length; i++)
                    {
                        if (!indexMapping.ContainsKey(i))
                            continue;

                        var value = columns[i].Trim();
                        switch (indexMapping[i])
                        {
                            case "TransactionName":
                                csvTransaction.Title = value;
                                break;
                            case "TransactionDate":
                                if (DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                                    csvTransaction.TransactionDate = dt;
                                else
                                    csvTransaction.TransactionDate = DateTime.UtcNow;
                                break;
                            case "TransactionValue":
                                if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var val))
                                    csvTransaction.Value = val;
                                break;
                            case "TransactionCurrency":
                                csvTransaction.Currency = value;
                                break;
                            case "TransactionType":
                                csvTransaction.TransactionType = value;
                                break;
                            case "CategoryName":
                                csvTransaction.CategoryName = value;
                                break;
                            case "AccountName":
                                csvTransaction.AccountName = value;
                                break;
                            case "Description":
                                csvTransaction.Description = value;
                                break;
                            case "Merchant":
                                csvTransaction.Merchant = value;
                                break;
                        }
                    }

                    if (string.IsNullOrWhiteSpace(csvTransaction.Title) ||
                        csvTransaction.Value == 0 ||
                        string.IsNullOrWhiteSpace(csvTransaction.Currency) ||
                        string.IsNullOrWhiteSpace(csvTransaction.AccountName) ||
                        string.IsNullOrWhiteSpace(csvTransaction.CategoryName))
                    {
                        result.FailedCount++;
                        result.ErrorMessages.Add($"Неверные данные в строке: {line}");
                        continue;
                    }


                    csvTransactions.Add(csvTransaction);
                }
            }

            var csvExpenseCategoryNames = csvTransactions
                .Where(t => t.TransactionType != "" && (t.TransactionType != "INCOME" || t.Value < 0))
                .Select(t => t.CategoryName.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
            var csvIncomeCategoryNames = csvTransactions
                .Where(t => t.TransactionType != "" && (t.TransactionType != "EXPENSE" || t.Value > 0))
                .Select(t => t.CategoryName.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
            var csvAccountNames = csvTransactions
                .Where(t => !string.IsNullOrWhiteSpace(t.AccountName))
                .Select(t => t.AccountName.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var existingCategories = await _categoryAccountClient.GetCategoriesAsync(userId, cancellationToken);
            var existingAccounts = await _categoryAccountClient.GetAccountsAsync(userId, cancellationToken);

            var expenseCategoriesToCreate = csvExpenseCategoryNames
                .Where(name => !existingCategories.Any(c => c.CategoryName.Equals(name, StringComparison.OrdinalIgnoreCase)))
                .ToList();
            var incomeCategoriesToCreate = csvIncomeCategoryNames
                .Where(name => !existingCategories.Any(c => c.CategoryName.Equals(name, StringComparison.OrdinalIgnoreCase)))
                .ToList();
            var accountsToCreate = csvAccountNames
                .Where(name => !existingAccounts.Any(a => a.AccountName.Equals(name, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            foreach (var categoryName in expenseCategoriesToCreate)
            {
                try
                {
                    var categoryDto = new CreateCategoryRequest(userId, categoryName, "EXPENSE", "/assets/categories/settings.png");
                    var newCategory = await _categoryAccountClient.CreateCategoryAsync(categoryDto, cancellationToken);
                    existingCategories.Add(newCategory);
                }
                catch (Exception ex)
                {
                    result.ErrorMessages.Add($"Ошибка создания категории '{categoryName}': {ex.Message}");
                }
            }

            foreach (var categoryName in incomeCategoriesToCreate)
            {
                try
                {
                    var categoryDto = new CreateCategoryRequest(userId, categoryName, "INCOME", "/assets/categories/settings.png");
                    var newCategory = await _categoryAccountClient.CreateCategoryAsync(categoryDto, cancellationToken);
                    existingCategories.Add(newCategory);
                }
                catch (Exception ex)
                {
                    result.ErrorMessages.Add($"Ошибка создания категории '{categoryName}': {ex.Message}");
                }
            }

            foreach (var accountName in accountsToCreate)
            {
                try
                {
                    var accountDto = new CreateAccountRequest(userId, accountName, "CASH", "EUR", 0, 0, "", "#be6464");
                    var newAccount = await _categoryAccountClient.CreateAccountAsync(accountDto, cancellationToken);
                    existingAccounts.Add(newAccount);
                }
                catch (Exception ex)
                {
                    result.ErrorMessages.Add($"Ошибка создания счета '{accountName}': {ex.Message}");
                }
            }

            foreach (var csvTran in csvTransactions)
            {
                try
                {
                    
                    var category = existingCategories.FirstOrDefault(c => c.CategoryName.Equals(csvTran.CategoryName?.Trim(), StringComparison.OrdinalIgnoreCase));
                    var account = existingAccounts.FirstOrDefault(a => a.AccountName.Equals(csvTran.AccountName?.Trim(), StringComparison.OrdinalIgnoreCase));

                    
                    if (category == null || account == null)
                    {
                        result.FailedCount++;
                        result.ErrorMessages.Add($"Отсутствует категория или счет для транзакции '{csvTran.Title}'");
                        continue;
                    }

                   
                    var tranDate = csvTran.TransactionDate ?? DateTime.UtcNow;
                    var tranType = csvTran.TransactionType ?? (csvTran.Value>0 ? "INCOME" : "EXPENSE");

                    var transactionCreateDto = new CreateTransactionRequest(csvTran.Value, csvTran.Title, csvTran.Currency,
                                                                            category.Id, account.Id, userId, csvTran.Description ?? "", "",
                                                                            tranDate, DateTime.UtcNow, tranType, csvTran.Merchant ?? ""); 
                                                                           

                    var created = await _transactionClient.CreateTransactionAsync(transactionCreateDto, cancellationToken);
                    if (created)
                        result.SuccessfulCount++;
                    else
                    {
                        result.FailedCount++;
                        result.ErrorMessages.Add($"Не удалось создать транзакцию '{csvTran.Title}'");
                    }
                }
                catch (Exception ex)
                {
                    result.FailedCount++;
                    result.ErrorMessages.Add($"Ошибка создания транзакции '{csvTran.Title}': {ex.Message}");
                }
            }

            return result;
        }
    }

}
