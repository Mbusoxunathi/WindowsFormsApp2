using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaiteringSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var ctrl = new Business.EmployeeController();
            var emp = new Business.Employee(Business.Role.RoleType.Waiter);
            emp.ID = "T001"; emp.EmployeeID = "E001"; emp.Name = "Test"; emp.Telephone = "0820000000";
            ((Business.Waiter)emp.role).getRate = 150;
            ctrl.DataMaintenance(emp);
            MessageBox.Show(ctrl.FinalizeChanges(emp) ? "Saved" : "Failed");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
