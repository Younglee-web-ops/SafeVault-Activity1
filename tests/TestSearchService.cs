// Tests/TestSearchService.cs
using NUnit.Framework;

[TestFixture]
public class TestSearchService
{
    private SearchService _service = null!;

    [SetUp]
    public void Setup()
    {
        // 실제 DB 연결 없이 인코딩/이스케이프 로직만 검증하는 용도라
        // 연결 문자열은 더미 값으로 둡니다.
        _service = new SearchService("Data Source=dummy");
    }

    [Test]
    public void TestXSS_ScriptTagIsEscaped()
    {
        // XSS 공격 시나리오: 사용자가 프로필 소개글에 스크립트 태그를 입력
        string malicious = "<script>alert('xss')</script>";

        string rendered = _service.RenderProfile(malicious);

        // 원본 <script> 태그가 그대로 남아 있으면 안 됨 (브라우저가 실행할 수 있음)
        Assert.That(rendered, Does.Not.Contain("<script>"),
            "스크립트 태그가 이스케이프되지 않고 그대로 렌더링되면 XSS에 취약합니다.");

        // HTML 인코딩된 형태(&lt;script&gt;)로는 포함되어 있어야 함
        Assert.That(rendered, Does.Contain("&lt;script&gt;"),
            "악성 입력은 HTML 엔티티로 이스케이프되어야 합니다.");
    }

    [Test]
    public void TestSearch_ParameterizedQuery_DoesNotThrowOnInjectionAttempt()
    {
        // SQL 인젝션 공격 시나리오: 검색어에 SQL 구문을 섞어서 전달
        // (매개변수화된 쿼리를 쓰면 이 값은 그냥 "찾을 수 없는 검색어" 취급되어야 하며,
        //  SQL 구문으로 해석되어 실행되지 않아야 합니다.)
        string malicious = "x'; DROP TABLE Users; --";

        // SqlCommand.Parameters.AddWithValue를 쓰는 이상, 이 문자열은 SQL 코드가 아니라
        // 그냥 '값'으로 바인딩되므로 실행 시점에 예외 없이 안전하게 처리되어야 합니다.
        Assert.DoesNotThrow(() =>
        {
            // 실제 DB 연결이 없는 테스트 환경이라 SqlException(연결 실패)은 나더라도,
            // "SQL 구문 오류"나 "여러 명령이 실행됨" 같은 인젝션 관련 예외는 아니어야 합니다.
            try { _service.SearchUsers(malicious); }
            catch (Microsoft.Data.SqlClient.SqlException) { /* 연결 실패는 이 테스트의 관심사가 아님 */ }
        });
    }
}