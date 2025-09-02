using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Customers;

namespace Nop.Services.Messages;
public interface IEmailVerificationCodeService
{
    Task InsertAsync(EmailVerificationCode entity);
    Task<EmailVerificationCode> GetLatestCodeAsync(int customerId);
    Task<bool> ValidateCodeAsync(int customerId, string code);
    Task MarkCodeAsUsedAsync(int id);
    Task DeleteAsync(EmailVerificationCode entity);

}