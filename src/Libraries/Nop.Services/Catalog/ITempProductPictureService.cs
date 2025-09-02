using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;

namespace Nop.Services.Catalog;
public interface ITempProductPictureService
{
    Task InsertAsync(TempProductPicture picture);
    Task DeleteAsync(TempProductPicture picture);
    Task<IList<TempProductPicture>> GetBySessionKeyAsync(string sessionKey);
    Task ClearBySessionKeyAsync(string sessionKey);
}

