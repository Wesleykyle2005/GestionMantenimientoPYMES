using SharedModels.ModelsDTO.Empresa;
using SharedModels.ModelsDTO.Equipo;
using SharedModels.ModelsDTO.Mantenimiento;
using SharedModels.ModelsDTO.Usuario;

namespace GestionMantenimientoPYMES;

public partial class InicioForm : MaterialSkin.Controls.MaterialForm
{
    private readonly ApiClient _apiClient;

    // ============================
    #region VARIABLES USUARIOS
    // ============================
    private UsuarioResponseDTO? usuarioSeleccionado = null;
    private List<EmpresaResponseDTO> empresasUsuarios = new List<EmpresaResponseDTO>();
    #endregion

    // ============================
    #region VARIABLES MANTENIMIENTO
    // ============================
    private MantenimientoResponseDTO? mantenimientoSeleccionado = null;
    private List<EquipoResponseDTO> equipos = new List<EquipoResponseDTO>();
    #endregion


    public InicioForm(ApiClient apiClient)
    {
        InitializeComponent();
        _apiClient = apiClient;

        #region Inicialización de Eventos y Configuración
        // Empresas
        ConfigurarEmpresaDataGridView();
        // Equipos
        ConfigurarEquiposDataGridView();
        //Mantenimientos
        ConfigurarMantenimientoDataGridView();
        // Usuarios
        ConfigurarUsuariosDataGridView();
        // Form Load
        this.Load += InicioForm_Load;
        #endregion

    }

    private async void InicioForm_Load(object sender, EventArgs e)
    {
        #region Carga inicial de datos
        await LoadEmpresasAsync();
        await CargarEmpresasComboAsync();
        CargarEstadosCombo();
        await LoadEquiposAsync();
        await CargarEquiposComboAsync();
        CargarTipoMantenimientoCombo();
        CargarEstadoMantenimientoCombo();
        await LoadMantenimientosAsync();
        await CargarEmpresasUsuariosComboAsync();
        CargarRolesCombo();
        await LoadUsuariosAsync();



        #endregion
    }

    // ============================
    #region EMPRESAS
    // ============================

    private EmpresaResponseDTO? empresaSeleccionada = null;

    private void ConfigurarEmpresaDataGridView()
    {
        EmpresaDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        EmpresaDataGridView.MultiSelect = false;
        EmpresaDataGridView.ReadOnly = true;
        EmpresaDataGridView.AllowUserToAddRows = false;
        EmpresaDataGridView.AllowUserToDeleteRows = false;
        EmpresaDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        EmpresaDataGridView.BackgroundColor = Color.White;
        EmpresaDataGridView.DefaultCellStyle.BackColor = Color.White;
        EmpresaDataGridView.DefaultCellStyle.ForeColor = Color.Black;
        EmpresaDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(35, 57, 93);
        EmpresaDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        EmpresaDataGridView.EnableHeadersVisualStyles = false;
    }

    public async Task LoadEmpresasAsync()
    {
        try
        {
            var empresas = await _apiClient.Empresa.GetAllAsync();
            EmpresaDataGridView.DataSource = empresas.ToList();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar empresas: {ex.Message}");
        }
    }

    private void EmpresaDataGridView_SelectionChanged(object sender, EventArgs e)
    {
        if (EmpresaDataGridView.SelectedRows.Count > 0)
        {
            var row = EmpresaDataGridView.SelectedRows[0].DataBoundItem as EmpresaResponseDTO;
            if (row != null)
            {
                empresaSeleccionada = row;
                NombreTextBox.Text = row.Nombre;
                DireccionTextBox.Text = row.Direccion;
                TelefonoTextBox.Text = row.Telefono;
            }
        }
    }

    private async void CrearButton_Click(object sender, EventArgs e)
    {
        var dto = new EmpresaCreateDTO
        {
            Nombre = NombreTextBox.Text,
            Direccion = DireccionTextBox.Text,
            Telefono = TelefonoTextBox.Text
        };

        try
        {
            await _apiClient.Empresa.CreateAsync(dto);
            await LoadEmpresasAsync();
            MessageBox.Show("Empresa creada correctamente.");
            LimpiarCamposEmpresa();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al crear empresa: {ex.Message}");
        }
    }

