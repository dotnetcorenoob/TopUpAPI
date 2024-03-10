using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using topUpApi.Entities;
using topUpApi.Services;

namespace topUpApi.Controllers
{
    // TransactionsController.cs
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITopUpRepository _repository;
        private readonly ITopUpService _topUpService;
        private readonly IBalanceService _balanceService;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(ITopUpRepository repository,
            IBalanceService balanceService,
            ITopUpService topUpService,
            ILogger<TransactionsController> logger)
        {
            _repository = repository;
            _balanceService = balanceService;
            _topUpService = topUpService;
            _logger = logger;
        }

        [HttpGet("GetTopUpOptions")]
        public ActionResult<IEnumerable<TopUpOptions>> GetTopUpOptions()
        {
            var beneficiaries = _repository.GetTopUpOptions();
            return Ok(beneficiaries);
        }


        [HttpPost("topup")]
        public ActionResult TopUp(TopUpTransactionModel transaction)
        {
            _logger.LogInformation("This Top transaction initiated for the user. " + transaction.UserId);
            // Check if the user is verified
            bool isVerified = false/* Logic to check user verification */;

            int currentUserId = transaction.UserId;
            // Check top-up limits based on verification status
            decimal maxTopUpPerBeneficiary = isVerified ? 500.00m : 1000.00m;

            if ( _repository.GetBeneficiaryById(transaction.BeneficiaryId, transaction.UserId) ==null)
            {
                return BadRequest($"Invalid Beneficiary Id for the user/ user not exists.");
            }
            

            // Check if the transaction amount is valid
            if (transaction.Amount <= 0 || transaction.Amount > maxTopUpPerBeneficiary)
            {
                return BadRequest($"Invalid top-up amount. Must be between 1 and {maxTopUpPerBeneficiary}.");
            }
            _logger.LogInformation(" Check if the user can top up based on the monthly limit");
            // Check if the user can top up based on the monthly limit
            if (!_topUpService.CanTopUp(currentUserId, transaction.Amount))
            {
                return BadRequest("Top-up amount exceeds the monthly limit.");
            }
            _logger.LogInformation("Check if the user has sufficient balance");
            // Check if the user has sufficient balance
            var userBalance = _balanceService.GetBalance(currentUserId);
            if (transaction.Amount + 1 > userBalance.Result) // Including the AED 1 charge
            {
                return BadRequest("Insufficient balance.");
            }
            _logger.LogInformation("Perform Top-up transaction");
            // Perform top-up transaction
            transaction.TransactionDate = DateTime.UtcNow;
            _repository.AddTransaction(transaction);
            _logger.LogInformation("Debit the transaction amount including the 1 AED charge for transaction");
            _balanceService.Debit(transaction.Amount + 1, currentUserId);           

            return Ok("Top-up successful.");
        }


    }

}
