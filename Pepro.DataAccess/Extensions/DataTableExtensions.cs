using System.Data;

namespace Pepro.DataAccess.Extensions;

static class DataTableExtensions
{
    /// <summary>
    /// Maps the first row of the <see cref="DataTable"/> to a single object using the specified mapping function.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of the mapped object.
    /// </typeparam>
    /// <param name="dataTable">
    /// The source data table.
    /// </param>
    /// <param name="map">
    /// A function that maps a <see cref="DataRow"/> to an object of type <typeparamref name="TSource"/>.
    /// </param>
    /// <returns>
    /// A single mapped object if one row exists; <c>null</c> if no rows exist.
    /// Throws an <see cref="InvalidOperationException"/> if more than one row exists.
    /// </returns>
    public static TSource? MapToSingleOrDefault<TSource>(
        this DataTable dataTable,
        Func<DataRow, TSource> map
    )
        where TSource : class
    {
        if (dataTable.Rows.Count == 0)
        {
            return default;
        }

        if (dataTable.Rows.Count == 1)
        {
            return map(dataTable.Rows[0]);
        }

        throw new InvalidOperationException(
            "Sequence contains more than one element"
        );
    }

    /// <summary>
    /// Maps all rows of the <see cref="DataTable"/> to a sequence of objects using the specified mapping function.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of the mapped objects.
    /// </typeparam>
    /// <param name="dataTable">
    /// The source data table.
    /// </param>
    /// <param name="map">
    /// A function that maps a <see cref="DataRow"/> to an object of type <typeparamref name="TSource"/>.
    /// </param>
    /// <returns>
    /// An enumerable collection of mapped objects.
    /// </returns>
    public static IEnumerable<TSource> MapMany<TSource>(
        this DataTable dataTable,
        Func<DataRow, TSource> map
    )
    {
        return dataTable.Rows.Cast<DataRow>().Select(map);
    }
}
