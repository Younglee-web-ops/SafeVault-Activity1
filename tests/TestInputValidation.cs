// Tests/TestInputValidation.cs
using NUnit.Framework;

[TestFixture]
public class TestInputValidation
{
    [Test]
    public void TestForSQLInjection()
    {
        // SQL 인젝션 공격 시도 문자열
        string malicious = "admin'; DROP TABLE Users; --";

        // 입력 검증 단계에서 이미 차단되어야 함
        bool isValid = InputValidator.TryValidateUsername(malicious, out _);
        Assert.That(isValid, Is.False, "SQL 인젝션 패턴이 검증을 통과하면 안 됩니다.");
    }

    [Test]
    public void TestForXSS()
    {
        // XSS 공격 시도 문자열
        string malicious = "<script>alert('xss')</script>";

        bool isValid = InputValidator.TryValidateUsername(malicious, out _);
        Assert.That(isValid, Is.False, "스크립트 태그가 포함된 입력은 차단되어야 합니다.");
    }
}