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
    public partial class Form1: Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        public class user
        {
            public string name { get; set; }
            public string pass { get; set; }
            public string role { get; set; }
        }

        //log in button
        private void loginButton_Click(object sender, EventArgs e)
        {
            string inputName = nameInput.Text;
            string inputPass = pwInput.Text;

            // checks for a match
            var m = users.FirstOrDefault(u => u.name == inputName && u.pass == inputPass);

            // if not null or if found matched
            if (m != null)
            {
                MessageBox.Show($"Welcome, [{m.role}] {m.name}!");

                Console.WriteLine("Passed Info: " + m.role);

                // goes to admin panel if admin
                if (m.role == "Admin")
                {
                    Form f = new AdminPanel(this);
                    f.Show();
                    this.Hide();
                }
                else // goes to main form
                {
                    Form f = new main(this, m.role);
                    f.Show();
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("INVALID USERNAME OR PASSWORD");
            }
        }

        // initialize
        public List<user> users { get; private set; } = new List<user>();
        private void Form1_Load(object sender, EventArgs e)
        {
            users.Add(new user { name = "a", pass = "a", role = "Admin" });
            users.Add(new user { name = "eric", pass = "ilovebbc", role = "Student" });
            users.Add(new user { name = "b", pass = "b", role = "Teacher" });
        }
    }
}
