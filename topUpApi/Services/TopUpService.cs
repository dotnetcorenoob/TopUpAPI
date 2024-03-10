namespace topUpApi.Services
{

    public interface ITopUpService
    {
        public bool CanTopUp(int userId, decimal amount);
    }
    public class TopUpService: ITopUpService
    {
        private readonly ITopUpRepository _repository;


        public TopUpService(ITopUpRepository repository)
        {
            _repository = repository;
        }

    
            public bool CanTopUp(int userId, decimal amount)
        {
            // Get the top-ups for the current month
            var topUpsThisMonth = _repository.GetTopUpsByUserIdAndMonth(userId, DateTime.Now.Month, DateTime.Now.Year);

            // Calculate the total top-up amount for the current month
            decimal totalTopUpThisMonth = topUpsThisMonth.Sum(t => t.Amount);

            // Check if the new top-up will exceed the monthly limit
            if (totalTopUpThisMonth + amount > 3000)
            {
                return false;
            }

            return true;
        }
    }
}
