using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Customers;
using Nop.Data;
using Nop.Services.Messages;

namespace Nop.Services;
// Services/EmailVerificationCodeService.cs
public class EmailVerificationCodeService : IEmailVerificationCodeService
{
    private readonly IRepository<EmailVerificationCode> _repo;

    public EmailVerificationCodeService(IRepository<EmailVerificationCode> repo)
    {
        _repo = repo;
    }

    public async Task InsertAsync(EmailVerificationCode entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        await _repo.InsertAsync(entity);
    }

    public async Task<EmailVerificationCode> GetLatestCodeAsync(int customerId)
    {
        return await _repo.Table
            .Where(x => x.CustomerId == customerId && !x.IsUsed)
            .OrderByDescending(x => x.CreatedOnUtc)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> ValidateCodeAsync(int customerId, string code)
    {
        var record = await _repo.Table
            .Where(x => x.CustomerId == customerId && x.Code == code && !x.IsUsed)
            .OrderByDescending(x => x.CreatedOnUtc)
            .FirstOrDefaultAsync();

        if (record == null)
            return false;

        // Optional: add expiration logic
        if ((DateTime.UtcNow - record.CreatedOnUtc).TotalMinutes > 10)
            return false;

        return true;
    }

    public async Task MarkCodeAsUsedAsync(int id)
    {
        var record = await _repo.GetByIdAsync(id);
        if (record != null)
        {
            record.IsUsed = true;
            await _repo.UpdateAsync(record);
        }
    }

    public async Task DeleteAsync(EmailVerificationCode entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));
        await _repo.DeleteAsync(entity);
    }
}

