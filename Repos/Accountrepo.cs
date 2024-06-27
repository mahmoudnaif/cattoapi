using AutoMapper;
using cattoapi.ClientModles;
using cattoapi.customResponse;
using cattoapi.DTOS;
using cattoapi.Interfaces;
using cattoapi.Models;
using cattoapi.utlities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace cattoapi.Repos
{
    public class Accountrepo : IAccountRepo
    {
        private readonly CattoDbContext _context;
        private readonly IMapper _mapper;

        public Accountrepo(CattoDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public CustomResponse<IEnumerable<AccountDTO>> GetAccounts()
        {
            ICollection<Account> accounts = _context.Accounts.ToList();

            if(accounts == null)
            return new CustomResponse<IEnumerable<AccountDTO>>(404,"No acccounts were found");

            IEnumerable<AccountDTO> accountDTO = _mapper.Map<IEnumerable<AccountDTO>>(accounts);

            return new CustomResponse<IEnumerable<AccountDTO>>(200, "Accounts retrieved successfully", accountDTO);
        }

        public CustomResponse<AccountDTO> GetAccountByEmail(string email)
        {
            if (!utlities.Utlities.IsValidEmail(email))
                return new CustomResponse<AccountDTO>(400, "Invalid email format. Please enter a valid email.");
            
            

            Account account =  _context.Accounts.SingleOrDefault(acc => acc.Email == email);

            if (account == null)
                return new CustomResponse<AccountDTO>(404, "NOT FOUND");


            AccountDTO accountDTO = _mapper.Map<AccountDTO>(account);

            return new CustomResponse<AccountDTO>(200, "Account found", accountDTO);



        }

        public CustomResponse<AccountDTO> GetAccountById(string strId)
        {

            if (!int.TryParse(strId, out int id))
                return new CustomResponse<AccountDTO>(400, "Invalid id format. Please enter a valid integer id." );


            Account account = _context.Accounts.SingleOrDefault(acc => acc.AccountId == id);

            if (account == null)
                return new CustomResponse<AccountDTO>(404, "NOT FOUND");


            AccountDTO accountDTO = _mapper.Map<AccountDTO>(account);

            return new CustomResponse<AccountDTO>(200, "Account found", accountDTO);


        }


        public CustomResponse<IEnumerable<AccountDTO>> SearchAccounts(string searchQuery,int take, int skip)
        {
            if (take == 0)
                return  new CustomResponse<IEnumerable<AccountDTO>>(404, "Not found");


            if (take < 0 || skip < 0)
                return new CustomResponse<IEnumerable<AccountDTO>>(400, "Invalid numbers take and skip must be 0 or more" );


            var searchResult = _context.Accounts.Where(acc => acc.UserName.ToLower().StartsWith(searchQuery.ToLower())).Skip(skip).Take(take).ToList();

            if(searchResult.Count == 0)
                return new CustomResponse<IEnumerable<AccountDTO>>(404, "NOT FOUND");


            IEnumerable<AccountDTO> accountDTO = _mapper.Map<IEnumerable<AccountDTO>>(searchResult);

            return new CustomResponse<IEnumerable<AccountDTO>>(200, "Accounts retrieved successfully", accountDTO);

        }






     }
}
