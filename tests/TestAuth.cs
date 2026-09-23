// Tests/TestAuth.cs
using NUnit.Framework;

[TestFixture]
public class TestAuth
{
    private AuthService _auth = null!;

    [SetUp]
    public void Setup()
    {
        _auth = new AuthService();
        _auth.Register("admin1", "CorrectPassword123!", "admin");
        _auth.Register("user1", "AnotherPassword456!", "user");
    }

    [Test]
    public void TestLoginSuccess_WithCorrectPassword()
    {
        // 올바른 자격 증명으로는 인증에 성공해야 함
        bool result = _auth.Authenticate("admin1", "CorrectPassword123!");
        Assert.That(result, Is.True, "올바른 비밀번호로는 로그인에 성공해야 합니다.");
    }

    [Test]
    public void TestLoginFailure_WithWrongPassword()
    {
        // 잘못된 로그인 시도 시뮬레이션 — 실패해야 함
        bool result = _auth.Authenticate("admin1", "WrongPassword!");
        Assert.That(result, Is.False, "잘못된 비밀번호로는 로그인에 실패해야 합니다.");
    }

    [Test]
    public void TestAdminRole_CanAccessAdminDashboard()
    {
        // admin 역할을 가진 사용자는 관리자 대시보드 접근 가능
        bool canAccess = _auth.CanAccessAdminDashboard("admin1");
        Assert.That(canAccess, Is.True, "admin 역할은 관리자 대시보드에 접근할 수 있어야 합니다.");
    }

    [Test]
    public void TestUserRole_CannotAccessAdminDashboard()
    {
        // 무단 접근 시뮬레이션 — user 역할은 관리자 대시보드 접근 불가해야 함
        bool canAccess = _auth.CanAccessAdminDashboard("user1");
        Assert.That(canAccess, Is.False, "user 역할은 관리자 대시보드에 접근할 수 없어야 합니다.");
    }
}