namespace TeamViewerLogReader.WinFormsApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lbl_Login = new Label();
            lbl_Password = new Label();
            txt_Login = new TextBox();
            txt_Password = new TextBox();
            label2 = new Label();
            btn_SignIn = new Button();
            btn_SignUp = new Button();
            SuspendLayout();
            // 
            // lbl_Login
            // 
            lbl_Login.AutoSize = true;
            lbl_Login.Font = new Font("Impact", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lbl_Login.ForeColor = Color.White;
            lbl_Login.Location = new Point(105, 210);
            lbl_Login.Name = "lbl_Login";
            lbl_Login.Size = new Size(53, 22);
            lbl_Login.TabIndex = 0;
            lbl_Login.Text = "Login:";
            // 
            // lbl_Password
            // 
            lbl_Password.AutoSize = true;
            lbl_Password.Font = new Font("Impact", 9F, FontStyle.Regular, GraphicsUnit.Point);
            lbl_Password.ForeColor = Color.White;
            lbl_Password.Location = new Point(105, 261);
            lbl_Password.Name = "lbl_Password";
            lbl_Password.Size = new Size(83, 22);
            lbl_Password.TabIndex = 1;
            lbl_Password.Text = "Password:";
            // 
            // txt_Login
            // 
            txt_Login.Location = new Point(195, 205);
            txt_Login.Name = "txt_Login";
            txt_Login.Size = new Size(295, 31);
            txt_Login.TabIndex = 2;
            // 
            // txt_Password
            // 
            txt_Password.Location = new Point(194, 252);
            txt_Password.Name = "txt_Password";
            txt_Password.Size = new Size(295, 31);
            txt_Password.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Impact", 30F, FontStyle.Regular, GraphicsUnit.Point);
            label2.ForeColor = Color.White;
            label2.Location = new Point(105, 66);
            label2.Name = "label2";
            label2.Size = new Size(383, 75);
            label2.TabIndex = 4;
            label2.Text = "CFP-TV Reader";
            // 
            // btn_SignIn
            // 
            btn_SignIn.Font = new Font("Impact", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btn_SignIn.ForeColor = Color.FromArgb(0, 33, 120);
            btn_SignIn.Location = new Point(105, 323);
            btn_SignIn.Name = "btn_SignIn";
            btn_SignIn.Size = new Size(112, 34);
            btn_SignIn.TabIndex = 5;
            btn_SignIn.Text = "Sign in";
            btn_SignIn.UseVisualStyleBackColor = true;
            btn_SignIn.Click += btn_SignIn_Click;
            // 
            // btn_SignUp
            // 
            btn_SignUp.Font = new Font("Impact", 9F, FontStyle.Regular, GraphicsUnit.Point);
            btn_SignUp.ForeColor = Color.FromArgb(0, 33, 120);
            btn_SignUp.Location = new Point(105, 374);
            btn_SignUp.Name = "btn_SignUp";
            btn_SignUp.Size = new Size(112, 34);
            btn_SignUp.TabIndex = 6;
            btn_SignUp.Text = "Sign Up";
            btn_SignUp.UseVisualStyleBackColor = true;
            btn_SignUp.Click += btn_SignUp_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(0, 33, 120);
            ClientSize = new Size(800, 450);
            Controls.Add(btn_SignUp);
            Controls.Add(btn_SignIn);
            Controls.Add(label2);
            Controls.Add(txt_Password);
            Controls.Add(txt_Login);
            Controls.Add(lbl_Password);
            Controls.Add(lbl_Login);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label lbl_Login;
        private Label lbl_Password;
        private TextBox txt_Login;
        private TextBox txt_Password;
        private Label label2;
        private Button btn_SignIn;
        private Button btn_SignUp;
    }
}