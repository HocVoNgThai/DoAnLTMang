namespace DoAn
{
    partial class MainPage
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnNCC = new System.Windows.Forms.Button();
            this.btnBETA = new System.Windows.Forms.Button();
            this.btnTHVL = new System.Windows.Forms.Button();
            this.btnHTV = new System.Windows.Forms.Button();
            this.btnVTV = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(146, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(513, 45);
            this.label1.TabIndex = 0;
            this.label1.Text = "Lịch chiếu phim và truyền hình\r\n";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Comic Sans MS", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Brown;
            this.label2.Location = new System.Drawing.Point(96, 154);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 32);
            this.label2.TabIndex = 1;
            this.label2.Text = "Xem lịch:";
            // 
            // btnNCC
            // 
            this.btnNCC.BackColor = System.Drawing.Color.DarkGray;
            this.btnNCC.BackgroundImage = global::DoAn.Properties.Resources.NCC;
            this.btnNCC.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNCC.Location = new System.Drawing.Point(303, 625);
            this.btnNCC.Name = "btnNCC";
            this.btnNCC.Size = new System.Drawing.Size(208, 120);
            this.btnNCC.TabIndex = 5;
            this.btnNCC.UseVisualStyleBackColor = false;
            this.btnNCC.Click += new System.EventHandler(this.btnNCC_Click);
            // 
            // btnBETA
            // 
            this.btnBETA.BackColor = System.Drawing.Color.DarkGray;
            this.btnBETA.BackgroundImage = global::DoAn.Properties.Resources.BETA1;
            this.btnBETA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBETA.Location = new System.Drawing.Point(303, 506);
            this.btnBETA.Name = "btnBETA";
            this.btnBETA.Size = new System.Drawing.Size(208, 81);
            this.btnBETA.TabIndex = 6;
            this.btnBETA.UseVisualStyleBackColor = false;
            this.btnBETA.Click += new System.EventHandler(this.btnBETA_Click);
            // 
            // btnTHVL
            // 
            this.btnTHVL.BackColor = System.Drawing.Color.DarkGray;
            this.btnTHVL.BackgroundImage = global::DoAn.Properties.Resources.THVL;
            this.btnTHVL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTHVL.Location = new System.Drawing.Point(303, 389);
            this.btnTHVL.Name = "btnTHVL";
            this.btnTHVL.Size = new System.Drawing.Size(208, 81);
            this.btnTHVL.TabIndex = 4;
            this.btnTHVL.UseVisualStyleBackColor = false;
            this.btnTHVL.Click += new System.EventHandler(this.btnTHVL_Click);
            // 
            // btnHTV
            // 
            this.btnHTV.BackColor = System.Drawing.Color.DarkGray;
            this.btnHTV.BackgroundImage = global::DoAn.Properties.Resources.HTVicon;
            this.btnHTV.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnHTV.Location = new System.Drawing.Point(303, 267);
            this.btnHTV.Name = "btnHTV";
            this.btnHTV.Size = new System.Drawing.Size(208, 81);
            this.btnHTV.TabIndex = 3;
            this.btnHTV.UseVisualStyleBackColor = false;
            this.btnHTV.Click += new System.EventHandler(this.btnHTV_Click);
            // 
            // btnVTV
            // 
            this.btnVTV.BackColor = System.Drawing.Color.DarkGray;
            this.btnVTV.BackgroundImage = global::DoAn.Properties.Resources.VTVlogo_finish;
            this.btnVTV.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnVTV.Location = new System.Drawing.Point(303, 154);
            this.btnVTV.Name = "btnVTV";
            this.btnVTV.Size = new System.Drawing.Size(208, 81);
            this.btnVTV.TabIndex = 2;
            this.btnVTV.UseVisualStyleBackColor = false;
            this.btnVTV.Click += new System.EventHandler(this.btnVTV_Click);
            // 
            // MainPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 780);
            this.Controls.Add(this.btnBETA);
            this.Controls.Add(this.btnNCC);
            this.Controls.Add(this.btnTHVL);
            this.Controls.Add(this.btnHTV);
            this.Controls.Add(this.btnVTV);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MainPage";
            this.Text = "MainPage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnVTV;
        private System.Windows.Forms.Button btnHTV;
        private System.Windows.Forms.Button btnTHVL;
        private System.Windows.Forms.Button btnNCC;
        private System.Windows.Forms.Button btnBETA;
    }
}