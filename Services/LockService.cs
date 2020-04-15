using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FindbookApi.Models;
using FindbookApi.AppExceptions;

namespace FindbookApi.Services
{
    public class LockService : ILockService
    {
        private readonly Context db;

        public LockService(Context context)
        {
            db = context;
        }

        /// <summary>
        /// Method requires user with lockrecord
        /// </summary>
        public async Task<DateTime> LockOut(User user, string reason, int minutes, int adminId)
        {
            if (user == null)
                throw new RequestArgumentException("User not found", 404);
            if (user.LockoutEnd > DateTime.UtcNow)
                throw new RequestArgumentException("The user already locked out", 400);
            DateTime end = DateTime.UtcNow.Add(TimeSpan.FromMinutes(minutes));
            user.LockoutEnd = end;
            user.LockRecord = UpdateLockRecord(user.LockRecord, new LockRecord(reason, adminId));
            await db.SaveChangesAsync();
            return end;
        }

        private LockRecord UpdateLockRecord(LockRecord origRecord, LockRecord newRecord)
        {
            if (origRecord == null)
                return newRecord;
            origRecord.Reason = newRecord.Reason;
            origRecord.AdminId = newRecord.AdminId;
            return origRecord;
        }
    }
}
