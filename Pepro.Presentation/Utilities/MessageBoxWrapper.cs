namespace Pepro.Presentation.Utilities;

public static class MessageBoxWrapper
{
    // TODO: Use `*.en.resx` instead
    private static string GetMessage(string key)
    {
        return key switch
        {
            "SelectData" => "Vui lòng chọn trường dữ liệu",
            "DataReadError" => "Không thể đọc dữ liệu",
            "SaveSuccess" => "Lưu dữ liệu thành công",
            "SaveFailed" => "Lưu dữ liệu thất bại",
            "DeleteSuccess" => "Đã xóa {0} dữ liệu thành công",
            "ResetPasswordSuccess" =>
                "Đã đặt mật khẩu gốc cho {0} tài khoản thành công",
            "LockSuccess" => "Đã khóa thành công {0} trường dữ liệu",
            "UnlockSuccess" => "Đã mở khóa thành công {0} trường dữ liệu",
            "FillInformation" => "Mời điền đầy đủ thông tin",
            "ConfirmDelete" => "Bạn có chắc muốn xóa không?",
            "ConfirmExit" => "Bạn có chắc muốn thoát không?",
            "Confirm" => "Bạn có chắc thực hiện thao tác không?",
            "TreasureFoundPremiumUnlock" =>
                "Ôi trời, bạn vừa mới tìm thấy kho báu đấy, mua premium đi, rồi chúng tôi mở khóa tính năng này",
            "TreasureFoundPremiumUnlock1" =>
                "Hay quá ta, tính năng này chưa được phát triển, cái nút chỉ để cho vui thôi :))) mua premium đi",
            "TreasureFoundPremiumUnlock2" =>
                "Bruh, xin lỗi chứ tính năng này đã sai nghiệp vụ hoàn toàn rồi, mua premium đi rồi tôi sửa sau",
            "TreasureFoundPremiumUnlock3" =>
                "Thật lòng mà nói thì tôi không biết phát triển tính này như thế nào cho đúng với thật tế nhất",
            _ => "Không có thông điệp nào phụ hợp với 'key'",
        };
    }

    public static DialogResult ShowInformation(string key, params object[] args)
    {
        string raw = GetMessage(key);

        return MessageBox.Show(
            string.Format(raw, args),
            "Thông báo",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        );
    }

    public static DialogResult ShowError(string key, params object[] args)
    {
        string raw = GetMessage(key);

        return MessageBox.Show(
            string.Format(raw, args),
            "Lỗi",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error
        );
    }

    public static DialogResult Confirm(string? key = null)
    {
        return MessageBox.Show(
            GetMessage(key ?? "Confirm"),
            "Xác nhận",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        );
    }
}
