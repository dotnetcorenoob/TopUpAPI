using topUpApi.Entities;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data.Common;

namespace topUpApi.Services    

// TopUpRepository.cs
{ 
    public interface ITopUpRepository
    {
        IEnumerable<BeneficiaryModel> GetBeneficiaries(int UserId);
        BeneficiaryModel GetBeneficiaryById(int id, int UserId);
        void AddBeneficiary(BeneficiaryModel beneficiary);
        void UpdateBeneficiary(BeneficiaryModel beneficiary);
        void DeleteBeneficiary(int id, int UserId);

        IEnumerable<TopUpTransactionModel> GetTransactions();
        IEnumerable<TopUpOptions> GetTopUpOptions();
        void AddTransaction(TopUpTransactionModel transaction);

        IEnumerable<BeneficiaryModel> GetBeneficiariesByUserId(int userId);

        IEnumerable<TopUpTransactionModel> GetTopUpsByUserIdAndMonth(int userId, int month, int year);
    }



    public class TopUpRepository : ITopUpRepository
    {
        private readonly string _connectionString;

        public TopUpRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<BeneficiaryModel> GetBeneficiaries(int UserId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<BeneficiaryModel>("SELECT * FROM Beneficiaries  WHERE UserId = @Id", new { Id = UserId });
            }
        }

        public BeneficiaryModel GetBeneficiaryById(int id,int UserId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<BeneficiaryModel>("SELECT * FROM Beneficiaries WHERE Id = @Id and UserId = @User", new { Id = id ,User=UserId });
            }
        }

        public IEnumerable<BeneficiaryModel>  GetBeneficiariesByUserId(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<BeneficiaryModel>("SELECT * FROM Beneficiaries WHERE UserId = @Id", new { Id = userId });
            }
        }
        public IEnumerable<TopUpTransactionModel> GetTransactions()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<TopUpTransactionModel>("SELECT * FROM TopUpTransactions");
            }
        }

        public IEnumerable<TopUpOptions> GetTopUpOptions()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<TopUpOptions>("SELECT * FROM TopUpOptions where status='Y'");
            }
        }
        public void AddBeneficiary(BeneficiaryModel beneficiary)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute("INSERT INTO Beneficiaries (PhoneNumber, Nickname,UserId) VALUES (@PhoneNumber, @Nickname,@UserId)", beneficiary);
            }
        }

        public void UpdateBeneficiary(BeneficiaryModel beneficiary)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute("UPDATE Beneficiaries SET PhoneNumber = @PhoneNumber, Nickname = @Nickname, UserId = @UserId WHERE Id = @Id and UserId= @UserId", beneficiary);
            }
        }

        public void DeleteBeneficiary(int id, int UserId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute("DELETE FROM Beneficiaries WHERE Id = @Id and UserId=@_UserId", new { Id = id ,_UserId=UserId });
            }
        }

        public IEnumerable<TopUpTransactionModel> GetTopUpsByUserIdAndMonth(int userId, int month, int year)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<TopUpTransactionModel>("SELECT * FROM TopUpTransactions WHERE UserId = @UserId AND MONTH(TransactionDate) = @Month AND YEAR(TransactionDate) = @Year", new { UserId = userId, Month = month, Year = year });
            }
          }

        public void AddTransaction(TopUpTransactionModel transaction)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute("INSERT INTO TopUpTransactions (BeneficiaryId, Amount, TransactionDate,UserId) VALUES (@BeneficiaryId, @Amount, @TransactionDate,@UserId)", transaction);
            }
        }
    }

}
