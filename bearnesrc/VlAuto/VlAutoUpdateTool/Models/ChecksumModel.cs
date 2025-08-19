/* by Tuyết Nhi */
/* FIX warning CS8618: Non-nullable property 'Path' must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring the property as nullable. */
namespace VlAutoUpdateTool.Models
{
    public class ChecksumModel
    {
        public string Path { get; set; } = string.Empty;
        public string Hash { get; set; } = string.Empty;
        public string UrlPath { get; set; } = string.Empty;
        public string AbsolutePath { get; set; } = string.Empty;

        /* by Tuyết Nhi */
        // Thêm thuộc tính lưu dung lượng file (bytes)
        public long Size { get; set; } = 0;
    }
}
