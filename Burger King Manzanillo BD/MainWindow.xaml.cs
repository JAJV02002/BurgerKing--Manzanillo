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
using System.Data.OleDb;//Agregamos libreria OleDB
using System.Data; //Agregamos System.Data


namespace Burger_King_Manzanillo_BD
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OleDbConnection con; //Agregamos la conexión
        DataTable dt;   //Agregamos la tabla
        public MainWindow()
        {
            InitializeComponent();
            //Conectamos la base de datos a nuestro archivo Access
            con = new OleDbConnection();
            con.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\BurgerKing-Manzanillo-BD.mdb";
            MostrarDatos();
        }

        //Mostrar los datos en la tabla
        private void MostrarDatos()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from Pedidos_Bimbo";
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            gvDatos.ItemsSource = dt.AsDataView();

            if (dt.Rows.Count > 0)
            {
                lbContenido.Visibility = System.Windows.Visibility.Hidden;
                gvDatos.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                lbContenido.Visibility = System.Windows.Visibility.Visible;
                gvDatos.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        //Limpiamos todos los campos
        private void LimpiaTodo()
        {
            txtNumPed.Text = "";
            txtPanWhp.Text = "";
            txtPanJr.Text = "";
            txtPanMedias.Text = "";
            txtFecha.Text = "";
            btnNuevo.Content = "Nuevo";
            txtPanWhp.IsEnabled = true;
        }
        
        private void BtnNuevo_Click_1(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;

            if (txtNumPed.Text != "")
            {
                if (txtNumPed.IsEnabled == true)
                {
                    if (txtPanJr.Text != "Ingresa la cantidad de panes")
                    {
                        cmd.CommandText = "insert into Pedidos_Bimbo(Numero_de_pedido,Pan_Whopper,Pan_Whopper_Jr,Pan_Medias_Lunas,Fecha_de_pedido) " +
                            "Values(" + txtNumPed.Text + ",'" + txtPanWhp.Text + "','" + txtPanJr.Text + "','" + txtPanMedias.Text + "','" + txtFecha.Text + "')";
                        cmd.ExecuteNonQuery();
                        MostrarDatos();
                        MessageBox.Show("El pedido se registró correctamente...");
                        LimpiaTodo();

                    }
                    else
                    {
                        MessageBox.Show("Por favor llena los datos....");
                    }
                }
                else
                {
                    cmd.CommandText = "update Pedidos_Bimbo set Pan_Whopper='" + txtPanWhp.Text + "',Pan_Whopper_Jr='" + txtPanJr.Text + "',Pan_Medias_Lunas=" + txtPanMedias.Text
                        + ",Fecha_de_pedido='" + txtFecha.Text + "' where Numero_de_Pedido=" + txtNumPed.Text;
                    cmd.ExecuteNonQuery();
                    MostrarDatos();
                    MessageBox.Show("Registro actualizado...");
                    LimpiaTodo();
                }
            }
            else
            {
                MessageBox.Show("Por favor ingresa el numero de pedido.......");
            }

        }
        
        private void BtnEditar_Click_1(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                txtNumPed.Text = row["Numero_de_Pedido"].ToString();
                txtPanWhp.Text = row["Pan_Whopper"].ToString();
                txtPanJr.Text = row["Pan_Whopper_Jr"].ToString();
                txtPanMedias.Text = row["Pan_Medias_Lunas"].ToString();
                txtFecha.Text = row["Fecha_de_pedido"].ToString();
                txtNumPed.IsEnabled = false;
                btnNuevo.Content = "Actualizar";
            }
            else
            {
                MessageBox.Show("Favor de seleccionar el pedido...");
            }

        }

        private void BtnBorrar_Click_1(object sender, RoutedEventArgs e)
        {
            if (gvDatos.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)gvDatos.SelectedItems[0];
                OleDbCommand cmd = new OleDbCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                cmd.Connection = con;
                cmd.CommandText = "Delete from Pedidos_Bimbo where Numero_de_Pedido=" +
               row["Numero_de_Pedido"].ToString();
                cmd.ExecuteNonQuery();
                MostrarDatos();
                MessageBox.Show("Pedido eliminado correctamente...");
                LimpiaTodo();
            }
        }

        private void BtnCancelar_Click_1(object sender, RoutedEventArgs e)
        {
            LimpiaTodo();
        }

        private void BtnSalir_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }



    }
}
