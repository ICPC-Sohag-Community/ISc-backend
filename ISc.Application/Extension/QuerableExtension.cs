using ISc.Shared;
using Microsoft.EntityFrameworkCore;

namespace ISc.Application.Extension
{
    public static class QuerableExtension
    {
        public static async Task<PaginatedRespnose<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize, CancellationToken cancellationToken) where T : class
        {
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            pageSize = pageSize == 0 ? 10 : pageSize;
            int count = await source.CountAsync();
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            List<T> items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return PaginatedRespnose<T>.Create(items, count, pageNumber, pageSize);
        }
    }
}
