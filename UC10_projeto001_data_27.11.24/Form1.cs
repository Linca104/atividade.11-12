using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;



namespace UC10_projeto001_data_27._11._24

{
    public partial class Form1 : Form
    {
        private MySqlConnection Conexao;


        private string data_source = "datasource=localhost;username=root;password='';database=db_agenda";

        private int? id_contato_sel = null;

        public Form1()
        {
            InitializeComponent();

            lvDados.View = View.Details;


            lvDados.LabelEdit = true;
            lvDados.AllowColumnReorder = true;
            lvDados.FullRowSelect = true;
            lvDados.GridLines = true;
            lvDados.Columns.Add("ID", 30, HorizontalAlignment.Left);
            lvDados.Columns.Add("Nome", 150, HorizontalAlignment.Left);
            lvDados.Columns.Add("E-mail", 150, HorizontalAlignment.Left);
            lvDados.Columns.Add("Telefone", 150, HorizontalAlignment.Left);

        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {

                Conexao = new MySqlConnection(data_source);

                MySqlCommand cmd = new MySqlCommand();


                cmd.Connection = Conexao;


                Conexao.Open();

                cmd.Parameters.AddWithValue("@NOME", txtNome.Text);
                cmd.Parameters.AddWithValue("@EMAIL", txtEmail.Text);
                cmd.Parameters.AddWithValue("@TELEFONE", txtTelefone.Text);
                cmd.Parameters.AddWithValue("@ID", id_contato_sel);

                if (id_contato_sel != null)
                {

                    cmd.CommandText = "INSERT INTO contato(nome, email, telefone) VALUES (@NOME, @EMAIL, @TELEFONE)";

                    cmd.Prepare();

                    cmd.ExecuteNonQuery();
                }

                else
                {

                    cmd.CommandText = "INSERT INTO contato(nome, email, telefone) VALUES (@NOME, @EMAIL, @TELEFONE)";
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Contato Atualizado Com Sucesso!!");
                }
                string sql = "INSERT INTO contato (nome,email,telefone) " +
                             "VALUES('" + txtNome.Text + "','" + txtEmail.Text + "','" + txtTelefone.Text + "')";


                MessageBox.Show(sql);


                MySqlCommand comando = new MySqlCommand(sql, Conexao);


                Conexao.Open();
                comando.ExecuteReader();

                MessageBox.Show("Deu certo!!!");

            }
            catch (MySqlException ex)
            {


                MessageBox.Show("Erro " + ex.Number + " ocorreu: " + ex.Message,
                                "Erro ",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }


            finally
            {
                Conexao.Close();
            }

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string q = "'%" + txtBuscar.Text + "%'";


                MessageBox.Show(q);


                Conexao = new MySqlConnection(data_source);

                string sql = "SELECT * " +
                               "FROM contato " +
                              "WHERE nome LIKE " + q + "OR email LIKE" + q;

                Conexao.Open();


                MessageBox.Show(sql);

                MySqlCommand comando = new MySqlCommand(sql, Conexao);
                MySqlDataReader reader = comando.ExecuteReader();

                lvDados.Items.Clear();

                while (reader.Read())
                {

                    string[] row =
                    {

                        reader.GetValue(0).ToString(),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3)
                        };

                    var linha_listview = new ListViewItem(row);


                    lvDados.Items.Add(linha_listview);
                }
                MessageBox.Show("Deu certo de novo!!!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                Conexao.Close();
            }
        }


        private void lvDados_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {

            ListView.SelectedListViewItemCollection items_sel = lvDados.SelectedItems;

            foreach (ListViewItem item in items_sel)
            {

                id_contato_sel = Convert.ToInt32(item.SubItems[0].Text);
                txtNome.Text = item.SubItems[1].Text;
                txtEmail.Text = item.SubItems[2].Text;
                txtTelefone.Text = item.SubItems[3].Text;
            }
        }

        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult CONF = MessageBox.Show("Tem certeza que deseja excluir?",
                                                     "opa, tem certeza?",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Warning);

                if (CONF == DialogResult.Yes)
                {


                    Conexao = new MySqlConnection(data_source);
                    Conexao.Open();

                    MySqlCommand cmd = new MySqlCommand();

                    cmd.Connection = Conexao;

                    cmd.Parameters.AddWithValue("@ID", id_contato_sel);

                    cmd.CommandText = "DELETE FROM contato WHERE id=@id ";

                    cmd.Prepare();

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Contato Excluido com",
                                    "Sucesso!",
                                     MessageBoxButtons.OK,
                                     MessageBoxIcon.Information);
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show("ERRO" + ex.Number + "ocorreu:" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Conexao.Close();
            }
        }
    }
}