    private async void ActualizarButton_Click(object sender, EventArgs e)
    {
        if (empresaSeleccionada == null)
        {
            MessageBox.Show("Selecciona una empresa para actualizar.");
            return;
        }

        var dto = new EmpresaUpdateDTO
        {
            EmpresaId = empresaSeleccionada.EmpresaId,
            Nombre = NombreTextBox.Text,
            Direccion = DireccionTextBox.Text,
            Telefono = TelefonoTextBox.Text
        };

        try
        {
            await _apiClient.Empresa.UpdateAsync(dto.EmpresaId, dto);
            await LoadEmpresasAsync();
            MessageBox.Show("Empresa actualizada correctamente.");
            LimpiarCamposEmpresa();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al actualizar empresa: {ex.Message}");
        }
    }

    private async void BorrarButton_Click(object sender, EventArgs e)
    {
        if (empresaSeleccionada == null)
        {
            MessageBox.Show("Selecciona una empresa para borrar.");
            return;
        }

        var confirm = MessageBox.Show("¿Seguro que deseas eliminar esta empresa?", "Confirmar", MessageBoxButtons.YesNo);
        if (confirm == DialogResult.Yes)
        {
            try
            {
                await _apiClient.Empresa.DeleteAsync(empresaSeleccionada.EmpresaId);
                await LoadEmpresasAsync();
                MessageBox.Show("Empresa eliminada correctamente.");
                LimpiarCamposEmpresa();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar empresa: {ex.Message}");
            }
        }
    }

    private void LimpiarCamposEmpresa()
    {
        NombreTextBox.Text = "";
        DireccionTextBox.Text = "";
        TelefonoTextBox.Text = "";
        empresaSeleccionada = null;
    }

    #endregion

    // ============================
    #region EQUIPOS
    // ============================

    private EquipoResponseDTO? equipoSeleccionado = null;
    private List<EmpresaResponseDTO> empresas = new List<EmpresaResponseDTO>();

    private void ConfigurarEquiposDataGridView()
    {
        EquiposDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        EquiposDataGridView.MultiSelect = false;
        EquiposDataGridView.ReadOnly = true;
        EquiposDataGridView.AllowUserToAddRows = false;
        EquiposDataGridView.AllowUserToDeleteRows = false;
        EquiposDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        EquiposDataGridView.BackgroundColor = Color.White;
        EquiposDataGridView.DefaultCellStyle.BackColor = Color.White;
        EquiposDataGridView.DefaultCellStyle.ForeColor = Color.Black;
        EquiposDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(35, 57, 93);
        EquiposDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        EquiposDataGridView.EnableHeadersVisualStyles = false;
    }

    private async Task CargarEmpresasComboAsync()
    {
        try
        {
            var empresasList = await _apiClient.Empresa.GetAllAsync();
            empresas = empresasList.ToList();
            EmpresaEquipoComboBox.DataSource = empresas;
            EmpresaEquipoComboBox.DisplayMember = "Nombre";
            EmpresaEquipoComboBox.ValueMember = "EmpresaId";
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al cargar empresas para equipos: " + ex.Message);
        }
    }

    private void CargarEstadosCombo()
    {
        EstadoEquipoComboBox.Items.Clear();
        EstadoEquipoComboBox.Items.Add("Operativo");
        EstadoEquipoComboBox.Items.Add("En mantenimiento");
        EstadoEquipoComboBox.SelectedIndex = 0;
    }

    public async Task LoadEquiposAsync()
    {
        try
        {
            // 1. Obtén todas las empresas (solo una vez si ya las tienes)
            var empresas = await _apiClient.Empresa.GetAllAsync();
            var empresasDict = empresas.ToDictionary(e => e.EmpresaId, e => e.Nombre);

            // 2. Obtén los equipos
            var equipos = await _apiClient.Equipo.GetAllAsync();
            var equiposList = equipos.ToList();

            // 3. Llena el campo EmpresaNombre en cada equipo
            foreach (var eq in equiposList)
            {
                if (empresasDict.TryGetValue(eq.EmpresaId, out var nombreEmpresa))
                    eq.EmpresaNombre = nombreEmpresa;
                else
                    eq.EmpresaNombre = "Desconocida";
            }

            // 4. Asigna al DataGridView
            EquiposDataGridView.DataSource = null;
            EquiposDataGridView.DataSource = equiposList;

            // 5. Oculta el Id de la empresa
            if (EquiposDataGridView.Columns["EmpresaId"] != null)
                EquiposDataGridView.Columns["EmpresaId"].Visible = false;
            if (EquiposDataGridView.Columns["EquipoId"] != null)
                EquiposDataGridView.Columns["EquipoId"].Visible = false;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar equipos: {ex.Message}");
        }
    }


