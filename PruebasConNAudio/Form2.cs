using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PruebasConNAudio
{
    public partial class Form2 : Form
    {

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void CargarDatos()
        {
            using (var db = new AppDbContext())
            {
                var nombres = db.blackList.ToList();  // Obtener los datos de la tabla
                dataGridView1.DataSource = nombres;   // Vincular los datos al DataGridView
            }

            // Agregar columna de botón para eliminar
            if (!dataGridView1.Columns.Contains("btnEliminar"))
            {
                DataGridViewButtonColumn btnEliminar = new DataGridViewButtonColumn();
                btnEliminar.Name = "btnEliminar";
                btnEliminar.HeaderText = "Eliminar";
                btnEliminar.Text = "X";
                btnEliminar.UseColumnTextForButtonValue = true;
                dataGridView1.Columns.Add(btnEliminar);
            }

            // Manejar el evento de eliminar
            dataGridView1.CellClick += dataGridView1_CellClick;
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["btnEliminar"].Index && e.RowIndex >= 0)
            {
                // Obtener el Id del nombre que se va a eliminar
                int id = (int)dataGridView1.Rows[e.RowIndex].Cells["Id"].Value;

                // Eliminar el nombre de la base de datos
                using (var db = new AppDbContext())
                {
                    var nombre = db.blackList.FirstOrDefault(n => n.Id == id);
                    if (nombre != null)
                    {
                        db.blackList.Remove(nombre);
                        db.SaveChanges();
                        MessageBox.Show("Nombre eliminado correctamente.");

                        // Recargar los datos
                        CargarDatos();
                    }
                }
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Obtener el Id del nombre editado
            int id = (int)dataGridView1.Rows[e.RowIndex].Cells["Id"].Value;
            string nuevoNombre = dataGridView1.Rows[e.RowIndex].Cells["windowTitle"].Value.ToString();

            // Actualizar el nombre en la base de datos
            using (var db = new AppDbContext())
            {
                var nombre = db.blackList.FirstOrDefault(n => n.Id == id);
                if (nombre != null)
                {
                    nombre.windowTitle = nuevoNombre;
                    db.SaveChanges();
                    MessageBox.Show("Nombre actualizado correctamente.");
                }
            }
        }
    }
}
