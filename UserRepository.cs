using Microsoft.Data.SqlClient;

public class UserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // 문자열을 '+'로 이어붙이지 않고 @username, @email 같은 매개변수 자리표시자를 쓰기 때문에,
    // 악의적인 SQL 구문이 들어와도 실행되지 않고 그냥 "문자열 값"으로만 처리됩니다.
    public void InsertUser(string username, string email)
    {
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(
            "INSERT INTO Users (Username, Email) VALUES (@username, @email)", conn);

        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@email", email);

        conn.Open();
        cmd.ExecuteNonQuery();
    }
}