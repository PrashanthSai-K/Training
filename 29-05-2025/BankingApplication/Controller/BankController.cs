using BankingApplication.Interfaces;
using BankingApplication.Models;
using BankingApplication.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingApplication.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class Bank : ControllerBase
    {
        private readonly IBankService _bankService;

        public Bank(IBankService bankService)
        {
            _bankService = bankService;
        }

        [HttpGet]
        [Route("Balance")]
        public async Task<IActionResult> GetAccountBalance(int AccountId)
        {
            var Balance = await _bankService.GetAccountBalance(AccountId);
            return Ok($"Account Balance is Rs.{Balance}");
        }

        [HttpPost]
        [Route("CreateAccount")]
        public async Task<ActionResult<AccountCreateDto>> CreateAccount(AccountCreateDto account)
        {
            var newaccount = new Account()
            {
                Name = account.Name,
                Email = account.Email,
                Balance = account.Balance
            };
            newaccount = await _bankService.CreateAccount(newaccount);
            return Created("", account);
        }

        [HttpPut]
        [Route("Withdraw")]
        public async Task<ActionResult<Account>> WithdrawAmount(WithdrawDto withdrawDto)
        {
            var account = await _bankService.Withdraw(withdrawDto);
            return Ok(account);
        }

        [HttpPut]
        [Route("Deposit")]
        public async Task<ActionResult<Account>> DepositAmount(DepositDto depositDto)
        {
            var account = await _bankService.Deposit(depositDto);
            return Ok(account);
        }

        [HttpPost]
        [Route("Transfer")]
        public async Task<ActionResult<TransferDto>> TransferAmount(TransferDto transferDto)
        {
            var transferred = await _bankService.Transfer(transferDto);
            return Ok(transferred);
        }

    }
}