    private void EquiposDataGridView_SelectionChanged(object sender, EventArgs e)
    {
        if (EquiposDataGridView.SelectedRows.Count > 0)
        {
            var row = EquiposDataGridView.SelectedRows[0].DataBoundItem as EquipoResponseDTO;
            if (row != null)
            {
                equipoSeleccionado = row;
                NombreEquipoTextBox.Text = row.Nombre;
                TipoEquipoTextBox.Text = row.Tipo;
                MarcaEquipoTextBox.Text = row.Marca;
                ModeloEquipoTextBox.Text = row.Modelo;
                EstadoEquipoComboBox.SelectedItem = row.Estado;
                var empresa = empresas.FirstOrDefault(emp => emp.EmpresaId == row.EmpresaId);
                if (empresa != null)
                    EmpresaEquipoComboBox.SelectedItem = empresa;
            }
        }
    }

    private async void CrearEquipoButton_Click(object sender, EventArgs e)
    {
        if (EmpresaEquipoComboBox.SelectedItem is not EmpresaResponseDTO empresaSeleccionadaCombo)
        {
            MessageBox.Show("Selecciona una empresa.");
            return;
        }

        var dto = new EquipoCreateDTO
        {
            Nombre = NombreEquipoTextBox.Text,
            Tipo = TipoEquipoTextBox.Text,
            Marca = MarcaEquipoTextBox.Text,
            Modelo = ModeloEquipoTextBox.Text,
            Estado = EstadoEquipoComboBox.SelectedItem?.ToString() ?? "Operativo",
            EmpresaId = empresaSeleccionadaCombo.EmpresaId
        };

        try
        {
            await _apiClient.Equipo.CreateAsync(dto);
            await LoadEquiposAsync();
            MessageBox.Show("Equipo creado correctamente.");
            LimpiarCamposEquipo();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al crear equipo: {ex.Message}");
        }
    }

    private async void ActualizarEquipoButton_Click(object sender, EventArgs e)
    {
        if (equipoSeleccionado == null)
        {
            MessageBox.Show("Selecciona un equipo para actualizar.");
            return;
        }
        if (EmpresaEquipoComboBox.SelectedItem is not EmpresaResponseDTO empresaSeleccionadaCombo)
        {
            MessageBox.Show("Selecciona una empresa.");
            return;
        }

        var dto = new EquipoUpdateDTO
        {
            EquipoId = equipoSeleccionado.EquipoId,
            Nombre = NombreEquipoTextBox.Text,
            Tipo = TipoEquipoTextBox.Text,
            Marca = MarcaEquipoTextBox.Text,
            Modelo = ModeloEquipoTextBox.Text,
            Estado = EstadoEquipoComboBox.SelectedItem?.ToString() ?? "Operativo",
            EmpresaId = empresaSeleccionadaCombo.EmpresaId
        };

        try
        {
            await _apiClient.Equipo.UpdateAsync(dto.EquipoId, dto);
            await LoadEquiposAsync();
            MessageBox.Show("Equipo actualizado correctamente.");
            LimpiarCamposEquipo();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al actualizar equipo: {ex.Message}");
        }
    }

    private async void BorrarEquipoButton_Click(object sender, EventArgs e)
    {
        if (equipoSeleccionado == null)
        {
            MessageBox.Show("Selecciona un equipo para borrar.");
            return;
        }

        var confirm = MessageBox.Show("¿Seguro que deseas eliminar este equipo?", "Confirmar", MessageBoxButtons.YesNo);
        if (confirm == DialogResult.Yes)
        {
            try
            {
                await _apiClient.Equipo.DeleteAsync(equipoSeleccionado.EquipoId);
                await LoadEquiposAsync();
                MessageBox.Show("Equipo eliminado correctamente.");
                LimpiarCamposEquipo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar equipo: {ex.Message}");
            }
        }
    }

