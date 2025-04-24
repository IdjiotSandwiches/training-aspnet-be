using Microsoft.EntityFrameworkCore.Storage;
using StnkApi.Common;

namespace StnkApi.Helpers
{
    public static class StnkHelper
    {
        public static decimal CalculateTax(decimal carPrice, int carTypeTax, int engineSizeTax, int progresiveTax)
        {
            return carPrice * carTypeTax / 100 + carPrice * engineSizeTax / 100 + carPrice * progresiveTax * 0.05m;
        }

        public static async Task<Result<T>> RollbackWithError<T>(IDbContextTransaction transaction, Result<T> sequenceResult)
        {
            await transaction.RollbackAsync();
            return Result<T>.Error(sequenceResult.Status, sequenceResult.Message);
        }
    }
}
