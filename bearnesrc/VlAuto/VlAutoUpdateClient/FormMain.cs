/*
########################
@Phát triển thêm từ source của VlAutoUpdateClient.
@Developer: Tuyết Nhi + yulum.
@Released date: 20250819.

Tối ưu hiển thị và loại bỏ thư mục Game.
Thay đổi logic đường dẫn.
Bổ sung thêm nhiều logic.
########################
*/

using System.Diagnostics;
using System.Security.Cryptography;
using VlAutoUpdateClient.Models;

namespace VlAutoUpdateClient;

public partial class FormMain : Form
{
    private bool IsRunning = false;
    private readonly IHttpClient _httpClient;
    private readonly IHttpClient _httpClientDownload;
    public FormMain(HttpClientResolver clientResolver)
    {
        InitializeComponent();
        _httpClient = clientResolver(HttpClientNameEnum.Default.ToString());
        _httpClientDownload = clientResolver(HttpClientNameEnum.Download.ToString());
    }

    private void FormMain_Load(object sender, EventArgs e)
    {
        Task.Factory.StartNew(() =>
        {
            var task = CheckSum();
        });
    }

    private async Task CheckSum()
    {
        try
        {
            if (IsRunning)
                return;
            IsRunning = true;

            buttonRetry.Invoke(() => buttonRetry.Enabled = false);

            progressBarAll.Invoke(() =>
            {
                progressBarAll.Maximum = 1;
                progressBarAll.Value = 0;
            });
            progressBarCurent.Invoke(() =>
            {
                progressBarCurent.Maximum = 1;
                progressBarCurent.Value = 0;
            });

            SetStatusText("Đang kết nối máy chủ...");

            //by Tuyết Nhi.
            string urlChecksum = Common.GetUrl("/updatefiles/checksum.txt");    //chỉ định file checksum.txt
            string json = string.Empty;

            try
            {
                var response = await _httpClient.Get(urlChecksum);
                if (!response.IsSuccessStatusCode)
                {
                    SetStatusText("Không thể kết nối máy chủ (mã lỗi: " + (int)response.StatusCode + ")");
                    return;
                }
                json = await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                SetStatusText("Lỗi HttpRequestException: " + ex.Message);
                return;
            }
            catch (Exception ex)
            {
                SetStatusText("Không thể kết nối máy chủ: " + Environment.NewLine + ex.Message);
                return;
            }

            if (string.IsNullOrEmpty(json))
            {
                SetStatusText("Tập tin checksum bị trống.");
                return;
            }

            ChecksumModel[]? models = Common.JsonDeserializeObject<ChecksumModel[]>(json);
            if (models == null || models.Length == 0)
            {
                SetStatusText("Tập tin checksum không đúng định dạng.");
                return;
            }

            SetStatusText("Đang kiểm tra tập tin...");
            string folderPath = Environment.CurrentDirectory;

            HashSet<ChecksumModel> filesDownload = new HashSet<ChecksumModel>();
            long totalDownloadBytes = 0;

            using (var hashAlgorithm = MD5.Create())
            {
                foreach (var model in models)
                {
                    model.AbsolutePath = model.AbsolutePath.TrimStart('/');
                    model.UrlPath = model.UrlPath.TrimStart('/');

                    string filePath = Path.Combine(folderPath, model.AbsolutePath);
                    model.Path = filePath;

                    SetStatusText("Đang kiểm tra tập tin: " + model.AbsolutePath);

                    if (!File.Exists(filePath))
                    {
                        filesDownload.Add(model);
                        totalDownloadBytes += model.Size;
                    }
                    else
                    {
                        FileInfo fileInfo = new FileInfo(filePath);
                        string hash = ComputeHash(fileInfo, hashAlgorithm);
                        if (hash != model.Hash)
                        {
                            filesDownload.Add(model);
                            totalDownloadBytes += model.Size;
                        }
                    }
                }
            }

            if (filesDownload.Count == 0)
            {
                progressBarAll.Invoke(() =>
                {
                    progressBarAll.Maximum = 1;
                    progressBarAll.Value = 1;
                });
                progressBarCurent.Invoke(() =>
                {
                    progressBarCurent.Maximum = 1;
                    progressBarCurent.Value = 1;
                });

                SetStatusText("Không có file nào cần cập nhật.");
                return;
            }

            //by Tuyết Nhi.
            //test yêu cầu dung lượng
            //totalDownloadBytes = 1L * 1024 * 1024 * 1024 * 1024; // 1 Terabyte
 			//long oneGB = 1024L * 1024 * 1024;
			//totalDownloadBytes = 10 * oneGB;	//10gb
            if (!HasEnoughDiskSpace(totalDownloadBytes, folderPath))
            {
                string message = $"ổ đĩa không đủ dung lượng. Cần ít nhất {(totalDownloadBytes / 1024 / 1024):N2} MB trống. Vui lòng giải phóng dung lượng và thử lại.";
                MessageBox.Show(message, "Dung lượng ổ đĩa không đủ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                SetStatusText(message);
                return;
            }

            progressBarAll.Invoke(() => progressBarAll.Maximum = filesDownload.Count);

            int i = 0;
            const int MaxPathLength = 240; // Giới hạn đường dẫn quá dài sẽ bị 404.
            foreach (var model in filesDownload)
            {
                if (string.IsNullOrEmpty(model.Path))
                    continue;
                //by Tuyết Nhi.
                if (model.Path.Length >= MaxPathLength)
                {
                    SetStatusText($"Bỏ qua file quá dài: {model.AbsolutePath}");
                    MessageBox.Show(
                        $"Tên file hoặc đường dẫn quá dài (>{MaxPathLength} ký tự):\n\n{model.Path}",
                        "Lỗi đường dẫn",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    continue;
                }
                i++;
                int currentIndex = i;

                progressBarAll.Invoke(() => progressBarAll.Value = currentIndex);
                SetStatusText($"Đang tải ({currentIndex}/{filesDownload.Count}) {model.AbsolutePath}");

                await DownLoadFile(model);
            }

            SetStatusText("Cập nhật hoàn tất.");
        }
        catch (Exception ex)
        {
            SetStatusText("Có lỗi xảy ra: " + ex.Message);
        }
        finally
        {
            buttonRetry.Invoke(() => buttonRetry.Enabled = true);
            IsRunning = false;
        }
    }

    //by Tuyết Nhi.
    private bool HasEnoughDiskSpace(long requiredBytes, string? folderPath)
    {
        try
        {
            if (string.IsNullOrEmpty(folderPath))
                return false;

            string? root = Path.GetPathRoot(folderPath);
            if (string.IsNullOrEmpty(root))
                return false;

            DriveInfo drive = new DriveInfo(root);
            if (!drive.IsReady)
                return false;

            long availableFreeSpace = drive.AvailableFreeSpace;
            return availableFreeSpace >= requiredBytes;
        }
        catch
        {
            return false;
        }
    }

    private string ComputeHash(FileInfo fileInfo, HashAlgorithm hashAlgorithm)
    {
        using (var fs = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            var hash = hashAlgorithm.ComputeHash(fs);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
        }
    }

    private async Task DownLoadFile(ChecksumModel model)
    {
        try
        {
            string? folderPath = Path.GetDirectoryName(model.Path);
            if (string.IsNullOrEmpty(folderPath))
            {
                return;
            }
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            /* FIX lỗi file bị 18kb by Tuyết Nhi*/
            // URL tải file từ thư mục updatefiles trên server
            // /updatefiles/aaa%2fxxx.txt xóa mã hóa thư mục thành thư mục bình thường.
            string url = Common.GetUrl("/updatefiles/" + model.UrlPath);    ///updatefiles?f=	fix to /updatefiles/

            using (FileStream fs = new FileStream(model.Path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                await _httpClientDownload.DownloadDataAsync(url, fs, progressBarCurent);
            }
        }
        catch (Exception ex)
        {
            SetStatusText("Có lỗi xảy ra: " + ex.Message);
        }
    }

    private void SetStatusText(string text)
    {
        labelStatus.Invoke(delegate { labelStatus.Text = text; });
    }

    private void buttonRetry_Click(object sender, EventArgs e)
    {
        Task.Factory.StartNew(() =>
        {
            var task = CheckSum();
        });
    }

    /* Mở game */
    private void buttonOpenGame_Click(object sender, EventArgs e)
    {
        //by Tuyết Nhi.
        if (IsRunning)
        {
            MessageBox.Show("Đang cập nhật... Vui lòng chờ hoàn tất trước khi mở game.",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
            return;
        }
        string folderPath = Environment.CurrentDirectory;
        string gameExePath = Path.Combine(folderPath, "game.exe"); // Bỏ thư mục Game, file game.exe nằm trực tiếp folder chạy

        if (!File.Exists(gameExePath))
        {
            MessageBox.Show("Không tìm thấy file game.exe!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = gameExePath,
            Arguments = "",          // Nếu không có tham số thì để string rỗng
            WorkingDirectory = folderPath,
            CreateNoWindow = false,
            UseShellExecute = true,  // cần true nếu muốn Verb "runas" hoạt động
            /* Verb = "runas"           // chạy với quyền admin nếu cần */
        };

        try
        {
            using var process = new Process { StartInfo = startInfo };
            process.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Không thể mở game: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    //add config by Tuyết Nhi.
    private void buttonAutoGame_Click(object sender, EventArgs e)
    {
        if (IsRunning)
        {
            MessageBox.Show("Đang cập nhật... Vui lòng chờ hoàn tất trước khi mở config.",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
            return;
        }
        string folderPath = Environment.CurrentDirectory;
        string configExePath = Path.Combine(folderPath, "config.exe");

        if (!File.Exists(configExePath))
        {
            MessageBox.Show("Không tìm thấy file config.exe!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = configExePath,
            Arguments = "",          // Nếu không có tham số thì để string rỗng
            WorkingDirectory = folderPath,
            CreateNoWindow = false,
            UseShellExecute = true,  // cần true nếu muốn Verb "runas" hoạt động
            /* Verb = "runas"           // chạy với quyền admin nếu cần */
        };

        try
        {
            using var process = new Process { StartInfo = startInfo };
            process.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Không thể mở config: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void buttonExit_Click(object sender, EventArgs e)
    {
        System.Windows.Forms.Application.Exit();
    }
}
