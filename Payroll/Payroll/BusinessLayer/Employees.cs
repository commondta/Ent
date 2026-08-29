using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Employees
    {
        SqlConnection sql_connection;
        Database database;
        string query;

        public Employees(string con_string)
        {
            sql_connection = new SqlConnection(con_string);
            database = new Database(con_string);
        }

        public string getInsertQuery(SalaryDetailModel[] childData)
        {
            string query = "";
            for (int i = 0; i < childData.Length; i++)
            {
                query += "INSERT INTO SalaryDetail (employeeId, Code, Name, EffectiveDate, Type, Amount, OT, Tax) " +
                        "VALUES (@Parent_ID, @c" + i + "_Code, @c" + i + "_Name, @c" + i + "_EffectiveDate, @c" + i + "_Type, @c" + i + "_Amount, @c" + i + "_OT, @c" + i + "_Tax) ";
            }
            return query;
        }

        private List<SqlParameter> getChildInsertParameters(SalaryDetailModel[] childData)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            for (int i = 0; i < childData.Length; i++)
            {
                parameters.Add(new SqlParameter("@c" + i + "_Code", (object)childData[i].Code ?? DBNull.Value));
                parameters.Add(new SqlParameter("@c" + i + "_Name", (object)childData[i].Name ?? DBNull.Value));
                parameters.Add(new SqlParameter("@c" + i + "_EffectiveDate", childData[i].EffectiveDate));
                parameters.Add(new SqlParameter("@c" + i + "_Type", (object)childData[i].Type ?? DBNull.Value));
                parameters.Add(new SqlParameter("@c" + i + "_Amount", (object)childData[i].Amount ?? DBNull.Value));
                parameters.Add(new SqlParameter("@c" + i + "_OT", childData[i].OT));
                parameters.Add(new SqlParameter("@c" + i + "_Tax", childData[i].Tax));
            }
            return parameters;
        }

        public List<SalaryDetailModel> Insert(EmployeeDetailModel parentData, SalaryDetailModel[] childData)
        {
            List<SqlParameter> parameters = getParentParameters(parentData);
            parameters.AddRange(getChildInsertParameters(childData));

            query = @"DECLARE @Parent_ID INT " +
                    "BEGIN TRAN " +
                        "INSERT INTO EmployeeDetail (PayrollName, EmployeeNumber, SalutationTitle, LegalFirstName, LegalLastName, MaritalStatus, Gender, DateOfBirth, CompanyStartDate, CitizenshipCountry, PhoneNo, MobileNo, EmailAddress, PostalAddress1, PostalAddress2, PostalAddress3, PostalTown, PostalZipCode, AccountName, AccountType, AccountNumber, SwiftCode, IBANno, BankName, BranchName, BranchCode, BankPostalAddress1, BankPostalAddress2, DaysWorkedEachWeek, HoursPerWeek, CostCenter, Department, PayrollAssignmentStartDate, PayrollAssignmentEndDate, JobTitlePosition, SalaryInstallments, NationalIdentityCardNo, CountryOfBirth, NationalTaxNumber, EmploymentContract) " +
                        "VALUES (@PayrollName, @EmployeeNumber, @SalutationTitle, @LegalFirstName, @LegalLastName, @MaritalStatus, @Gender, @DateOfBirth, @CompanyStartDate, @CitizenshipCountry, @PhoneNo, @MobileNo, @EmailAddress, @PostalAddress1, @PostalAddress2, @PostalAddress3, @PostalTown, @PostalZipCode, @AccountName, @AccountType, @AccountNumber, @SwiftCode, @IBANno, @BankName, @BranchName, @BranchCode, @BankPostalAddress1, @BankPostalAddress2, @DaysWorkedEachWeek, @HoursPerWeek, @CostCenter, @Department, @PayrollAssignmentStartDate, @PayrollAssignmentEndDate, @JobTitlePosition, @SalaryInstallments, @NationalIdentityCardNo, @CountryOfBirth, @NationalTaxNumber, @EmploymentContract) " +
                        "SET @Parent_ID = SCOPE_IDENTITY() " +
                        getInsertQuery(childData) +
                    "COMMIT TRAN";
            database.Set(query, parameters.ToArray());

            query = "SELECT TOP (@RowCount) id, employeeId FROM SalaryDetail ORDER BY id DESC";
            DataTable dt = database.Get(query, new SqlParameter("@RowCount", childData.Length));
            List<SalaryDetailModel> list = new List<SalaryDetailModel>();
            SalaryDetailModel childIds;
            for (int i = 0; i < childData.Length; i++)
            {
                childIds = new SalaryDetailModel();
                childIds.id = Convert.ToInt16(dt.Rows[i][0]);
                childIds.employeeId = Convert.ToInt16(dt.Rows[i][1]);
                list.Add(childIds);
            }

            return list;
        }

        private List<SqlParameter> getParentParameters(EmployeeDetailModel parentData)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@PayrollName", (object)parentData.PayrollName ?? DBNull.Value));
            parameters.Add(new SqlParameter("@EmployeeNumber", (object)parentData.EmployeeNumber ?? DBNull.Value));
            parameters.Add(new SqlParameter("@SalutationTitle", (object)parentData.SalutationTitle ?? DBNull.Value));
            parameters.Add(new SqlParameter("@LegalFirstName", (object)parentData.LegalFirstName ?? DBNull.Value));
            parameters.Add(new SqlParameter("@LegalLastName", (object)parentData.LegalLastName ?? DBNull.Value));
            parameters.Add(new SqlParameter("@MaritalStatus", (object)parentData.MaritalStatus ?? DBNull.Value));
            parameters.Add(new SqlParameter("@Gender", (object)parentData.Gender ?? DBNull.Value));
            parameters.Add(new SqlParameter("@DateOfBirth", parentData.DateOfBirth));
            parameters.Add(new SqlParameter("@CompanyStartDate", parentData.CompanyStartDate));
            parameters.Add(new SqlParameter("@CitizenshipCountry", (object)parentData.CitizenshipCountry ?? DBNull.Value));
            parameters.Add(new SqlParameter("@PhoneNo", (object)parentData.PhoneNo ?? DBNull.Value));
            parameters.Add(new SqlParameter("@MobileNo", (object)parentData.MobileNo ?? DBNull.Value));
            parameters.Add(new SqlParameter("@EmailAddress", (object)parentData.EmailAddress ?? DBNull.Value));
            parameters.Add(new SqlParameter("@PostalAddress1", (object)parentData.PostalAddress1 ?? DBNull.Value));
            parameters.Add(new SqlParameter("@PostalAddress2", (object)parentData.PostalAddress2 ?? DBNull.Value));
            parameters.Add(new SqlParameter("@PostalAddress3", (object)parentData.PostalAddress3 ?? DBNull.Value));
            parameters.Add(new SqlParameter("@PostalTown", (object)parentData.PostalTown ?? DBNull.Value));
            parameters.Add(new SqlParameter("@PostalZipCode", (object)parentData.PostalZipCode ?? DBNull.Value));
            parameters.Add(new SqlParameter("@AccountName", (object)parentData.AccountName ?? DBNull.Value));
            parameters.Add(new SqlParameter("@AccountType", (object)parentData.AccountType ?? DBNull.Value));
            parameters.Add(new SqlParameter("@AccountNumber", (object)parentData.AccountNumber ?? DBNull.Value));
            parameters.Add(new SqlParameter("@SwiftCode", (object)parentData.SwiftCode ?? DBNull.Value));
            parameters.Add(new SqlParameter("@IBANno", (object)parentData.IBANno ?? DBNull.Value));
            parameters.Add(new SqlParameter("@BankName", (object)parentData.BankName ?? DBNull.Value));
            parameters.Add(new SqlParameter("@BranchName", (object)parentData.BranchName ?? DBNull.Value));
            parameters.Add(new SqlParameter("@BranchCode", (object)parentData.BranchCode ?? DBNull.Value));
            parameters.Add(new SqlParameter("@BankPostalAddress1", (object)parentData.BankPostalAddress1 ?? DBNull.Value));
            parameters.Add(new SqlParameter("@BankPostalAddress2", (object)parentData.BankPostalAddress2 ?? DBNull.Value));
            parameters.Add(new SqlParameter("@DaysWorkedEachWeek", (object)parentData.DaysWorkedEachWeek ?? DBNull.Value));
            parameters.Add(new SqlParameter("@HoursPerWeek", (object)parentData.HoursPerWeek ?? DBNull.Value));
            parameters.Add(new SqlParameter("@CostCenter", (object)parentData.CostCenter ?? DBNull.Value));
            parameters.Add(new SqlParameter("@Department", (object)parentData.Department ?? DBNull.Value));
            parameters.Add(new SqlParameter("@PayrollAssignmentStartDate", parentData.PayrollAssignmentStartDate));
            parameters.Add(new SqlParameter("@PayrollAssignmentEndDate", parentData.PayrollAssignmentEndDate));
            parameters.Add(new SqlParameter("@JobTitlePosition", (object)parentData.JobTitlePosition ?? DBNull.Value));
            parameters.Add(new SqlParameter("@SalaryInstallments", (object)parentData.SalaryInstallments ?? DBNull.Value));
            parameters.Add(new SqlParameter("@NationalIdentityCardNo", (object)parentData.NationalIdentityCardNo ?? DBNull.Value));
            parameters.Add(new SqlParameter("@CountryOfBirth", (object)parentData.CountryOfBirth ?? DBNull.Value));
            parameters.Add(new SqlParameter("@NationalTaxNumber", (object)parentData.NationalTaxNumber ?? DBNull.Value));
            parameters.Add(new SqlParameter("@EmploymentContract", (object)parentData.EmploymentContract ?? DBNull.Value));
            return parameters;
        }

        public string getUpdateQuery(SalaryDetailModel[] childData)
        {
            string query = "";
            for (int i = 0; i < childData.Length; i++)
            {
                query += "UPDATE SalaryDetail SET Code=@u" + i + "_Code, Name=@u" + i + "_Name, EffectiveDate=@u" + i + "_EffectiveDate, Type=@u" + i + "_Type, Amount=@u" + i + "_Amount, OT=@u" + i + "_OT, Tax=@u" + i + "_Tax WHERE id=@u" + i + "_id; ";
            }
            return query;
        }

        private List<SqlParameter> getChildUpdateParameters(SalaryDetailModel[] childData)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            for (int i = 0; i < childData.Length; i++)
            {
                parameters.Add(new SqlParameter("@u" + i + "_Code", (object)childData[i].Code ?? DBNull.Value));
                parameters.Add(new SqlParameter("@u" + i + "_Name", (object)childData[i].Name ?? DBNull.Value));
                parameters.Add(new SqlParameter("@u" + i + "_EffectiveDate", childData[i].EffectiveDate));
                parameters.Add(new SqlParameter("@u" + i + "_Type", (object)childData[i].Type ?? DBNull.Value));
                parameters.Add(new SqlParameter("@u" + i + "_Amount", (object)childData[i].Amount ?? DBNull.Value));
                parameters.Add(new SqlParameter("@u" + i + "_OT", childData[i].OT));
                parameters.Add(new SqlParameter("@u" + i + "_Tax", childData[i].Tax));
                parameters.Add(new SqlParameter("@u" + i + "_id", childData[i].id));
            }
            return parameters;
        }

        public void Update(EmployeeDetailModel parentData, SalaryDetailModel[] childData)
        {
            string query = "";
            List<SqlParameter> parameters = new List<SqlParameter>();
            for (int i = 0; i < childData.Length; i++)
            {
                if (childData[i].id != 0)
                {
                    query += "UPDATE SalaryDetail SET Code=@p" + i + "_Code, Name=@p" + i + "_Name, EffectiveDate=@p" + i + "_EffectiveDate, Type=@p" + i + "_Type, Amount=@p" + i + "_Amount, OT=@p" + i + "_OT, Tax=@p" + i + "_Tax WHERE id=@p" + i + "_id; ";
                    parameters.Add(new SqlParameter("@p" + i + "_id", childData[i].id));
                }
                else
                {
                    query += "INSERT INTO SalaryDetail (employeeId, Code, Name, EffectiveDate, Type, Amount, OT, Tax) " +
                        "VALUES (@p" + i + "_employeeId, @p" + i + "_Code, @p" + i + "_Name, @p" + i + "_EffectiveDate, @p" + i + "_Type, @p" + i + "_Amount, @p" + i + "_OT, @p" + i + "_Tax) ";
                    parameters.Add(new SqlParameter("@p" + i + "_employeeId", parentData.id));
                }
                parameters.Add(new SqlParameter("@p" + i + "_Code", (object)childData[i].Code ?? DBNull.Value));
                parameters.Add(new SqlParameter("@p" + i + "_Name", (object)childData[i].Name ?? DBNull.Value));
                parameters.Add(new SqlParameter("@p" + i + "_EffectiveDate", childData[i].EffectiveDate));
                parameters.Add(new SqlParameter("@p" + i + "_Type", (object)childData[i].Type ?? DBNull.Value));
                parameters.Add(new SqlParameter("@p" + i + "_Amount", (object)childData[i].Amount ?? DBNull.Value));
                parameters.Add(new SqlParameter("@p" + i + "_OT", childData[i].OT));
                parameters.Add(new SqlParameter("@p" + i + "_Tax", childData[i].Tax));
            }

            query += "UPDATE EmployeeDetail SET PayrollName=@PayrollName" +
                                        ", EmployeeNumber=@EmployeeNumber" +
                                        ", SalutationTitle=@SalutationTitle" +
                                        ", LegalFirstName=@LegalFirstName" +
                                        ", LegalLastName=@LegalLastName" +
                                        ", MaritalStatus=@MaritalStatus" +
                                        ", Gender=@Gender" +
                                        ", DateOfBirth=@DateOfBirth" +
                                        ", CompanyStartDate=@CompanyStartDate" +
                                        ", CitizenshipCountry=@CitizenshipCountry" +
                                        ", PhoneNo=@PhoneNo" +
                                        ", MobileNo=@MobileNo" +
                                        ", EmailAddress=@EmailAddress" +
                                        ", PostalAddress1=@PostalAddress1" +
                                        ", PostalAddress2=@PostalAddress2" +
                                        ", PostalAddress3=@PostalAddress3" +
                                        ", PostalTown=@PostalTown" +
                                        ", PostalZipCode=@PostalZipCode" +
                                        ", AccountName=@AccountName" +
                                        ", AccountType=@AccountType" +
                                        ", AccountNumber=@AccountNumber" +
                                        ", SwiftCode=@SwiftCode" +
                                        ", IBANno=@IBANno" +
                                        ", BankName=@BankName" +
                                        ", BranchName=@BranchName" +
                                        ", BranchCode=@BranchCode" +
                                        ", BankPostalAddress1=@BankPostalAddress1" +
                                        ", BankPostalAddress2=@BankPostalAddress2" +
                                        ", DaysWorkedEachWeek=@DaysWorkedEachWeek" +
                                        ", HoursPerWeek=@HoursPerWeek" +
                                        ", CostCenter=@CostCenter" +
                                        ", Department=@Department" +
                                        ", PayrollAssignmentStartDate=@PayrollAssignmentStartDate" +
                                        ", PayrollAssignmentEndDate=@PayrollAssignmentEndDate" +
                                        ", JobTitlePosition=@JobTitlePosition" +
                                        ", SalaryInstallments=@SalaryInstallments" +
                                        ", NationalIdentityCardNo=@NationalIdentityCardNo" +
                                        ", CountryOfBirth=@CountryOfBirth" +
                                        ", NationalTaxNumber=@NationalTaxNumber" +
                                        ", EmploymentContract=@EmploymentContract" +
                                        " WHERE id=@id;";
            parameters.AddRange(getParentParameters(parentData));
            parameters.Add(new SqlParameter("@id", parentData.id));

            database.Set(query, parameters.ToArray());
        }

        public List<EmployeeDetailModel> getAll()
        {
            query = "SELECT * FROM EmployeeDetail";
            DataTable dt = database.Get(query);

            return dataTableToList(dt);
        }

        public List<EmployeeDetailModel> getPayrollProcess()
        {
            query = "SELECT id, LegalFirstName, LegalLastName FROM EmployeeDetail";
            DataTable dt = database.Get(query);

            return dataTableToListPayrollProcess(dt);
        }

        private List<EmployeeDetailModel> dataTableToListPayrollProcess(DataTable dt)
        {
            List<EmployeeDetailModel> list = new List<EmployeeDetailModel>();
            EmployeeDetailModel obj;
            foreach (DataRow row in dt.Rows)
            {
                obj = new EmployeeDetailModel();
                obj.id = Convert.ToInt32(row[0]);
                obj.LegalFirstName = row[1].ToString();
                obj.LegalLastName = row[2].ToString();
                obj.SalaryDetail = getSalaryDetail(obj.id);

                list.Add(obj);
            }

            return list;
        }

        private List<EmployeeDetailModel> dataTableToList(DataTable dt)
        {
            List<EmployeeDetailModel> list = new List<EmployeeDetailModel>();
            EmployeeDetailModel obj;
            foreach (DataRow row in dt.Rows)
            {
                obj = new EmployeeDetailModel();
                obj.id = Convert.ToInt32(row[0]);
                obj.PayrollName = row[1].ToString();
                obj.EmployeeNumber = row[2].ToString();
                obj.SalutationTitle = row[3].ToString();
                obj.LegalFirstName = row[4].ToString();
                obj.LegalLastName = row[5].ToString();
                obj.MaritalStatus = row[6].ToString();
                obj.Gender = row[7].ToString();
                obj.DateOfBirth = Convert.ToDateTime(row[8].ToString());
                obj.CompanyStartDate = Convert.ToDateTime(row[9].ToString());
                obj.CitizenshipCountry = row[10].ToString();
                obj.PhoneNo = row[11].ToString();
                obj.MobileNo = row[12].ToString();
                obj.EmailAddress = row[13].ToString();
                obj.PostalAddress1 = row[14].ToString();
                obj.PostalAddress2 = row[15].ToString();
                obj.PostalAddress3 = row[16].ToString();
                obj.PostalTown = row[17].ToString();
                obj.PostalZipCode = row[18].ToString();
                obj.AccountName = row[19].ToString();
                obj.AccountType = row[20].ToString();
                obj.AccountNumber = row[21].ToString();
                obj.SwiftCode = row[22].ToString();
                obj.IBANno = row[23].ToString();
                obj.BankName = row[24].ToString();
                obj.BranchName = row[25].ToString();
                obj.BranchCode = row[26].ToString();
                obj.BankPostalAddress1 = row[27].ToString();
                obj.BankPostalAddress2 = row[28].ToString();
                obj.DaysWorkedEachWeek = row[29].ToString();
                obj.HoursPerWeek = row[30].ToString();
                obj.CostCenter = row[31].ToString();
                obj.Department = row[32].ToString();
                obj.PayrollAssignmentStartDate = Convert.ToDateTime(row[33].ToString());
                obj.PayrollAssignmentEndDate = Convert.ToDateTime(row[34].ToString());
                obj.JobTitlePosition = row[35].ToString();
                obj.SalaryInstallments = row[36].ToString();
                obj.NationalIdentityCardNo = row[37].ToString();
                obj.CountryOfBirth = row[38].ToString();
                obj.NationalTaxNumber = row[39].ToString();
                obj.EmploymentContract = row[40].ToString();
                obj.SalaryDetail = getSalaryDetail(obj.id);

                list.Add(obj);
            }

            return list;
        }

        public List<SalaryDetailModel> getSalaryDetail(int employeeId)
        {
            query = "SELECT id, Code, Name, EffectiveDate, Type, Amount, OT, Tax FROM SalaryDetail WHERE employeeId=@employeeId";
            DataTable dt = database.Get(query, new SqlParameter("@employeeId", employeeId));

            return dataTableToList_Salary(dt);
        }

        private List<SalaryDetailModel> dataTableToList_Salary(DataTable dt)
        {
            List<SalaryDetailModel> list = new List<SalaryDetailModel>();
            SalaryDetailModel obj;
            foreach (DataRow row in dt.Rows)
            {
                obj = new SalaryDetailModel();
                obj.id = Convert.ToInt32(row[0]);
                obj.Code = row[1].ToString();
                obj.Name = row[2].ToString();
                obj.EffectiveDate = Convert.ToDateTime(row[3].ToString());
                obj.Type = row[4].ToString();
                obj.Amount = row[5].ToString();
                obj.OT = Convert.ToBoolean(row[6]);
                obj.Tax = Convert.ToBoolean(row[7]);

                list.Add(obj);
            }

            return list;
        }

        public void Delete(string id)
        {
            query = "DELETE FROM SalaryDetail WHERE employeeId=@id " +
                    "DELETE FROM EmployeeDetail WHERE id=@id";
            database.Set(query, new SqlParameter("@id", (object)id ?? DBNull.Value));
        }

        public Double getTxbleSlry(Int32 employeeId)
        {
            query = "SELECT SUM(CONVERT(float, Amount)) " +
                    "FROM SalaryDetail " +
                    "WHERE EmployeeID = @employeeId AND Tax = 1";

            DataTable dt = database.Get(query, new SqlParameter("@employeeId", employeeId));
            if (dt.Rows[0][0].ToString() == "")
            {
                return 0;
            }
            return Convert.ToDouble(dt.Rows[0][0]);
        }
    }
}