    private void LimpiarCamposEquipo()
    {
        NombreEquipoTextBox.Text = "";
        TipoEquipoTextBox.Text = "";
        MarcaEquipoTextBox.Text = "";
        ModeloEquipoTextBox.Text = "";
        EstadoEquipoComboBox.SelectedIndex = 0;
        EmpresaEquipoComboBox.SelectedIndex = 0;
        equipoSeleccionado = null;
    }

    #endregion



    // ============================
    #region MANTENIMIENTO - CONFIGURACIÓN Y CARGA DE DATOS
    // ============================

    private void ConfigurarMantenimientoDataGridView()
    {
        MantenimientoDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        MantenimientoDataGridView.MultiSelect = false;
        MantenimientoDataGridView.ReadOnly = true;
        MantenimientoDataGridView.AllowUserToAddRows = false;
        MantenimientoDataGridView.AllowUserToDeleteRows = false;
        MantenimientoDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        MantenimientoDataGridView.BackgroundColor = Color.White;
        MantenimientoDataGridView.DefaultCellStyle.BackColor = Color.White;
        MantenimientoDataGridView.DefaultCellStyle.ForeColor = Color.Black;
        MantenimientoDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(35, 57, 93);
        MantenimientoDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        MantenimientoDataGridView.EnableHeadersVisualStyles = false;
    }

    private async Task CargarEquiposComboAsync()
    {
        try
        {
            var equiposList = await _apiClient.Equipo.GetAllAsync();
            equipos = equiposList.ToList();
            EquipoComboBox.DataSource = equipos;
            EquipoComboBox.DisplayMember = "Nombre";
            EquipoComboBox.ValueMember = "EquipoId";
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al cargar equipos: " + ex.Message);
        }
    }

    private void CargarTipoMantenimientoCombo()
    {
        TipoMantenimientoComboBox.Items.Clear();
        TipoMantenimientoComboBox.Items.Add("Preventivo");
        TipoMantenimientoComboBox.Items.Add("Correctivo");
        TipoMantenimientoComboBox.SelectedIndex = 0;
    }

    private void CargarEstadoMantenimientoCombo()
    {
        EstadoMantenimientoComboBox.Items.Clear();
        EstadoMantenimientoComboBox.Items.Add("Pendiente");
        EstadoMantenimientoComboBox.Items.Add("En proceso");
        EstadoMantenimientoComboBox.Items.Add("Completado");
        EstadoMantenimientoComboBox.SelectedIndex = 0;
    }

    public async Task LoadMantenimientosAsync()
    {
        try
        {

            var empresas = await _apiClient.Empresa.GetAllAsync();
            var empresasDict = empresas.ToDictionary(e => e.EmpresaId, e => e.Nombre);


            var equipos = await _apiClient.Equipo.GetAllAsync();
            var equiposDict = equipos.ToDictionary(eq => eq.EquipoId, eq => eq);


            var mantenimientos = await _apiClient.Mantenimiento.GetAllAsync();
            var mantenimientosList = mantenimientos.ToList();

            foreach (var m in mantenimientosList)
            {
                if (equiposDict.TryGetValue(m.EquipoId, out var equipo))
                {
                    m.EquipoNombre = equipo.Nombre;

                    if (empresasDict.TryGetValue(equipo.EmpresaId, out var nombreEmpresa))
                        m.EmpresaNombre = nombreEmpresa;
                    else
                        m.EmpresaNombre = "Desconocida";
                }
                else
                {
                    m.EquipoNombre = "Desconocido";
                    m.EmpresaNombre = "Desconocida";
                }
            }

            // 5. Asigna la lista al DataGridView
            MantenimientoDataGridView.DataSource = null;
            MantenimientoDataGridView.DataSource = mantenimientosList;

            // 6. Oculta columnas de IDs y tipo de equipo
            if (MantenimientoDataGridView.Columns["MantenimientoId"] != null)
                MantenimientoDataGridView.Columns["MantenimientoId"].Visible = false;
            if (MantenimientoDataGridView.Columns["EquipoId"] != null)
                MantenimientoDataGridView.Columns["EquipoId"].Visible = false;
            if (MantenimientoDataGridView.Columns["EquipoTipo"] != null)
                MantenimientoDataGridView.Columns["EquipoTipo"].Visible = false;
            if (MantenimientoDataGridView.Columns["EmpresaId"] != null)
                MantenimientoDataGridView.Columns["EmpresaId"].Visible = false;

            // 7. Opcional: Cambia encabezados para mayor claridad
            if (MantenimientoDataGridView.Columns["EquipoNombre"] != null)
                MantenimientoDataGridView.Columns["EquipoNombre"].HeaderText = "Equipo";
            if (MantenimientoDataGridView.Columns["EmpresaNombre"] != null)
                MantenimientoDataGridView.Columns["EmpresaNombre"].HeaderText = "Empresa";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar mantenimientos: {ex.Message}");
        }
    }


