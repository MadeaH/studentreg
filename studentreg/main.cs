using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static studentreg.main;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ComboBox = System.Windows.Forms.ComboBox;
using TextBox = System.Windows.Forms.TextBox;

namespace studentreg
{
    public partial class main: Form
    {
        private Form1 _loginF;
        private string _role;
        private string selected_Student;

        // initializes the code, gets the code ready
        public main(Form1 f, string role)
        {
            InitializeComponent();

            _loginF = f;
            _role = role;

            tabControl1.Dock = DockStyle.Fill;


            if (role == "Student")
            {
                regPage.Enabled = false;
                attPage.Enabled = false;
                recordPage.Enabled = false;

                std_list_datagrid.ReadOnly = true;
            }
            else
            {
                regPage.Enabled = true;
                attPage.Enabled = true;
                recordPage.Enabled = true;

                std_list_datagrid.ReadOnly = false;

            }

        }

        public class studentInfo
        {
            public string std_fn { get; set; }
            public string std_mn { get; set; }
            public string std_ln { get; set; }
            public string std_fullname { get; set; }

            public string std_sex { get; set; }
            public string std_bdate { get; set; }
            public string std_civil { get; set; }

            public string std_nationality { get; set; }
            public string std_religion { get; set; }

            public string std_contact { get; set; }
            public string std_email { get; set; }

            public string std_address { get; set; }

            public string std_program { get; set; }
            public string std_year { get; set; }
            public string std_id { get; set; }
            public string std_transferee { get; set; }
            public string std_reports { get; set; }

            public string std_g_fn { get; set; }
            public string std_g_mn { get; set; }
            public string std_g_ln { get; set; }
            public string std_g_contact { get; set; }

        }

        // class/function to manage list
        public class StudentManager
        {
            private List<studentInfo> students = new List<studentInfo>();

            public void AddStudent(
                string fn, string mn, string ln, string fullname,
                string sex, string bdate, string civil,
                string nationality, string religion,
                string contact, string email,
                string address, string program, string year, string id, string transferee,
                string gfn, string gmn, string gln, string gcontact)
            {
                studentInfo student = new studentInfo
                {
                    std_fn = fn,
                    std_mn = mn,
                    std_ln = ln,
                    std_fullname = fullname,
                    std_sex = sex,
                    std_bdate = bdate,
                    std_civil = civil,
                    std_nationality = nationality,
                    std_religion = religion,
                    std_contact = contact,
                    std_email = email,
                    std_address = address,
                    std_program = program,
                    std_year = year,
                    std_id = id,
                    std_transferee = transferee,
                    std_g_fn = gfn,
                    std_g_mn = gmn,
                    std_g_ln = gln,
                    std_g_contact = gcontact
                };

                students.Add(student);
            }

            public List<studentInfo> GetAllStudents()
            {
                return students;
            }
        }
        private StudentManager std_mng = new StudentManager();

        // checks for empty spaces in the registration tab page
        private bool CheckForEmptySpace(TabPage tabPage)
        {
            bool isThereEmpty = false;

            foreach (Control control in tabPage.Controls)
            {
                // Check if the control is a TextBox
                if (control is TextBox textBox)
                {
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        isThereEmpty = true;
                    }
                }
                else if (control is ComboBox comboBox)
                {
                    if (comboBox.SelectedIndex == -1)
                    {
                        isThereEmpty = true;
                    }
                }
                else if (control is DateTimePicker dateTimePicker)
                {
                    if (dateTimePicker.Value.Date == DateTimePicker.MinimumDateTime.Date)
                    {
                        isThereEmpty = true;
                    }
                }
            }

            if (isThereEmpty == true)
            {
                return true;
            }

            return false;
        }

        // clears everything in that tabpage
        private void EmptyRegisterPage(TabPage tabPage)
        {
            foreach(Control ctrl in tabPage.Controls)
            {
                if (ctrl is TextBox tb)
                {
                    tb.Clear();
                } else if (ctrl is ComboBox cb)
                {
                    cb.SelectedIndex = -1;
                } else if (ctrl is DateTimePicker dtp)
                {
                    dtp.Value = DateTime.Now;
                }
            }
        }

