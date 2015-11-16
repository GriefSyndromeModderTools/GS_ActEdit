namespace GS_ActEdit
{
    partial class FormPreview
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ButtonZoomIn = new System.Windows.Forms.Button();
            this.ButtonZoomOut = new System.Windows.Forms.Button();
            this.ButtonRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox1.Location = new System.Drawing.Point(9, 9);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(727, 393);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // ButtonZoomIn
            // 
            this.ButtonZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonZoomIn.Location = new System.Drawing.Point(9, 410);
            this.ButtonZoomIn.Name = "ButtonZoomIn";
            this.ButtonZoomIn.Size = new System.Drawing.Size(75, 23);
            this.ButtonZoomIn.TabIndex = 1;
            this.ButtonZoomIn.Text = "Zoom in";
            this.ButtonZoomIn.UseVisualStyleBackColor = true;
            this.ButtonZoomIn.Click += new System.EventHandler(this.ButtonZoomIn_Click);
            // 
            // ButtonZoomOut
            // 
            this.ButtonZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonZoomOut.Location = new System.Drawing.Point(90, 410);
            this.ButtonZoomOut.Name = "ButtonZoomOut";
            this.ButtonZoomOut.Size = new System.Drawing.Size(75, 23);
            this.ButtonZoomOut.TabIndex = 1;
            this.ButtonZoomOut.Text = "Zoom out";
            this.ButtonZoomOut.UseVisualStyleBackColor = true;
            this.ButtonZoomOut.Click += new System.EventHandler(this.ButtonZoomOut_Click);
            // 
            // ButtonRefresh
            // 
            this.ButtonRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ButtonRefresh.Location = new System.Drawing.Point(171, 410);
            this.ButtonRefresh.Name = "ButtonRefresh";
            this.ButtonRefresh.Size = new System.Drawing.Size(75, 23);
            this.ButtonRefresh.TabIndex = 1;
            this.ButtonRefresh.Text = "Refresh";
            this.ButtonRefresh.UseVisualStyleBackColor = true;
            this.ButtonRefresh.Click += new System.EventHandler(this.ButtonRefresh_Click);
            // 
            // FormPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 445);
            this.Controls.Add(this.ButtonRefresh);
            this.Controls.Add(this.ButtonZoomOut);
            this.Controls.Add(this.ButtonZoomIn);
            this.Controls.Add(this.pictureBox1);
            this.Name = "FormPreview";
            this.Text = "LayerPreview";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button ButtonZoomIn;
        private System.Windows.Forms.Button ButtonZoomOut;
        private System.Windows.Forms.Button ButtonRefresh;
    }
}