    #endregion




    // ============================
    #region MANTENIMIENTO - CRUD Y EVENTOS
    // ============================

    private void MantenimientoDataGridView_SelectionChanged(object sender, EventArgs e)
    {
        if (MantenimientoDataGridView.SelectedRows.Count > 0)
        {
            var row = MantenimientoDataGridView.SelectedRows[0].DataBoundItem as MantenimientoResponseDTO;
            if (row != null)
            {
                mantenimientoSeleccionado = row;
                // Cargar datos en los controles
                var equipo = equipos.FirstOrDefault(eq => eq.EquipoId == row.EquipoId);
                if (equipo != null)
                    EquipoComboBox.SelectedItem = equipo;
                FechaDateTimePicker.Value = row.Fecha;
                TipoMantenimientoComboBox.SelectedItem = row.Tipo;
                DescripcionMantenimientoTextBox.Text = row.Descripcion;
                EstadoMantenimientoComboBox.SelectedItem = row.Estado;
            }
        }
    }

    private async void CrearMantenimientoButton_Click(object sender, EventArgs e)
    {
        if (EquipoComboBox.SelectedItem is not EquipoResponseDTO equipoSeleccionadoCombo)
        {
            MessageBox.Show("Selecciona un equipo.");
            return;
        }

        var dto = new MantenimientoCreateDTO
        {
            EquipoId = equipoSeleccionadoCombo.EquipoId,
            Fecha = FechaDateTimePicker.Value,
            Tipo = TipoMantenimientoComboBox.SelectedItem?.ToString() ?? "Preventivo",
            Descripcion = DescripcionMantenimientoTextBox.Text,
            Estado = EstadoMantenimientoComboBox.SelectedItem?.ToString() ?? "Pendiente"
        };

        try
        {
            await _apiClient.Mantenimiento.CreateAsync(dto);
            await LoadMantenimientosAsync();
            MessageBox.Show("Mantenimiento creado correctamente.");
            LimpiarCamposMantenimiento();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al crear mantenimiento: {ex.Message}");
        }
    }

    private async void ActualizarMantenimientoButton_Click(object sender, EventArgs e)
    {
        if (mantenimientoSeleccionado == null)
        {
            MessageBox.Show("Selecciona un mantenimiento para actualizar.");
            return;
        }
        if (EquipoComboBox.SelectedItem is not EquipoResponseDTO equipoSeleccionadoCombo)
        {
            MessageBox.Show("Selecciona un equipo.");
            return;
        }

        var dto = new MantenimientoUpdateDTO
        {
            MantenimientoId = mantenimientoSeleccionado.MantenimientoId,
            EquipoId = equipoSeleccionadoCombo.EquipoId,
            Fecha = FechaDateTimePicker.Value,
            Tipo = TipoMantenimientoComboBox.SelectedItem?.ToString() ?? "Preventivo",
            Descripcion = DescripcionMantenimientoTextBox.Text,
            Estado = EstadoMantenimientoComboBox.SelectedItem?.ToString() ?? "Pendiente"
        };

        try
        {
            await _apiClient.Mantenimiento.UpdateAsync(dto.MantenimientoId, dto);
            await LoadMantenimientosAsync();
            MessageBox.Show("Mantenimiento actualizado correctamente.");
            LimpiarCamposMantenimiento();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al actualizar mantenimiento: {ex.Message}");
        }
    }

