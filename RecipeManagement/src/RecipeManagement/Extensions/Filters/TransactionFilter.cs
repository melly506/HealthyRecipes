namespace RecipeManagement.Extensions.Filters;

using System;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Serilog;


public class TransactionAttribute : TypeFilterAttribute
{
    public TransactionAttribute() : base(typeof(TransactionFilter))
    {
    }

    public class TransactionFilter : IAsyncActionFilter
    {
        private readonly ILogger<TransactionFilter> _logger;

        public TransactionFilter(ILogger<TransactionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controllerName = context.RouteData.Values["controller"]?.ToString();
            var actionName = context.RouteData.Values["action"]?.ToString();
            var transactionId = Guid.NewGuid().ToString();

            _logger.LogInformation("Starting transaction {TransactionId} for {Controller}.{Action}",
                transactionId, controllerName, actionName);

            using var transactionScope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var executedContext = await next();

                if (executedContext.Exception == null)
                {
                    transactionScope.Complete();
                    _logger.LogInformation("Transaction {TransactionId} completed successfully for {Controller}.{Action}",
                        transactionId, controllerName, actionName);
                }
                else
                {
                    _logger.LogError(executedContext.Exception,
                        "Transaction {TransactionId} failed for {Controller}.{Action} with error: {ErrorMessage}",
                        transactionId, controllerName, actionName, executedContext.Exception.Message);
                    // Transaction will automatically roll back
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Unhandled exception in transaction {TransactionId} for {Controller}.{Action}: {ErrorMessage}",
                    transactionId, controllerName, actionName, ex.Message);
                throw new TransactionException($"Transaction failed: {ex.Message}", ex);
            }
        }
    }
}
