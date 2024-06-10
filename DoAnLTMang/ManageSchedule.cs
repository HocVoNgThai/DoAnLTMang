using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DoAn
{
    public partial class ManageSchedule : Form
    {
        public class TaskInfo
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Triggers { get; set; }
            public string DisplayName => $"{Name} - {Triggers} - {Description}";
        }
        public ManageSchedule()
        {
            InitializeComponent();
            LoadTaskScheduler();
        }
        private void LoadTaskScheduler()
        {
            try
            {
                using (TaskService ts = new TaskService())
                {
                    List<TaskInfo> taskInfos = new List<TaskInfo>();

                    // Lấy tất cả các folder trong Task Scheduler
                    GetTasksFromFolder(ts.RootFolder, taskInfos);

                    // Xuất danh sách task ra ListBox (hoặc điều khiển hiển thị khác)
                    //listBox1.DataSource = taskInfos;
                    //listBox1.DisplayMember = "DisplayName";
                    foreach (TaskInfo taskInfo in taskInfos)
                    {
                        TreeNode taskNode = new TreeNode(taskInfo.Name);
                        treeViewTask.Nodes.Add(taskNode);

                        // Tạo node cho trigger của task
                      
                        TreeNode triggerNode = new TreeNode("Trigger: " + taskInfo.Triggers);
                        taskNode.Nodes.Add(triggerNode);

                        // Tạo node cho definition của task
                        TreeNode definitionNode = new TreeNode("Definition: " + taskInfo.Description);
                        taskNode.Nodes.Add(definitionNode);
                    }
                    if(taskInfos.Count > 0)
                    {
                        lbNotify.Text = $"Có {taskInfos.Count} chương trình được đặt thông báo";
                    }
                    else
                    {
                        lbNotify.Text = "Không có chương trình nào được đặt thông báo";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void GetTasksFromFolder(TaskFolder folder, List<TaskInfo> taskInfos)
        {
            // Lấy tất cả các task trong folder hiện tại
            foreach (Microsoft.Win32.TaskScheduler.Task task in folder.Tasks)
            {
                string description = task.Definition.RegistrationInfo.Description;
                string triggers = string.Join("; ", task.Definition.Triggers.Select(t => t.ToString()));

                if (task.Name.StartsWith("TV Show Reminder"))
                {
                    taskInfos.Add(new TaskInfo
                    {
                        Name = task.Name,
                        Description = description,
                        Triggers = triggers
                    });
                } 
            }

            // Đệ quy để lấy task trong các subfolder
            foreach (TaskFolder subFolder in folder.SubFolders)
            {
                GetTasksFromFolder(subFolder, taskInfos);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            TreeNode nodeToRemove = treeViewTask.SelectedNode;
            if (MessageBox.Show("Are you sure to delete notification for this program?", "Recheck", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                                == DialogResult.Yes)
            {
                DeleteTask(nodeToRemove.Text);
                nodeToRemove.Remove();
            }
        }
        private void DeleteTask(string taskName)
        {
            try
            {
                using (TaskService ts = new TaskService())
                {
                    Microsoft.Win32.TaskScheduler.Task task = ts.FindTask(taskName);
                    if (task != null)
                    {
                        task.Folder.DeleteTask(taskName);
                        MessageBox.Show($"Task '{taskName}' has been deleted successfully.", "Task Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Task '{taskName}' not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}

