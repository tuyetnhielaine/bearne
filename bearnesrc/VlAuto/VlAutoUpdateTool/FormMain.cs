/*
########################
@Phát triển thêm từ source của VlAutoUpdateTool.
@Developer: Tuyết Nhi + yulum.
@Released date: 20250819.

Tối ưu hiển thị và loại bỏ thư mục Game.
Tạo danh sách file đẹp hơn.
Bảo vệ hàm tránh lỗi.
Bổ sung nhiều logic mới.
########################
*/

using System.Diagnostics;		//add DEBUG.
using System.Security.Cryptography;
using VlAutoUpdateTool.Models;

namespace VlAutoUpdateTool
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            /* by Tuyết Nhi */
            SetupImageList();
        }

        private void buttonFileSelect_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                var filePath = folderBrowserDialog.SelectedPath;
                textBoxFolderPath.Text = filePath;
                ListDirectory(treeViewPath, filePath);
            }
        }

        private void ListDirectory(TreeView treeView, string path)
        {
            treeView.Nodes.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);
            treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo));
            foreach (TreeNode node in treeView.Nodes)
                node.Expand();
        }

        /* by Tuyết Nhi */
        private static TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeNode(directoryInfo.Name)
            {
                Tag = directoryInfo.FullName,
                Checked = true,
                ImageIndex = 0,             // Icon thư mục
                SelectedImageIndex = 0
            };

            try
            {
                foreach (var directory in directoryInfo.GetDirectories())
                {
                    directoryNode.Nodes.Add(CreateDirectoryNode(directory));
                }

                foreach (var file in directoryInfo.GetFiles())
                {
                    directoryNode.Nodes.Add(new TreeNode(file.Name)
                    {
                        Tag = file.FullName,
                        Checked = true,
                        ImageIndex = 1,         // Icon file
                        SelectedImageIndex = 1
                    });
                }
            }
            catch (UnauthorizedAccessException)
            {
                directoryNode.Nodes.Add(new TreeNode("Không thể truy cập file.")
                {
                    ImageIndex = 1,
                    SelectedImageIndex = 1
                });
            }

            return directoryNode;
        }

        /* by Tuyết Nhi */
        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            // Reserved for future use
            // Kiểm tra đã chọn thư mục chưa, tránh lỗi The value cannot be an empty string. (Parameter 'oldValue')
            if (string.IsNullOrEmpty(textBoxFolderPath.Text) || !Directory.Exists(textBoxFolderPath.Text))
            {
                MessageBox.Show("Vui lòng chọn thư mục để tạo checksum.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
			
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Chọn một file để thêm";
            openFileDialog.Filter = "Tất cả các file (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                AddSingleFileToTree(filePath);
            }
        }

        /* by Tuyết Nhi */
        private void AddSingleFileToTree(string filePath)
        {
            // Kiểm tra file tồn tại
            if (!File.Exists(filePath))
            {
                MessageBox.Show("File không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kiểm tra file đã có trong TreeView chưa, tránh bị trùng file.
            if (IsPathAlreadyInTree(filePath))
            {
                MessageBox.Show("File này đã được thêm vào danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Thêm node mới
            string fileName = Path.GetFileName(filePath);

            TreeNode fileNode = new TreeNode(fileName)
            {
                Tag = filePath,
                Checked = true,
                ImageIndex = 1,              // Icon file
                SelectedImageIndex = 1
            };

            treeViewPath.Nodes.Add(fileNode);
            fileNode.EnsureVisible();       // Cuộn tới node vừa thêm nếu cần
        }

        /* by Tuyết Nhi */
        private bool IsPathAlreadyInTree(string path)
        {
            foreach (TreeNode node in treeViewPath.Nodes)
            {
                if (IsPathExistsInNode(node, path))
                    return true;
            }
            return false;
        }

        /* by Tuyết Nhi */
        private bool IsPathExistsInNode(TreeNode node, string path)
        {
            if (node.Tag is string existingPath &&
                string.Equals(existingPath, path, StringComparison.OrdinalIgnoreCase))
                return true;
            foreach (TreeNode child in node.Nodes)
            {
                if (IsPathExistsInNode(child, path))
                    return true;
            }
            return false;
        }

        /* by Tuyết Nhi */
        private bool allChecked = false;
        private void buttonSaveConfig_Click(object sender, EventArgs e)
        {
            // Reserved for future use
            if (treeViewPath.Nodes.Count == 0)
            {
                MessageBox.Show("Chưa có file nào để check hoặc uncheck.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            allChecked = !allChecked;
            CheckAllNodes(treeViewPath.Nodes, allChecked);
        }

        /* by Tuyết Nhi */
        private void CheckAllNodes(TreeNodeCollection nodes, bool check)
        {
            foreach (TreeNode node in nodes)
            {
                node.Checked = check;
                if (node.Nodes.Count > 0)
                    CheckAllNodes(node.Nodes, check);
            }
        }

        /* by Tuyết Nhi */
        private void buttonCreateChecksum_Click(object sender, EventArgs e)
        {
            // Kiểm tra đã chọn thư mục chưa, tránh lỗi The value cannot be an empty string. (Parameter 'oldValue')
            if (string.IsNullOrEmpty(textBoxFolderPath.Text) || !Directory.Exists(textBoxFolderPath.Text))
            {
                MessageBox.Show("Vui lòng chọn thư mục để tạo checksum.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

			//by Tuyết Nhi, fix người dùng không check thư mục cha trong treeViewPath.Nodes, vẫn tạo được checksum khiến cho danh sách bị trống.
			if (!HasCheckedFolders(treeViewPath.Nodes))
			{
				MessageBox.Show("Vui lòng chọn thư mục cha trong danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

            // Kiểm tra xem có file nào được chọn (Checked) không
            if (!HasAnyCheckedFile(treeViewPath.Nodes))
            {
                MessageBox.Show("Không có file nào được chọn để tạo checksum.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Kiểm tra file lỗi
            List<string> invalidFiles = new List<string>();
            CheckInvalidFiles(treeViewPath.Nodes, invalidFiles);

            if (invalidFiles.Count > 0)
            {
                string message = "Các file hoặc thư mục sau có ký tự không hợp lệ:\n\n" + string.Join("\n", invalidFiles);
                MessageBox.Show(message, "Lỗi tên file/thư mục", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<ChecksumModel> models = CreateChecksum();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(models, Newtonsoft.Json.Formatting.Indented);
            string updateFolderPath = textBoxFolderPath.Text;

            if (string.IsNullOrWhiteSpace(updateFolderPath) || !Directory.Exists(updateFolderPath))
            {
                MessageBox.Show("Bạn không chọn thư mục hoặc thư mục không tồn tại, file checksum sẽ lưu tại thư mục chạy ứng dụng.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                updateFolderPath = AppDomain.CurrentDomain.BaseDirectory;
            }

            // Tạo thư mục nếu chưa tồn tại
            Directory.CreateDirectory(updateFolderPath);

            string fileName = Path.Combine(updateFolderPath, "checksum.txt");
            File.WriteAllText(fileName, json);

            // Tự mở thư mục chứa file checksum
            System.Diagnostics.Process.Start("explorer.exe", updateFolderPath);
            MessageBox.Show($"Tạo checksum thành công!\nFile lưu tại: {updateFolderPath}", "Thông báo");
        }

        /* by Tuyết Nhi */
        // cấm đặt tên có ký tự đặc biệt, nó có thể khiến autoupdate không thể tải về.
        private static readonly char[] InvalidFileNameChars =
        {
            '<', '>', ':', '"', '/', '\\', '|', '?', '*', ' ',
            '&', '%', '#', '+', '=', '{', '}', '[', ']', '`', '^', '~',
            ';', ',', '!', '@', '$', '\'', '(', ')', '\t', '\r', '\n'
        };

        /* by Tuyết Nhi */
        private void CheckInvalidFiles(TreeNodeCollection nodes, List<string> invalidFiles)
        {
            const int MaxPathLength = 240;		//đường dẫn file dài tối đa 240 ký tự.

            foreach (TreeNode node in nodes)
            {
                if (!node.Checked) continue;

                if (node.Tag is string path)
                {
                    // Lấy tên file hoặc thư mục cuối cùng
                    string name = Path.GetFileName(path.TrimEnd(Path.DirectorySeparatorChar));

                    bool tooLong = path.Length >= MaxPathLength;
                    bool invalidChar = name.IndexOfAny(InvalidFileNameChars) >= 0;

                    if (tooLong || invalidChar)
                    {
                        invalidFiles.Add(path);
                    }
                }

                if (node.Nodes.Count > 0)
                {
                    CheckInvalidFiles(node.Nodes, invalidFiles);
                }
            }
        }

        /* by Tuyết Nhi */
		private bool HasCheckedFolders(TreeNodeCollection nodes)
		{
			foreach (TreeNode node in nodes)
			{
				if (node.Checked)
				{
					// Nếu node cha được checked, thì ok, kiểm tra con tiếp
					if (node.Nodes.Count > 0)
					{
						// Kiểm tra đệ quy trong node con
						if (HasCheckedFolders(node.Nodes))
							return true;
						else
							return true; // node cha checked => tính là có checked folder
					}
					else
					{
						return true;
					}
				}
			}
			return false;
		}
		/* by Tuyết Nhi */
        private bool HasAnyCheckedFile(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                // Nếu là file và được checked
                if (node.Checked && File.Exists(node.Tag?.ToString()))
                    return true;
                if (HasAnyCheckedFile(node.Nodes))
                    return true;
            }

            return false;
        }

        private List<ChecksumModel> CreateChecksum()
        {
            string rootPath = textBoxFolderPath.Text;
            List<ChecksumModel> models = new();

            if (treeViewPath.Nodes.Count > 0)
            {
                using var hashAlgorithm = MD5.Create();

                foreach (TreeNode node in treeViewPath.Nodes)
                {
                    AddFileToChecksum(node, rootPath, hashAlgorithm, models);
                }
            }

            return models;
        }

        private void AddFileToChecksum(TreeNode node, string rootPath, HashAlgorithm hashAlgorithm, List<ChecksumModel> models)
        {
            /* by Tuyết Nhi */
            //tránh lỗi The value cannot be an empty string. (Parameter 'oldValue')
            if (string.IsNullOrWhiteSpace(rootPath) || !Directory.Exists(rootPath))
            {
                MessageBox.Show(
                    "Bạn chưa chọn thư mục hoặc thư mục không tồn tại.\n" +
                    "Chương trình sẽ lưu file checksum vào thư mục chạy ứng dụng.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                rootPath = AppDomain.CurrentDomain.BaseDirectory;
            }

            if (node == null || node.Tag is not string path || !node.Checked)
                return;

            if (File.Exists(path))
            {
                FileInfo fileInfo = new FileInfo(path);
                string relativePath = path.Replace(rootPath, string.Empty).Replace("\\", "/").TrimStart('/');

                ChecksumModel model = new()
                {
                    Path = path,
                    AbsolutePath = relativePath,
                    UrlPath = relativePath,
                    Hash = ComputeHash(fileInfo, hashAlgorithm),
                    /* by Tuyết Nhi */
                    Size = fileInfo.Length  // <-- Thêm dòng này để lưu kích thước file (byte)
                };

                models.Add(model);
            }

            foreach (TreeNode child in node.Nodes)
            {
                AddFileToChecksum(child, rootPath, hashAlgorithm, models);
            }
        }

        public string ComputeHash(FileInfo fileInfo, HashAlgorithm hashAlgorithm)
        {
            using var fs = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var hash = hashAlgorithm.ComputeHash(fs);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLowerInvariant();
        }

        private void treeViewPath_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null) return;

            bool isChecked = e.Node.Checked;
            foreach (TreeNode child in e.Node.Nodes)
            {
                child.Checked = isChecked;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            //nút thư mục không cần...
        }

        /* by Tuyết Nhi */
        private void SetupImageList()
        {
            ImageList imageListIcons = new ImageList();
            imageListIcons.ImageSize = new Size(16, 16); // kích thước

            // Load ảnh từ thư mục Resources.resx
            imageListIcons.Images.Add(Properties.Resources.folder); // thư mục index 0
            imageListIcons.Images.Add(Properties.Resources.file);   // tập tin index 1

            treeViewPath.ImageList = imageListIcons;
        }

    }
}
