using System.Collections.Generic;
using BCrypt.Net;

public class UserAccount
{
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string Role { get; set; } = "user"; // "admin" 또는 "user"
}

public class AuthService
{
    private readonly Dictionary<string, UserAccount> _users = new();

    // 비밀번호를 그대로 저장하지 않고 bcrypt로 해시해서 저장합니다.
    // 해시는 단방향이라 저장된 값만 봐서는 원래 비밀번호를 알 수 없습니다.
    public void Register(string username, string password, string role = "user")
    {
        string hash = BCrypt.Net.BCrypt.HashPassword(password);
        _users[username] = new UserAccount { Username = username, PasswordHash = hash, Role = role };
    }

    // 로그인 시 입력한 비밀번호를 저장된 해시와 비교합니다 (인증).
    public bool Authenticate(string username, string password)
    {
        if (!_users.TryGetValue(username, out var user)) return false;
        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }

    public string? GetRole(string username)
    {
        return _users.TryGetValue(username, out var user) ? user.Role : null;
    }

    // 역할 기반 권한 부여: "admin" 역할을 가진 사용자만 관리자 대시보드 접근 가능 (권한 부여).
    public bool CanAccessAdminDashboard(string username)
    {
        return GetRole(username) == "admin";
    }
}