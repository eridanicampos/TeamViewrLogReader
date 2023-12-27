
using TeamViewerLogReader.Service.DTOs;
using TeamViewerLogReader.Service.Interfaces;

namespace TeamViewerLogReader.WinFormsApp
{
    public partial class Form1 : Form
    {
        private readonly IUserTvLogService _service;
        public Form1(IUserTvLogService service)
        {
            InitializeComponent();
            _service = service;
        }


        private void btn_SignIn_Click(object sender, EventArgs e)
        {

            var loginDto = new LoginDTO();
            loginDto.Login = txt_Login.Text;
            loginDto.Password = txt_Password.Text;

            try
            {
                var objUser = _service.Login(loginDto);
                if (objUser != null)
                {
                    this.Hide();

                    AdminUser adminForm = new AdminUser(_service, objUser);
                    adminForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Sorry",
                                    "Incorrect username or password!",
                                    MessageBoxButtons.OK);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void btn_SignUp_Click(object sender, EventArgs e)
        {
            this.Hide();

            AdminUser adminForm = new AdminUser(_service);
            adminForm.ShowDialog();
        }

    }
}