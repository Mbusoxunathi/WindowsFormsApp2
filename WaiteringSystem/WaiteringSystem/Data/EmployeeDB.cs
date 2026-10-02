using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using WaiteringSystem.Business;

namespace WaiteringSystem.Data
{
    public class EmployeeDB : DB   // 1.1 EmployeeDB inherits from DB
    {
        #region Data members
        // 1.2 One table name and SELECT statement per role type
        private string table1 = "HeadWaiter";
        private string sqlLocal1 = "SELECT * FROM HeadWaiter";
        private string table2 = "Waiter";
        private string sqlLocal2 = "SELECT * FROM Waiter";
        private string table3 = "Runner";
        private string sqlLocal3 = "SELECT * FROM Runner";

        // 1.3 Collection of employees declaration
        private Collection<Employee> employees;
        #endregion

        #region Property Method: Collection
        // 1.4 Return the collection of all employees
        public Collection<Employee> AllEmployees
        {
            get { return employees; }
        }
        #endregion

        #region Constructor
        // 1.5, 1.7
        public EmployeeDB() : base()
        {
            employees = new Collection<Employee>();

            FillDataSet(sqlLocal1, table1);
            Add2Collection(table1);
            FillDataSet(sqlLocal2, table2);
            Add2Collection(table2);
            FillDataSet(sqlLocal3, table3);
            Add2Collection(table3);
        }
        #endregion

        #region Utility Methods
        // 1.6.1 return the current dataset
        public DataSet GetDataSet()
        {
            return dsMain;
        }

        // 1.6.2 Build the collection from one table (HeadWaiter, Waiter or Runner)
        private void Add2Collection(string table)
        {
            DataRow myRow = null;
            Employee anEmp;
            HeadWaiter headW;
            Waiter waiter;
            Runner runner;
            Role.RoleType roleValue = Role.RoleType.NoRole;

            switch (table)
            {
                case "HeadWaiter":
                    roleValue = Role.RoleType.Headwaiter;
                    break;
                case "Waiter":
                    roleValue = Role.RoleType.Waiter;
                    break;
                case "Runner":
                    roleValue = Role.RoleType.Runner;
                    break;
            }

            foreach (DataRow myRow_loopVariable in dsMain.Tables[table].Rows)
            {
                myRow = myRow_loopVariable;
                if (myRow.RowState != DataRowState.Deleted)
                {
                    anEmp = new Employee(roleValue);
                    anEmp.ID = Convert.ToString(myRow["ID"]).TrimEnd();
                    anEmp.EmployeeID = Convert.ToString(myRow["EMPID"]).TrimEnd();
                    anEmp.Name = Convert.ToString(myRow["Name"]).TrimEnd();
                    anEmp.Telephone = Convert.ToString(myRow["Phone"]).TrimEnd();

                    switch (anEmp.role.getRoleValue)
                    {
                        case Role.RoleType.Headwaiter:
                            headW = (HeadWaiter)anEmp.role;
                            headW.SalaryAmount = ToDecimal(myRow["Salary"]);
                            break;
                        case Role.RoleType.Waiter:
                            waiter = (Waiter)anEmp.role;
                            waiter.getRate = ToDecimal(myRow["DayRate"]);
                            waiter.getShifts = ToInt(myRow["NoOfShifts"]);
                            waiter.getTips = ToDecimal(myRow["Tips"]);
                            break;
                        case Role.RoleType.Runner:
                            runner = (Runner)anEmp.role;
                            runner.getRate = ToDecimal(myRow["DayRate"]);
                            runner.getShifts = ToInt(myRow["NoOfShifts"]);
                            runner.getTips = ToDecimal(myRow["Tips"]);
                            break;
                    }
                    employees.Add(anEmp);
                }
            }
        }

        // 1.6.3 
        private void FillRow(DataRow aRow, Employee anEmp)
        {
            HeadWaiter headwaiter;
            Waiter waiter;
            Runner runner;

            aRow["ID"] = anEmp.ID;
            aRow["EMPID"] = anEmp.EmployeeID;
            aRow["Name"] = anEmp.Name;
            aRow["Phone"] = anEmp.Telephone;

            switch (anEmp.role.getRoleValue)
            {
                case Role.RoleType.Headwaiter:
                    headwaiter = (HeadWaiter)anEmp.role;
                    aRow["Role"] = ((byte)anEmp.role.getRoleValue).ToString();
                    aRow["Salary"] = headwaiter.SalaryAmount;
                    break;
                case Role.RoleType.Waiter:
                    waiter = (Waiter)anEmp.role;
                    aRow["Role"] = (byte)anEmp.role.getRoleValue;
                    aRow["DayRate"] = waiter.getRate;
                    aRow["NoOfShifts"] = (short)waiter.getShifts;
                    aRow["Tips"] = waiter.getTips;
                    break;
                case Role.RoleType.Runner:
                    runner = (Runner)anEmp.role;
                    aRow["Role"] = (byte)anEmp.role.getRoleValue;
                    aRow["DayRate"] = runner.getRate;
                    aRow["NoOfShifts"] = (short)runner.getShifts;
                    aRow["Tips"] = runner.getTips;
                    break;
            }
        }

        
        private static decimal ToDecimal(object value)
        {
            return value == DBNull.Value ? 0m : Convert.ToDecimal(value);
        }

