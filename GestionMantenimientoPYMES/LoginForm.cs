using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionMantenimientoPYMES
{
    public partial class LoginForm : MaterialSkin.Controls.MaterialForm
    {
        private readonly ApiClient _apiClient = new ApiClient();
        public LoginForm()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkin.MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(
            Primary.Blue900, Primary.Blue700, Primary.Blue500, Accent.Orange700, TextShade.WHITE
            );
        }

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            await LoginAsync();
        }

        private async Task LoginAsync()
        {
            string correo = materialTextBoxCorreo.Text;
            string contrasena = materialTextBoxContrasena.Text;

            try
            {
                var token = await _apiClient.Usuario.AuthenticateUserAsync(correo, contrasena);

                if (!string.IsNullOrEmpty(token))
                {
                    MaterialMessageBox.Show("Inicio de sesión exitoso!", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _apiClient.SetAuthToken(token);
                    Hide();
                    InicioForm inicioForm = new InicioForm(_apiClient);
                    inicioForm.Show();
                }
                else
                {
                    MaterialMessageBox.Show("Inicio de sesión fallido!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MaterialMessageBox.Show("Error inesperado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
