using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using topUpApi.Entities;
using topUpApi.Services;

namespace topUpApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiariesController : ControllerBase
    {
        private readonly ITopUpRepository _repository;

        public BeneficiariesController(ITopUpRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BeneficiaryModel>> GetBeneficiaries(int UserId)
        {
            var beneficiaries = _repository.GetBeneficiaries(UserId);
            return Ok(beneficiaries);
        }

        [HttpPost]
        public ActionResult AddBeneficiary(BeneficiaryModel beneficiary)
        {
            // Validate nickname length
            if (beneficiary.Nickname.Length > 20)
            {
                return BadRequest("Nickname must not exceed 20 characters.");
            }

            // Validate maximum number of active beneficiaries
            int currentUserId = beneficiary.UserId; // You need to implement this method

            // Check if the user already has 5 beneficiaries
            var userBeneficiaries = _repository.GetBeneficiariesByUserId(currentUserId).ToList();

            if (userBeneficiaries.Count >= 5)
            {
                return BadRequest("You cannot add more than 5 beneficiaries.");
            }
            beneficiary.UserId = currentUserId;
            _repository.AddBeneficiary(beneficiary);
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult UpdateBeneficiary(int id,int UserId, BeneficiaryModel updatedBeneficiary)
        {
            var existingBeneficiary = _repository.GetBeneficiaryById(id, UserId);

            if (existingBeneficiary == null)
            {
                return NotFound();
            }

            // Validate and update logic (adjust as needed)
            // ...

            existingBeneficiary.PhoneNumber = updatedBeneficiary.PhoneNumber;
            existingBeneficiary.Nickname = updatedBeneficiary.Nickname;
            existingBeneficiary.UserId = updatedBeneficiary.UserId;
            // Update other properties as needed

            _repository.UpdateBeneficiary(existingBeneficiary);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteBeneficiary(int id, int UserId)
        {
            var existingBeneficiary = _repository.GetBeneficiaryById(id,UserId);

            if (existingBeneficiary == null)
            {
                return NotFound();
            }

            // Delete logic
            // ...

            _repository.DeleteBeneficiary(id, UserId);
            return Ok();
        }

    }
}
