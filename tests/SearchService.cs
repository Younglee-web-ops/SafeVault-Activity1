using Microsoft.Data.SqlClient;
using System.Net;

public class SearchService
{
    private readonly string _connectionString;
    public SearchService(string connectionString) => _connectionString = connectionString;

    // === 디버깅 전 (취약한 코드) ===
    // public List<string> SearchUsers_VULNERABLE(string keyword)
    // {
    //     using var conn = new SqlConnection(_connectionString);
    //     var cmd = new SqlCommand("SELECT Username FROM Users WHERE Username = '" + keyword + "'", conn);
    //     // 문제: keyword를 SQL 문자열에 그대로 이어붙임 -> SQL 인젝션에 그대로 노출됨
    //     ...
    // }

    // === 디버깅 후 (수정된 코드) ===
    // 매개변수화된 쿼리로 교체 -> keyword가 SQL 구문으로 해석되지 않고 값으로만 처리됨
    public List<string> SearchUsers(string keyword)
    {
        var results = new List<string>();
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("SELECT Username FROM Users WHERE Username = @keyword", conn);
        cmd.Parameters.AddWithValue("@keyword", keyword);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            results.Add(reader.GetString(0));
        return results;
    }

    // === 디버깅 전 (취약한 코드) ===
    // public string RenderProfile_VULNERABLE(string bio) => $"<div>{bio}</div>";
    // 문제: bio를 그대로 HTML에 삽입 -> <script> 태그가 그대로 실행됨 (XSS)

    // === 디버깅 후 (수정된 코드) ===
    // HTML 인코딩을 거쳐서 <, >, " 같은 문자가 태그로 해석되지 않도록 이스케이프 처리
    public string RenderProfile(string bio)
    {
        string safeBio = WebUtility.HtmlEncode(bio);
        return $"<div>{safeBio}</div>";
    }
}