using System.Collections.ObjectModel;
using WaiteringSystem.Data;

namespace WaiteringSystem.Business
{
    public class EmployeeController
    {
        #region Data members
        // 2.2 Reference to the database layer
        private EmployeeDB employeeDB;
        // 2.3 Collection of employees
        private Collection<Employee> employees;
        #endregion

        #region Properties
        public Collection<Employee> AllEmployees
        {
            get { return employees; }
        }
        #endregion

        #region Constructor
        // 2.4
        public EmployeeController()
        {
            employeeDB = new EmployeeDB();
            employees = employeeDB.AllEmployees;
        }
        #endregion

        #region Database Communication
        // 2.5.1
        public void DataMaintenance(Employee anEmp)
        {
            employeeDB.DataSetChange(anEmp);
            employees.Add(anEmp);
        }

        // 2.5.2 
        public bool FinalizeChanges(Employee employee)
        {
            return employeeDB.UpdateDataSource(employee);
        }
        #endregion
    }
}