    private async void BorrarMantenimientoButton_Click(object sender, EventArgs e)
    {
        if (mantenimientoSeleccionado == null)
        {
            MessageBox.Show("Selecciona un mantenimiento para borrar.");
            return;
        }

        var confirm = MessageBox.Show("¿Seguro que deseas eliminar este mantenimiento?", "Confirmar", MessageBoxButtons.YesNo);
        if (confirm == DialogResult.Yes)
        {
            try
            {
                await _apiClient.Mantenimiento.DeleteAsync(mantenimientoSeleccionado.MantenimientoId);
                await LoadMantenimientosAsync();
                MessageBox.Show("Mantenimiento eliminado correctamente.");
                LimpiarCamposMantenimiento();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar mantenimiento: {ex.Message}");
            }
        }
    }

    private void LimpiarCamposMantenimiento()
    {
        if (EquipoComboBox.Items.Count > 0)
            EquipoComboBox.SelectedIndex = 0;
        FechaDateTimePicker.Value = DateTime.Now;
        TipoMantenimientoComboBox.SelectedIndex = 0;
        DescripcionMantenimientoTextBox.Text = "";
        EstadoMantenimientoComboBox.SelectedIndex = 0;
        mantenimientoSeleccionado = null;
    }

    #endregion


    // ============================
    #region USUARIOS - CONFIGURACIÓN Y CARGA DE DATOS
    // ============================

    private async Task CargarEmpresasUsuariosComboAsync()
    {
        try
        {
            var empresasList = await _apiClient.Empresa.GetAllAsync();
            empresasUsuarios = empresasList.ToList();
            EmpresaUsuarioComboBox.DataSource = empresasUsuarios;
            EmpresaUsuarioComboBox.DisplayMember = "Nombre";
            EmpresaUsuarioComboBox.ValueMember = "EmpresaId";
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error al cargar empresas para usuarios: " + ex.Message);
        }
    }

    private void CargarRolesCombo()
    {
        RolUsuarioComboBox.Items.Clear();
        RolUsuarioComboBox.Items.Add("Administrador");
        RolUsuarioComboBox.Items.Add("Técnico");
        RolUsuarioComboBox.Items.Add("Usuario");
        RolUsuarioComboBox.SelectedIndex = 0;
    }

    public async Task LoadUsuariosAsync()
    {
        try
        {
            // 1. Obtén empresas para mostrar el nombre
            var empresasDict = empresasUsuarios.ToDictionary(e => e.EmpresaId, e => e.Nombre);

            // 2. Obtén usuarios
            var usuarios = await _apiClient.Usuario.GetAllAsync();
            var usuariosList = usuarios.ToList();

            // 3. Rellena el nombre de la empresa en cada usuario (si tu DTO lo permite)
            foreach (var u in usuariosList)
            {
                if (empresasDict.TryGetValue(u.EmpresaId, out var nombreEmpresa))
                    u.EmpresaNombre = nombreEmpresa;
                else
                    u.EmpresaNombre = "Desconocida";
            }

            UsuariosDataGridView.DataSource = null;
            UsuariosDataGridView.DataSource = usuariosList;

            // 4. Oculta columnas de IDs y contraseña
            if (UsuariosDataGridView.Columns["UsuarioId"] != null)
                UsuariosDataGridView.Columns["UsuarioId"].Visible = false;
            if (UsuariosDataGridView.Columns["EmpresaId"] != null)
                UsuariosDataGridView.Columns["EmpresaId"].Visible = false;
            if (UsuariosDataGridView.Columns["Contraseña"] != null)
                UsuariosDataGridView.Columns["Contraseña"].Visible = false;

            // 5. Ajusta encabezado para EmpresaNombre
            if (UsuariosDataGridView.Columns["EmpresaNombre"] != null)
                UsuariosDataGridView.Columns["EmpresaNombre"].HeaderText = "Empresa";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar usuarios: {ex.Message}");
        }
    }

    private void ConfigurarUsuariosDataGridView()
    {
        UsuariosDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        UsuariosDataGridView.MultiSelect = false;
        UsuariosDataGridView.ReadOnly = true;
        UsuariosDataGridView.AllowUserToAddRows = false;
        UsuariosDataGridView.AllowUserToDeleteRows = false;
        UsuariosDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        UsuariosDataGridView.BackgroundColor = Color.White;
        UsuariosDataGridView.DefaultCellStyle.BackColor = Color.White;
        UsuariosDataGridView.DefaultCellStyle.ForeColor = Color.Black;
        UsuariosDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(35, 57, 93);
        UsuariosDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        UsuariosDataGridView.EnableHeadersVisualStyles = false;
    }


