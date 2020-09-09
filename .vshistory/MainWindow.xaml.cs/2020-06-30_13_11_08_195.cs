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
            string consulta = "SELECT * FROM CLIENTE";  // Datos a consultar, muestra todos los clientesw por el campo seleccionado en Display
            string consulta2 = "SELECT * FROM CLIENTE WHERE POBLACION = 'MADRID' "; //Muestra los clientes que viven en Madrid
            string consulta3 = "SELECT * FROM Cliente INNER JOIN Pedido ON Cliente.id=Pedido.cCliente WHERE Cliente.poblacion='MADRID'"; // Muestra los clientes que han hecho pedido de madrid

            SqlDataAdapter miAdaptadorSQL = new SqlDataAdapter(consulta, miConexionSQL); //Introducimos al adaptador la consulta y la conexión

            using (miAdaptadorSQL)
            {
                DataTable clientesTabla = new DataTable();//Creamos una tabla de datos

                miAdaptadorSQL.Fill(clientesTabla); //Introducimos al adaptador los datos a mostrar

                ListaCliente.DisplayMemberPath = "nombre"; //Campo de la tabla a mostrar
                ListaCliente.SelectedValuePath = "Id"; 
                ListaCliente.ItemsSource = clientesTabla.DefaultView;
            }
        }

        private void ListaCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MuestraPedido();
        }

        private void MuestraPedido()
        {
            string consultaPedido = "SELECT * FROM PEDIDO P INNER JOIN CLIENTE C ON C.Id=P.cCliente" +
                "WHERE C.Id=@ClienteId"; //Es una consulta con parametros

            SqlCommand sqlComando = new SqlCommand(consultaPedido, miConexionSQL); //Introducimos al comando la consulta y la conexion
           
            SqlDataAdapter miAdaptadorSQL2 = new SqlDataAdapter(sqlComando); //Introducimos el comando al adaptador, la conexion ya ha sido agregada en el comando

            using (miAdaptadorSQL2)
            {
                sqlComando.Parameters.AddWithValue("@ClienteId", ListaCliente.SelectedValue);

                DataTable PedidosTabla = new DataTable();

                miAdaptadorSQL2.Fill(PedidosTabla);

                ListaPedido.DisplayMemberPath = "fechaPedido";
                ListaPedido.SelectedValuePath = "Id";
                ListaPedido.ItemsSource = PedidosTabla.DefaultView;
            }
        }
    }
}
