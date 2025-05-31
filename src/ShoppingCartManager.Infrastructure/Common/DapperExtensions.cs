namespace ShoppingCartManager.Infrastructure.Common;

public static class DapperExtensions
{
    public static async Task<Option<T>> GetSingleBy<T>(
        this IDbConnection connection,
        string tableName,
        string columnName,
        object value
    )
    {
        var sql = $"SELECT * FROM [{tableName}] WHERE [{columnName}] = @Value";
        var result = await connection.QuerySingleOrDefaultAsync<T>(
            sql,
            param: new { Value = value }
        );

        return Optional(result);
    }

    public static async Task<Option<T>> GetSingleWhere<T>(
        this IDbConnection connection,
        string tableName,
        Dictionary<string, object> where
    )
    {
        var conditions = string.Join(" AND ", where.Keys.Select(k => $"[{k}] = @{k}"));
        var sql = $"SELECT * FROM [{tableName}] WHERE {conditions}";

        var result = await connection.QuerySingleOrDefaultAsync<T>(
            sql,
            where.ToDynamicParameters()
        );

        return Optional(result);
    }

    public static async Task<IEnumerable<T>> GetAllWhere<T>(
        this IDbConnection connection,
        string tableName,
        Dictionary<string, object> where
    )
    {
        var conditions = string.Join(" AND ", where.Keys.Select(k => $"[{k}] = @{k}"));
        var sql = $"SELECT * FROM [{tableName}] WHERE {conditions}";

        var result = await connection.QueryAsync<T>(
            sql,
            where.ToDynamicParameters()
        );

        return result;
    }

    public static async Task<bool> Exists(
        this IDbConnection connection,
        string tableName,
        string columnName,
        object value
    )
    {
        var sql = $"SELECT 1 FROM [{tableName}] WHERE [{columnName}] = @Value";
        var result = await connection.ExecuteScalarAsync<bool>(sql, new { Value = value });

        return result;
    }

    public static async Task<bool> Add<T>(
        this IDbConnection connection,
        string tableName,
        T entity
    )
    {
        var properties = typeof(T).GetProperties().ToList();

        var columnNames = string.Join(", ", properties.Select(p => $"[{p.Name}]"));
        var paramNames = string.Join(", ", properties.Select(p => $"@{p.Name}"));

        var sql = $"INSERT INTO [{tableName}] ({columnNames}) VALUES ({paramNames})";

        var result = await connection.ExecuteAsync(sql, entity);
        return result > 0;
    }

    public static async Task<bool> Update<T>(
        this IDbConnection connection,
        string tableName,
        T entity
    )
    {
        var properties = typeof(T)
            .GetProperties()
            .Where(p => p.Name != nameof(EntityBase.Id))
            .ToList();

        var setClause = string.Join(", ", properties.Select(p => $"[{p.Name}] = @{p.Name}"));
        var sql = $"UPDATE [{tableName}] SET {setClause} WHERE [Id] = @Id";

        var result = await connection.ExecuteAsync(sql, entity);
        return result > 0;
    }

    public static async Task<bool> DeleteById(
        this IDbConnection connection,
        string tableName,
        Guid id
    )
    {
        var sql = $"DELETE FROM [{tableName}] WHERE [Id] = @Id";
        var result = await connection.ExecuteAsync(sql, new { Id = id });
        return result > 0;
    }

    public static async Task<(IEnumerable<T> Results, int TotalCount)> QueryPaginated<T>(
        this IDbConnection connection,
        string tableName,
        Dictionary<string, object> where,
        string orderBy,
        int skip,
        int take)
    {
        var conditions = string.Join(" AND ", where.Keys.Select(k => $"[{k}] = @{k}"));
        var sql = $"""
                   
                           SELECT * FROM [{tableName}]
                           WHERE {conditions}
                           ORDER BY {orderBy}
                           OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;
                   
                           SELECT COUNT(*) FROM [{tableName}]
                           WHERE {conditions};
                       
                   """;

        var parameters = where.ToDynamicParameters();
        parameters.Add("Skip", skip);
        parameters.Add("Take", take);

        using var multi = await connection.QueryMultipleAsync(sql, parameters);

        var results = await multi.ReadAsync<T>();
        var total = await multi.ReadSingleAsync<int>();

        return (results, total);
    }


    private static DynamicParameters ToDynamicParameters(this Dictionary<string, object> dict)
    {
        var parameters = new DynamicParameters();
        foreach (var keyValuePair in dict)
        {
            parameters.Add(keyValuePair.Key, keyValuePair.Value);
        }

        return parameters;
    }
}