    #endregion

    // ============================
    #region USUARIOS - CRUD Y EVENTOS
    // ============================

    private void UsuariosDataGridView_SelectionChanged(object sender, EventArgs e)
    {
        if (UsuariosDataGridView.SelectedRows.Count > 0)
        {
            var row = UsuariosDataGridView.SelectedRows[0].DataBoundItem as UsuarioResponseDTO;
            if (row != null)
            {
                usuarioSeleccionado = row;
                NombreUsuarioTextBox.Text = row.Nombre;
                CorreoUsuarioTextBox.Text = row.Email;
                RolUsuarioComboBox.SelectedItem = row.Rol;
                // Contraseña NO se muestra por seguridad
                var empresa = empresasUsuarios.FirstOrDefault(emp => emp.EmpresaId == row.EmpresaId);
                if (empresa != null)
                    EmpresaUsuarioComboBox.SelectedItem = empresa;
            }
        }
    }

    private async void CrearUsuarioButton_Click(object sender, EventArgs e)
    {
        if (EmpresaUsuarioComboBox.SelectedItem is not EmpresaResponseDTO empresaSeleccionada)
        {
            MessageBox.Show("Selecciona una empresa.");
            return;
        }

        var dto = new UsuarioCreateDTO
        {
            Nombre = NombreUsuarioTextBox.Text,
            Email = CorreoUsuarioTextBox.Text,
            Rol = RolUsuarioComboBox.SelectedItem?.ToString() ?? "Usuario",
            Contraseña = ContraseñaUsuarioTextBox.Text,
            EmpresaId = empresaSeleccionada.EmpresaId
        };

        try
        {
            await _apiClient.Usuario.CreateAsync(dto);
            await LoadUsuariosAsync();
            MessageBox.Show("Usuario creado correctamente.");
            LimpiarCamposUsuario();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al crear usuario: {ex.Message}");
        }
    }

    private async void ActualizarUsuarioButton_Click(object sender, EventArgs e)
    {
        if (usuarioSeleccionado == null)
        {
            MessageBox.Show("Selecciona un usuario para actualizar.");
            return;
        }
        if (EmpresaUsuarioComboBox.SelectedItem is not EmpresaResponseDTO empresaSeleccionada)
        {
            MessageBox.Show("Selecciona una empresa.");
            return;
        }

        var dto = new UsuarioUpdateDTO
        {
            UsuarioId = usuarioSeleccionado.UsuarioId,
            Nombre = NombreUsuarioTextBox.Text,
            Email = CorreoUsuarioTextBox.Text,
            Rol = RolUsuarioComboBox.SelectedItem?.ToString() ?? "Usuario",
            Contraseña = ContraseñaUsuarioTextBox.Text, // Solo si quieres permitir cambiarla
            EmpresaId = empresaSeleccionada.EmpresaId
        };

        try
        {
            await _apiClient.Usuario.UpdateAsync(dto.UsuarioId, dto);
            await LoadUsuariosAsync();
            MessageBox.Show("Usuario actualizado correctamente.");
            LimpiarCamposUsuario();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al actualizar usuario: {ex.Message}");
        }
    }

    private async void BorrarUsuarioButton_Click(object sender, EventArgs e)
    {
        if (usuarioSeleccionado == null)
        {
            MessageBox.Show("Selecciona un usuario para borrar.");
            return;
        }

        var confirm = MessageBox.Show("¿Seguro que deseas eliminar este usuario?", "Confirmar", MessageBoxButtons.YesNo);
        if (confirm == DialogResult.Yes)
        {
            try
            {
                await _apiClient.Usuario.DeleteAsync(usuarioSeleccionado.UsuarioId);
                await LoadUsuariosAsync();
                MessageBox.Show("Usuario eliminado correctamente.");
                LimpiarCamposUsuario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar usuario: {ex.Message}");
            }
        }
    }

    private void LimpiarCamposUsuario()
    {
        NombreUsuarioTextBox.Text = "";
        CorreoUsuarioTextBox.Text = "";
        RolUsuarioComboBox.SelectedIndex = 0;
        ContraseñaUsuarioTextBox.Text = "";
        EmpresaUsuarioComboBox.SelectedIndex = 0;
        usuarioSeleccionado = null;
    }

    #endregion
}
