using TeamViewerLogReader.Domain;
using TeamViewerLogReader.Service;
using TeamViewerLogReader.Service.Interfaces;

namespace TeamViewerLogReader.WinFormsApp
{
    public partial class AdminUser : Form
    {
        private readonly IUserTvLogService _service;
        private UserTvLog _user { get; set; }

        public AdminUser(IUserTvLogService service, UserTvLog? user = null)
        {
            InitializeComponent();
            _service = service;
            if (user != null)
            {
                _user = user;
                ModeUpdate(user);
            }
        }

        private void ModeUpdate(UserTvLog user)
        {
            txt_FullName.Text = user.Name;
            txt_Surname.Text = user.Surname;
            txt_Username.Text = user.Username;
            txt_Password.Text = string.Empty;
            txt_ConfirmPassword.Visible = true;
            lbl_ConfirmPassword.Visible = true;
            txt_Phone.Text = user.PhoneNumber;
            txt_Position.Text = user.Position;
            txt_Company.Text = user.Company;
            btn_action.Text = "Update";
            lbl_title.Text = "Update User";
            txt_Email.Text = user.Email;
        }

        private void btn_action_Click(object sender, EventArgs e)
        {
            try
            {
                if (btn_action.Text == "Update")
                {
                    _service.Update(GetUserUpdate());
                    MessageBox.Show("User successfully updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    _service.Create(GetUserCreate());
                    MessageBox.Show("User successfully created.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private UserTvLog GetUserCreate()
        {
            return new UserTvLog()
            {
                Company = txt_Company.Text,
                Name = txt_FullName.Text,
                Surname = txt_Surname.Text,
                PasswordHash = txt_Password.Text,
                PhoneNumber = txt_Phone.Text,
                Position = txt_Position.Text,
                Username = txt_Username.Text,
                Email = txt_Email.Text
            };
        }

        private UserTvLog GetUserUpdate()
        {
            if (txt_Password.Text != txt_ConfirmPassword.Text)
            {
                throw new InvalidOperationException("Passwords do not match.");
            }
            _user.Company = txt_Company.Text;
            _user.Name = txt_FullName.Text;
            _user.Surname = txt_Surname.Text;
            _user.PasswordHash = (txt_Password.Text != string.Empty) ? txt_Password.Text : _user.PasswordHash;
            _user.PhoneNumber = txt_Phone.Text;
            _user.Position = txt_Position.Text;
            _user.Username = txt_Username.Text;
            _user.Email = txt_Email.Text;
            return _user;
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
