using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace PC02_SI55_Fabricio_Subilete_Paulini
{
    public partial class Form1 : Form
    {
        private SqlConnection cn;
        public Form1()
        {
            InitializeComponent();
            string connectionString = "Server=.;Database=DBINSTITUTO;Integrated Security = True; ";
            cn = new SqlConnection(connectionString);

        }

        private void MostrarDocentes()
        {
            try
            {
                cn.Open();
                SqlDataAdapter adaptador = new SqlDataAdapter("SELECT * FROM Docente", cn);
                DataTable tabla = new DataTable();
                adaptador.Fill(tabla);
                dgDocente.DataSource = tabla;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar docentes: " + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }

        private void MostrarCursos()
        {
            try
            {
                cn.Open();
                SqlDataAdapter adaptador = new SqlDataAdapter("SELECT * FROM Curso", cn);
                DataTable tabla = new DataTable();
                adaptador.Fill(tabla);
                dgCurso.DataSource = tabla;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mostrar docentes: " + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }

        private void MostrarDocentesXCurso()
        {
            try
            {
                cn.Open();
                string query = "SELECT * FROM DocentePorCurso";
                SqlCommand cmd = new SqlCommand(query, cn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgDocenteXCurso.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }


        private void btnAgregarDocente_Click(object sender, EventArgs e)
        {
            if (tbIDDocente.Text == "" || tbNombreDocente.Text == "" || tbApellido.Text == "" || tbDNI.Text == "" || tbEscuela.Text=="")
            {
                MessageBox.Show("Ingrese todos los campos");
                return;
            }
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_InsertarDocente", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id_docente", tbIDDocente.Text);
                cmd.Parameters.AddWithValue("@Nombre", tbNombreDocente.Text);
                cmd.Parameters.AddWithValue("@Apellido", tbApellido.Text);
                cmd.Parameters.AddWithValue("@DNI", tbDNI.Text);
                cmd.Parameters.AddWithValue("@Escuela", tbEscuela.Text);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            MostrarDocentes();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MostrarDocentes();
            MostrarCursos();
            MostrarDocentesXCurso();
        }

        private void btnLimpiarDocente_Click(object sender, EventArgs e)
        {
            tbIDDocente.Text = "";
            tbNombreDocente.Text = "";
            tbApellido.Text = "";
            tbDNI.Text = "";
            tbEscuela.Text = "";
        }

        private void btnEliminarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgDocente.SelectedRows.Count > 0)
                {
                    string codDoc = dgDocente.SelectedRows[0].Cells["Id_docente"].Value.ToString();
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("sp_EliminarDocente", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id_docente", codDoc);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Docente eliminado con éxito!");
                }
            }
            finally
            {
                cn.Close();
            }
            MostrarDocentes();
        }

        private void btnActualizarCliente_Click(object sender, EventArgs e)
        {
            if (dgDocente.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione una fila para actualizar.");
                return;
            }
            string iddoc = dgDocente.SelectedRows[0].Cells["Id_docente"].Value.ToString();
            string nombdoc = tbNombreDocente.Text;
            string apelldoc = tbApellido.Text;
            string DNI = tbDNI.Text;
            string escuela = tbEscuela.Text;
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_ActualizarDocente", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id_docente", iddoc);
                cmd.Parameters.AddWithValue("@Nombre", nombdoc);
                cmd.Parameters.AddWithValue("@Apellido", apelldoc);
                cmd.Parameters.AddWithValue("@DNI", DNI);
                cmd.Parameters.AddWithValue("@Escuela", escuela);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Docente actualizado con éxito!");
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar el docente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            MostrarDocentes();
        }

        private void btnAgregarCurso_Click(object sender, EventArgs e)
        {
            if (tbIDCurso.Text == "" || tbIDNombreCurso.Text == "" || tbCreditos.Text == "")
            {
                MessageBox.Show("Ingrese todos los campos");
                return;
            }
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_InsertarCurso", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id_curso", tbIDCurso.Text);
                cmd.Parameters.AddWithValue("@Nombrecurso", tbIDNombreCurso.Text);
                cmd.Parameters.AddWithValue("@Creditos", tbCreditos.Text);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            MostrarCursos();
        }

        private void btnLimpiarCurso_Click(object sender, EventArgs e)
        {
            tbIDCurso.Text = "";
            tbIDNombreCurso.Text = "";
            tbCreditos.Text = "";
        }

        private void btnActualizarCurso_Click(object sender, EventArgs e)
        {
            if (dgCurso.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione una fila para actualizar.");
                return;
            }
            string idcur = dgCurso.SelectedRows[0].Cells["Id_curso"].Value.ToString();
            string nomcur = tbIDNombreCurso.Text;
            int credit = Convert.ToInt32(tbCreditos.Text);

            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_ActualizarCurso", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id_curso", idcur);
                cmd.Parameters.AddWithValue("@Nombrecurso", nomcur);
                cmd.Parameters.AddWithValue("@Creditos", credit);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Curso actualizado con éxito!");
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar el curso.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            MostrarCursos();
        }

        private void btnEliminarCurso_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgCurso.SelectedRows.Count > 0)
                {
                    string codCurso = dgCurso.SelectedRows[0].Cells["Id_curso"].Value.ToString();
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("sp_EliminarCurso", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id_curso", codCurso);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Curso eliminado con éxito!");
                }
            }
            finally
            {
                cn.Close();
            }
            MostrarCursos();
        }

        private void btnAgregarDocenteXCurso_Click(object sender, EventArgs e)
        {
            if (tbIDDocenteXCurso.Text=="" || tbIDCursoXCurso.Text==""||tbSemestre.Text=="")
            {
                MessageBox.Show("Ingrese todos los campos");
                return;
            }
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_InsertarDocentePorCurso", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id_DOCXCURSO", int.Parse(tbIDDXC.Text));
                cmd.Parameters.AddWithValue("@Id_docente", int.Parse(tbIDDocenteXCurso.Text));
                cmd.Parameters.AddWithValue("@Id_curso", int.Parse(tbIDCursoXCurso.Text));
                cmd.Parameters.AddWithValue("@Semestre", tbSemestre.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Agregado con éxito!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            MostrarDocentesXCurso();

        }

        private void btnLimpiarDocenteXCurso_Click(object sender, EventArgs e)
        {
            tbIDDXC.Text = "";
            tbIDDocenteXCurso.Text = "";
            tbIDCursoXCurso.Text = "";
            tbSemestre.Text = "";
        }

        private void btnEliminarDocenteXCurso_Click(object sender, EventArgs e)
        {
            if (tbIDDXC.Text == "" || tbIDDocenteXCurso.Text == "")
            {
                MessageBox.Show("Ingrese el ID y el ID Docente");
                return;
            }
            try
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_EliminarDocentePorCurso", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id_DOCXCURSO", int.Parse(tbIDDXC.Text));
                cmd.Parameters.AddWithValue("@Id_docente", int.Parse(tbIDDocenteXCurso.Text));
                cmd.ExecuteNonQuery();
                MessageBox.Show("Eliminado con éxito!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                cn.Close();
            }
            MostrarDocentesXCurso();
        }
    }
    
}