        private static int ToInt(object value)
        {
            return value == DBNull.Value ? 0 : Convert.ToInt32(value);
        }
        #endregion

        #region Database Operations CRUD
        // 1.8 
        public void DataSetChange(Employee anEmp)
        {
            DataRow aRow = null;
            string dataTable = table1;

            switch (anEmp.role.getRoleValue)
            {
                case Role.RoleType.Headwaiter:
                    dataTable = table1;
                    break;
                case Role.RoleType.Waiter:
                    dataTable = table2;
                    break;
                case Role.RoleType.Runner:
                    dataTable = table3;
                    break;
            }

            aRow = dsMain.Tables[dataTable].NewRow();
            FillRow(aRow, anEmp);
            dsMain.Tables[dataTable].Rows.Add(aRow);
        }
        #endregion

        #region Build Parameters, Create Commands & Update database
        // 1.9.1 Parameters for the INSERT command
        private void Build_INSERT_Parameters(Employee anEmp)
        {
            SqlParameter param = default(SqlParameter);

            param = new SqlParameter("@ID", SqlDbType.NVarChar, 15, "ID");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@EMPID", SqlDbType.NVarChar, 10, "EMPID");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@Name", SqlDbType.NVarChar, 100, "Name");
            daMain.InsertCommand.Parameters.Add(param);

            param = new SqlParameter("@Phone", SqlDbType.NVarChar, 15, "Phone");
            daMain.InsertCommand.Parameters.Add(param);

            // 1.9.1.2 
            switch (anEmp.role.getRoleValue)
            {
                case Role.RoleType.Headwaiter:
                    param = new SqlParameter("@Role", SqlDbType.NChar, 10, "Role");
                    daMain.InsertCommand.Parameters.Add(param);
                    param = new SqlParameter("@Salary", SqlDbType.Money, 8, "Salary");
                    daMain.InsertCommand.Parameters.Add(param);
                    break;
                case Role.RoleType.Waiter:
                case Role.RoleType.Runner:
                    param = new SqlParameter("@Role", SqlDbType.TinyInt, 1, "Role");
                    daMain.InsertCommand.Parameters.Add(param);
                    param = new SqlParameter("@DayRate", SqlDbType.Money, 8, "DayRate");
                    daMain.InsertCommand.Parameters.Add(param);
                    param = new SqlParameter("@NoOfShifts", SqlDbType.SmallInt, 2, "NoOfShifts");
                    daMain.InsertCommand.Parameters.Add(param);
                    param = new SqlParameter("@Tips", SqlDbType.Money, 8, "Tips");
                    daMain.InsertCommand.Parameters.Add(param);
                    break;
            }
        }

        // 1.9.2 
        private void Create_INSERT_Command(Employee anEmp)
        {
            switch (anEmp.role.getRoleValue)
            {
                case Role.RoleType.Headwaiter:
                    daMain.InsertCommand = new SqlCommand(
                        "INSERT into HeadWaiter (ID, EMPID, Name, Phone, Role, Salary) " +
                        "VALUES (@ID, @EMPID, @Name, @Phone, @Role, @Salary)", cnMain);
                    break;
                case Role.RoleType.Waiter:
                    daMain.InsertCommand = new SqlCommand(
                        "INSERT into Waiter (ID, EMPID, Name, Phone, Role, DayRate, NoOfShifts, Tips) " +
                        "VALUES (@ID, @EMPID, @Name, @Phone, @Role, @DayRate, @NoOfShifts, @Tips)", cnMain);
                    break;
                case Role.RoleType.Runner:
                    daMain.InsertCommand = new SqlCommand(
                        "INSERT into Runner (ID, EMPID, Name, Phone, Role, DayRate, NoOfShifts, Tips) " +
                        "VALUES (@ID, @EMPID, @Name, @Phone, @Role, @DayRate, @NoOfShifts, @Tips)", cnMain);
                    break;
            }
            Build_INSERT_Parameters(anEmp);
        }

        // 1.9.3 
        public bool UpdateDataSource(Employee anEmp)
        {
            bool success = true;
            Create_INSERT_Command(anEmp);

            switch (anEmp.role.getRoleValue)
            {
                case Role.RoleType.Headwaiter:
                    success = UpdateDataSource(sqlLocal1, table1);
                    break;
                case Role.RoleType.Waiter:
                    success = UpdateDataSource(sqlLocal2, table2);
                    break;
                case Role.RoleType.Runner:
                    success = UpdateDataSource(sqlLocal3, table3);
                    break;
            }
            return success;
        }
        #endregion
    }
}