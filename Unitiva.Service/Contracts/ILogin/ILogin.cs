using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unitiva.Domain.User;

namespace Unitiva.Service.Contracts.ILogin
{
    public interface ILogin
    {
        public Task<string> login(string username, string password);
        public Task<bool> register(RegisterUserDto request);
    }
}
