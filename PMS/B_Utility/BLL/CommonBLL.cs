using B_DB_Context;
using B_DB_Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static B_Utility.BLL.CommonBLL;

namespace B_Utility.BLL
{
    public class CommonBLL : IDisposable
    {
        private readonly DataBase_Context _context;

        public CommonBLL(DataBase_Context context)
        {
            _context = context;
        }

        #region GetNameById

        public string GetPropertyByMeterId(string meterNo)
        {
            return _context.MeterDetail.Where(x => x.MeterNumber == meterNo).Select(x => x.MeterInstallation.PropertyNo).FirstOrDefault();
        }

        public string GetRealEstateName(int id)
        {
            return _context.Real_Estates.Where(x => x.ID == id).Select(x => x.Description).FirstOrDefault();
        }

        public string GetProjectName(int id)
        {
            return _context.Projects.Where(x => x.ID == id).Select(x => x.Description).FirstOrDefault();
        }

        public string GetPhaseName(int id)
        {
            return _context.Phases.Where(x => x.ID == id).Select(x => x.Description).FirstOrDefault();
        }

        public string GetBlockName(int id)
        {
            return _context.Blocks.Where(x => x.ID == id).Select(x => x.Description).FirstOrDefault();
        }
        
        public string GetFloorName(int id)
        {
            return _context.Floors.Where(x => x.ID == id).Select(x => x.Description).FirstOrDefault();
        }

        public string GetCategoryName(int id)
        {
            return _context.Categories.Where(x => x.ID == id).Select(x => x.Description).FirstOrDefault();
        }

        public string GetTypeName(int id)
        {
            return _context.PropertyTypes.Where(x => x.ID == id).Select(x => x.Description).FirstOrDefault();
        }

        public string GetSectoreName(int id)
        {
            return _context.Sectors.Where(x => x.ID == id).Select(x => x.Description).FirstOrDefault();
        }

        public string GetNatureName(int id)
        {
            return _context.Natures.Where(x => x.ID == id).Select(x => x.Description).FirstOrDefault();
        }
        public string GetGlobalChargeName(int id)
        {
            return _context.GlobalChargeSetup.Where(x => x.Id == id).Select(x => x.Description).FirstOrDefault();
        }
        #endregion

        #region approvals

        public bool UpdateStockCreation()
        {
            var stock = _context.StockCreations.Where(x => !x.is_deleted && x.is_active == true && x.Is_StockCreationRequested == true).ToList();

            if(stock?.Count > 0)
            {
                foreach(var item in stock)
                {
                    item.Is_StockCreationApproved = true;
                    item.Updated_at = DateTime.Now;

                    _context.SaveChanges();
                }

                return true;
            }
            return false;
        }
        #endregion

        public string GetConstrcutionStatus(int ID)
        {
            string status = "";
            var result =  _context.ConstructionMonitoring.Where(x => x.StockCreationId == ID).FirstOrDefault();
            if (result != null)
                status = result.ConstructionStatus;
            else
                status = "";
            return status;
        }

        public string GetSapAccountByChargeTypeId(int id)
        {
            return _context.ChargeGroupType.Where(x => x.Id == id).Select(x => x.SapAccount).FirstOrDefault();
        }
        public string GetSapAccountByGlobalChargeDetail(int id)
        {
            return _context.GlobalChargeDetail.Where(x => x.Id == id).Select(x => x.SapAccount).FirstOrDefault();
        }
        public string GetChargeTypeDescription(int id)
        {
            var asd = _context.ChargeGroupType.Where(x => x.Id == id).Select(x => x.ChargeTypeName).FirstOrDefault();
            return asd;
        }
        public string GetSapCardCodeForMember(int id)
        {
            var docnum= _context.MemberProfile.Where(x => x.Id == id).Select(x => x.DocNum).FirstOrDefault();
            return docnum != null ? docnum : "N/A";
        }

        public int GetStockIdFromNDCMember(int id)
        {
            return (int)_context.NDCRequestForMember.Where(x => x.Id == id).Select(x => x.StockCreationId).FirstOrDefault();
        }

        public string GetDepartmentFromUserId(int userId)
        {
            var depart = _context.PMSUser.Where(x => x.Id == userId).FirstOrDefault();
            return depart.DEPARTMENT_DESC ?? "N/A";
        }
         
        public DateTime? GetConstructedDateTime(int id)
        {
            var constructionMonitoring = _context.ConstructionMonitoring.FirstOrDefault(x => x.StockCreationId == id);

            if (constructionMonitoring != null)
            {
                return constructionMonitoring.LastModified;
            }

            return null;
        }

        public SoftLockResponse IsSoftLockActive(int stockCreationId, int softlock)
        {
            SoftLockResponse softLockResponse = new SoftLockResponse();

            StockCreation stock = _context.StockCreations.Find(stockCreationId);

            if (stock is not null && stock.PropertyStatus == "Cancel")
            {
                softLockResponse.IsFound = true;
                softLockResponse.message = $"The Property is Cancelled by N-Stack";

                return softLockResponse;
            }

            DateTime currentDate = DateTime.Now.Date;

            var result = _context.SoftLock.Where(x =>
                                         x.RegistrationNoProfile.StockCreationId == stockCreationId &&
                                         x.Reason == softlock.ToString() &&
                                         x.Status == "Active"
                                        //&&
                                        //x.StartDate <= DateTime.Now.Date.AddDays(-1) &&
                                        //x.EndDate >= DateTime.Now.Date
                                        ).FirstOrDefault();
            if (result != null)
            {
                softLockResponse.IsFound = true;
                softLockResponse.message = $"The Property is under {result.SoftLockName}";
            }

            return softLockResponse;
        }

        public async Task<string> GetNextChallanNumberAsync(string? source = "CHALLAN")
        {
            //using var transaction = await _context.Database
            //    .BeginTransactionAsync(IsolationLevel.Serializable);

            var series = await _context.VoucherSeries
                .Where(x => x.VoucherType == source)
                .FirstOrDefaultAsync();

            if (series == null)
                throw new Exception("Voucher series not configured");

            series.CurrentNumber += 1;
            series.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            //await transaction.CommitAsync();

            var now = DateTime.Now;
            var year = now.Month >= 7 ? now.Year + 1 : now.Year;
            var financialYear = now.Month.ToString("D2") + "-" + (year % 100).ToString("D2");

            return $"{series.Prefix}/{financialYear}/{series.CurrentNumber:D6}";
        }


        public class SoftLockResponse
        {
            public bool IsFound { get; set; }
            public string? message { get; set; }
        }

        public void Dispose()
        {
              _context.Dispose();
        }
    }
}
