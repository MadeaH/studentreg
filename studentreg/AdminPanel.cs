using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace studentreg
{
    public partial class AdminPanel: Form
    {

        // initialize
        public AdminPanel(Form1 f)
        {
            InitializeComponent();

            dataGridView1.Rows.Clear();

            foreach (var user in f.users)
            {
                dataGridView1.Rows.Add(user.name, user.pass, user.role);
            }
        }

        private void dataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {

        }

        // sign out
        private void signOutBtn_Click(object sender, EventArgs e)
        {
            Form f = new Form1();
            f.Show();
            this.Hide();
        }
    }
}