        // adds it to every datagridview/tablelists
        private void addToLists()
        {
            string full_name = $"{ln_input.Text.ToUpper()}, {fn_input.Text.ToUpper()} {mn_input.Text.ToUpper()}";
            string gfull_name = $"{gln_input.Text.ToUpper()}, {gfn_input.Text.ToUpper()} {gmn_input.Text.ToUpper()}";
            string combined_pys = $"{program_input.Text} {year_input.Text}";

            std_list_datagrid.Rows.Add(
                id_input.Text, 
                ln_input.Text,
                mn_input.Text, 
                fn_input.Text,
                program_input.Text, 
                year_input.Text,
                gender_input.Text, 
                bdate_input.Value.ToShortDateString(), 
                address_input.Text,
                nationality_input.Text, 
                civilstatus_input.Text, 
                contactnum_input.Text,
                transferee_input.Checked.ToString(),
                religion_input.Text,
                gln_input.Text, 
                gmn_input.Text, 
                gfn_input.Text, 
                gnnumber_input.Text
                );

            att_grid.Rows.Add(id_input.Text, full_name, combined_pys);
        }

        // real-time calculation of average
        private double calculateAverage(int index)
        {
            double total = 0;
            int count = 0;

            foreach (DataGridViewRow row in records_dg.Rows)
            {
                if (row.Cells[index].Value != null && double.TryParse(row.Cells[index].Value.ToString(), out double value))
                {
                    total += value;
                    count++;
                }
            }

            return count > 0 ? total / count : 0;
        }

        // register button, gets every text in every label and puts it in the manager
        private void regBtn_Click(object sender, EventArgs e)
        {

            bool hasEmpty = CheckForEmptySpace(regPage);
            if (hasEmpty == true)
            {
                MessageBox.Show("There are empty or unselected controls in the form.");
            }
            else
            {
                MessageBox.Show("All controls are filled correctly.");

                string full_name = $"{ln_input.Text.ToUpper()}, {fn_input.Text.ToUpper()} {mn_input.Text.ToUpper()}";

                std_mng.AddStudent(
                fn: fn_input.Text,
                mn: mn_input.Text,
                ln: ln_input.Text,
                fullname: full_name,
                sex: gender_input.Text,
                bdate: bdate_input.Value.ToShortDateString(),
                civil: civilstatus_input.Text,
                nationality: nationality_input.Text,
                religion: religion_input.Text,
                contact: contactnum_input.Text,
                email: email_input.Text,
                address: address_input.Text,
                program: program_input.Text,
                year: year_input.Text,
                id: id_input.Text,
                transferee: transferee_input.Checked.ToString(),
                gfn: gfn_input.Text,
                gmn: gmn_input.Text,
                gln: gln_input.Text,
                gcontact: gnnumber_input.Text
            );
                addToLists();
            }
        }

        // clear button
        private void clearAll_btn_Click(object sender, EventArgs e)
        {
            att_grid.Rows.Clear();
        }

        // add date to attendance
        private void datecol_btn_Click(object sender, EventArgs e)
        {
            string selectedDate = attdg_dtp.Value.ToShortDateString();
            att_grid.Columns.Add(selectedDate, selectedDate);
        }

        // removes date at attendance
        private void removedatecol_btn_Click(object sender, EventArgs e)
        {
            string selectedDate = attdg_dtp.Value.ToShortDateString();
            if (att_grid.Columns.Contains(selectedDate))
            {
                att_grid.Columns.Remove(selectedDate);
                MessageBox.Show($"Column '{selectedDate}' has been removed.");
            }
            else
            {
                MessageBox.Show($"Column '{selectedDate}' does not exist.");
            }

        }

        // search bar
        private void stdlist_search_btn_Click(object sender, EventArgs e)
        {
            string searched = searchbox_stdlist.Text.ToLower();
            std_list_datagrid.Rows.Clear();

            var students = std_mng.GetAllStudents();
            foreach (var std in students)
            {
                if(std.std_id.ToLower().Contains(searched) || std.std_fullname.ToLower().Contains(searched))
                {
                    string full_name = $"{ln_input.Text.ToUpper()}, {fn_input.Text.ToUpper()} {mn_input.Text.ToUpper()}";
                    string gfull_name = $"{gln_input.Text.ToUpper()}, {gfn_input.Text.ToUpper()} {gmn_input.Text.ToUpper()}";
                    string combined_pys = $"{program_input.Text} {year_input.Text}";

                    std_list_datagrid.Rows.Add(std.std_id, std.std_ln, std.std_mn, std.std_fn,
                        std.std_program, std.std_year,
                        std.std_sex, std.std_bdate, std.std_address,
                        std.std_nationality, std.std_civil, std.std_contact,
                        std.std_transferee,
                        std.std_religion,
                        std.std_g_ln, std.std_g_mn, std.std_g_fn,
                        std.std_g_contact);
                }
            }
        }

