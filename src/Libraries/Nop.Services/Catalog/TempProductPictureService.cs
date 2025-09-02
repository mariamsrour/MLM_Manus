using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Data;

namespace Nop.Services.Catalog;
public class TempProductPictureService : ITempProductPictureService
{
    private readonly IRepository<TempProductPicture> _tempPictureRepository;

    public TempProductPictureService(IRepository<TempProductPicture> tempPictureRepository)
    {
        _tempPictureRepository = tempPictureRepository;
    }

    public async Task InsertAsync(TempProductPicture picture)
    {
        await _tempPictureRepository.InsertAsync(picture);
    }

    public async Task DeleteAsync(TempProductPicture picture)
    {
        await _tempPictureRepository.DeleteAsync(picture);
    }

    public async Task<IList<TempProductPicture>> GetBySessionKeyAsync(string sessionKey)
    {
        return await _tempPictureRepository.Table
            .Where(p => p.SessionId == sessionKey)
            .OrderBy(p => p.DisplayOrder)
            .ToListAsync();
    }

    public async Task ClearBySessionKeyAsync(string sessionKey)
    {
        var records = await GetBySessionKeyAsync(sessionKey);
        foreach (var record in records)
            await _tempPictureRepository.DeleteAsync(record);
    }
}

