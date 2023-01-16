﻿using CleanArchMvc.Domain.Account;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CleanArchMvc.Infra.Data.Identity
{
    public class AuthenticateService : IAuthenticate
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AuthenticateService(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<bool> Authenticate(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email,
                password, false, lockoutOnFailure: false);

            return result.Succeeded;

        }


        public async Task<bool> RegisterUser(string email, string password)
        {
            var aplicationUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
            };

            var result = await _userManager.CreateAsync(aplicationUser, password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(aplicationUser, isPersistent: false);
            }

            return result.Succeeded;
        }
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
