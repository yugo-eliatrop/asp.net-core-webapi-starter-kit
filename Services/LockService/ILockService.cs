using System;
using System.Threading.Tasks;
using FindbookApi.Models;

namespace FindbookApi.Services
{
    public interface ILockService
    {
        /// <summary>
        /// Method requires user with lockrecord
        /// </summary>
        Task<DateTime> LockOut(User user, string reason, int minutes, int adminId);
    }
}
