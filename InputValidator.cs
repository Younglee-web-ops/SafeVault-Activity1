using System.Text.RegularExpressions;

public static class InputValidator
{
    // 사용자명: 영문/숫자/공백/밑줄만 허용 (SQL이나 스크립트에 쓰이는 ' " ; -- < > 등은 전부 차단)
    public static bool TryValidateUsername(string username, out string error)
    {
        error = "";
        if (string.IsNullOrWhiteSpace(username) || username.Length > 50)
        {
            error = "사용자명은 1~50자여야 합니다.";
            return false;
        }
        if (!Regex.IsMatch(username, @"^[a-zA-Z0-9_ ]+$"))
        {
            error = "사용자명에 허용되지 않는 문자가 포함되어 있습니다.";
            return false;
        }
        return true;
    }

    // 이메일: 기본적인 형식(예: abc@domain.com)만 통과
    public static bool TryValidateEmail(string email, out string error)
    {
        error = "";
        if (string.IsNullOrWhiteSpace(email) ||
            !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            error = "이메일 형식이 올바르지 않습니다.";
            return false;
        }
        return true;
    }
}