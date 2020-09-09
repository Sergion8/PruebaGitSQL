using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace BaseDeDatos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection miConexionSQL;
        public MainWindow()
        {
            InitializeComponent();

            string miConexion = ConfigurationManager.ConnectionStrings["BaseDeDatos.Properties.Settings.CursoPildorasConnectionString"].ConnectionString;

            miConexionSQL = new SqlConnection(miConexion);

            MuestraClientes();
        }

        private void MuestraClientes()
        {
            string consulta = "SELECT * FROM CLIENTE";  // Datos a consultar, muestra el nombre de los clientes(campo clave)
            string consulta2 = "SELECT * FROM CLIENTE WHERE POBLACION = 'MADRID' ";

            SqlDataAdapter miAdaptadorSQL = new SqlDataAdapter(consulta, miConexionSQL);

            using (miAdaptadorSQL)
            {
                DataTable clientesTabla = new DataTable();

                miAdaptadorSQL.Fill(clientesTabla);

                ListaCliente.DisplayMemberPath = "direccion";
                ListaCliente.SelectedValuePath = "Id";
                ListaCliente.ItemsSource = clientesTabla.DefaultView;
            }
        }

    }
}