        // clear button
        private void clearBtn_Click(object sender, EventArgs e)
        {
            EmptyRegisterPage(regPage);
        }

        // checks if there's any changes inside the student list
        private void std_list_datagrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var student = std_mng.GetAllStudents()[e.RowIndex];

                switch (e.ColumnIndex)
                {
                    case 0:
                        student.std_id = std_list_datagrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        break;
                    case 1:
                        student.std_ln = std_list_datagrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        student.std_fullname = $"{std_list_datagrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()}, {student.std_mn} {student.std_fn}";
                        break;
                    case 2:
                        student.std_mn = std_list_datagrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        student.std_fullname = $"{student.std_ln}, {std_list_datagrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()} {student.std_fn}";
                        break;
                    case 3:
                        student.std_fn = std_list_datagrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        student.std_fullname = $"{student.std_ln}, {student.std_mn} {student.std_mn = std_list_datagrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()}";
                        break;
                    case 4:
                        student.std_program = std_list_datagrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        break;
                    case 5:
                        student.std_year = std_list_datagrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        break;
                    case 6:
                        student.std_sex = std_list_datagrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        break;
                    case 7:
                        student.std_bdate = std_list_datagrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        break;
                    case 8:
                        student.std_address = std_list_datagrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        break;
                    case 9:
                        student.std_nationality = std_list_datagrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        break;
                    case 10:
                        student.std_civil = std_list_datagrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        break;
                    case 11:
                        student.std_contact = std_list_datagrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        break;
                    case 12:
                        student.std_transferee = std_list_datagrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        break;
                    case 13:
                        student.std_religion = std_list_datagrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        break;
                    case 14:
                        student.std_g_ln = std_list_datagrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        break;
                    case 15:
                        student.std_g_mn = std_list_datagrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        break;
                    case 16:
                        student.std_g_fn = std_list_datagrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        break;
                    case 17:
                        student.std_g_contact = std_list_datagrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        break;
                }
            }
        }

        // attendance search bar
        private void att_search_btn_Click(object sender, EventArgs e)
        {
            string searched = atd_searchbar.Text.ToLower();
            att_grid.Rows.Clear();

            var students = std_mng.GetAllStudents();
            foreach (var std in students)
            {
                if (std.std_id.ToLower().Contains(searched) || std.std_fullname.ToLower().Contains(searched))
                {
                    string combined_pys = $"{program_input.Text} {year_input.Text}";

                    att_grid.Rows.Add(std.std_id, std.std_fullname, combined_pys);
                }
            }
        }

        // if the users clicks on the student list, it selects that student
        private void std_list_datagrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var getID = std_list_datagrid.Rows[e.RowIndex].Cells[0].Value;
                var getLN = std_list_datagrid.Rows[e.RowIndex].Cells[1].Value;
                var getMN = std_list_datagrid.Rows[e.RowIndex].Cells[2].Value;
                var getFN = std_list_datagrid.Rows[e.RowIndex].Cells[3].Value;

                string toLabel = $"SELECTED STUDENT: {getID.ToString()} - {getLN.ToString()}, {getMN.ToString()} {getFN.ToString()}";
                selectedLabel.Text = toLabel;
                records_selectedLabel.Text = toLabel;

                selected_Student = getID.ToString();
            }
        }

        // updates average
        private void records_dg_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int index = e.ColumnIndex;

                double avg = calculateAverage(index);

                avg_label.Text = $"Average: {avg}";
            }
        }

        // signout
        private void signOutBtn_Click(object sender, EventArgs e)
        {
            Form f = new Form1();
            f.Show();
            this.Hide();
        }

    }
}
