namespace DoAn
{
    partial class CPQG
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Panel_Outer = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Panel_Outer
            // 
            this.Panel_Outer.AutoScroll = true;
            this.Panel_Outer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Panel_Outer.Location = new System.Drawing.Point(0, 139);
            this.Panel_Outer.Name = "Panel_Outer";
            this.Panel_Outer.Size = new System.Drawing.Size(1334, 690);
            this.Panel_Outer.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label1.Location = new System.Drawing.Point(136, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(835, 45);
            this.label1.TabIndex = 2;
            this.label1.Text = "Danh sách phim đang chiếu tại rạp Chiếu phim QG\r\n";
            // 
            // CPQG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1334, 829);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Panel_Outer);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1352, 876);
            this.MinimumSize = new System.Drawing.Size(1352, 876);
            this.Name = "CPQG";
            this.Text = "Rạp Chiếu Phim Quốc Gia";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel Panel_Outer;
        private System.Windows.Forms.Label label1;
    }
}

