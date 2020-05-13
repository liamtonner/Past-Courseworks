namespace xlocation
{
    partial class clientUI
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
        public void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Input = new System.Windows.Forms.Label();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.Send = new System.Windows.Forms.Button();
            this.OutputLabel = new System.Windows.Forms.Label();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SuspendLayout();
            // 
            // Input
            // 
            this.Input.AutoSize = true;
            this.Input.Location = new System.Drawing.Point(38, 35);
            this.Input.Name = "Input";
            this.Input.Size = new System.Drawing.Size(86, 32);
            this.Input.TabIndex = 0;
            this.Input.Text = "Input:";
            this.Input.Click += new System.EventHandler(this.label1_Click);
            // 
            // InputBox
            // 
            this.InputBox.Location = new System.Drawing.Point(131, 35);
            this.InputBox.Name = "InputBox";
            this.InputBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.InputBox.Size = new System.Drawing.Size(632, 38);
            this.InputBox.TabIndex = 1;
            this.InputBox.TextChanged += new System.EventHandler(this.InputBox_TextChanged);
            // 
            // Send
            // 
            this.Send.Location = new System.Drawing.Point(323, 79);
            this.Send.Name = "Send";
            this.Send.Size = new System.Drawing.Size(196, 48);
            this.Send.TabIndex = 2;
            this.Send.Text = "Send";
            this.Send.UseVisualStyleBackColor = true;
            this.Send.Click += new System.EventHandler(this.Send_Click);
            // 
            // OutputLabel
            // 
            this.OutputLabel.AutoSize = true;
            this.OutputLabel.Location = new System.Drawing.Point(38, 165);
            this.OutputLabel.Name = "OutputLabel";
            this.OutputLabel.Size = new System.Drawing.Size(109, 32);
            this.OutputLabel.TabIndex = 4;
            this.OutputLabel.Text = "Output:";
            // 
            // TextBox1
            // 
            this.TextBox1.Location = new System.Drawing.Point(153, 165);
            this.TextBox1.Multiline = true;
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBox1.Size = new System.Drawing.Size(610, 166);
            this.TextBox1.TabIndex = 5;
            this.TextBox1.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // clientUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 506);
            this.Controls.Add(this.TextBox1);
            this.Controls.Add(this.OutputLabel);
            this.Controls.Add(this.Send);
            this.Controls.Add(this.InputBox);
            this.Controls.Add(this.Input);
            this.Name = "clientUI";
            this.Text = "Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Input;
        private System.Windows.Forms.TextBox InputBox;
        private System.Windows.Forms.Button Send;
        private System.Windows.Forms.Label OutputLabel;
        public System.Windows.Forms.TextBox TextBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}