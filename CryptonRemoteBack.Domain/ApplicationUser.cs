using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace CryptonRemoteBack.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public List<Wallet> Wallets { get; set; } = new();
        public List<Farm> Farms { get; set; } = new();
        public List<FlightSheet> FlightSheets { get; set; } = new();
    }

    public struct UserRoles
    {
        //public const string Admin = "Admin";

        public const string User = "User";
    }
}
