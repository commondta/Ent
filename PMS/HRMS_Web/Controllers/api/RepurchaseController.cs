using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common.Enums;
using B_Utility.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CloudinaryDotNet.Actions;
using static iTextSharp.text.pdf.AcroFields;
using Microsoft.EntityFrameworkCore;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepurchaseController : ControllerBase
    {
        private readonly DataBase_Context _db;
        public RepurchaseController(DataBase_Context db)
        {
            _db = db;
        }
        [HttpGet]
        [Route("GetRequestForPrint")]
        public IActionResult GetRequestForPrint(int id)
        {
            try
            {
                var result = _db.RePurchase
                                         .Where(x => x.Id == id)
                                         .Include(x => x.RePurchaseFinanceDetail)
                                         .Include(x => x.RePurchasePropertyDivision)
                                         .Include(x => x.StockCreation)
                                             .ThenInclude(x => x.MemberProfile)
                                         .Select(x => new
                                         {
                                             DocNumber = x.Id,
                                             Type = x.Type,
                                             RegistrationNo = x.StockCreation.RegistrationNo,
                                             PropertNo = x.StockCreation.PropertyNo,
                                             MemberProfile = x.StockCreation.MemberProfile,
                                             Father_Huband_Name = x.StockCreation.MemberProfile.RelationshipWith ?? "N/A",
                                             MemberCnic = x.StockCreation.MemberProfile.Cnic ?? "N/A",
                                             Area = x.StockCreation.ActualSize,
                                             UnitArea = x.StockCreation.ActualSizeUnit,
                                             Block = _db.Blocks.Where(y => y.ID == Convert.ToInt32(x.StockCreation.Block)).FirstOrDefault().Description ?? "N/A",
                                             MarketValue = x.MarketValue ?? "N/A",
                                             PurchaseRefundValue = x.PurchaseRefundValue ?? "N/A",
                                             NetProfitLoss = x.NetProfitLoss ?? "N/A",
                                             BookingPrice = x.RePurchaseFinanceDetail.Sum(financeDetail => financeDetail.DocTotal),
                                             ReceivedAmount = x.RePurchaseFinanceDetail.Sum(financeDetail => financeDetail.AmountRecieved),
                                             BalanceAmount = x.RePurchaseFinanceDetail.Sum(financeDetail => financeDetail.AmountDue),
                                             DocDate = x.CreatedOn
                                         })
                                         .FirstOrDefault();

                return Ok(result);

            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }
        [HttpPost]
        [Route("/api/Repurchase/AddRepurchased")]
        public IActionResult AddRepurchased(RePurchase model)
        {
            try
            {
                Response_Result responce = null;


                if (model.RePurchasePropertyDivision.Count() == 0 && model.Type == "Cancellation")
                {
                    var existingReg = _db.StockCreations.FirstOrDefault(x => x.ID == model.StockCreationId);



                    if (existingReg != null)
                    {
                        // lock previous registration no.

                        existingReg.is_deleted = true;
                        existingReg.is_active = false;
                        if (model.DeductionAmount !="")
                        {
                            //responce = new SapIntegrationController(_db).PostingCreditNoteForRepurchase(model);
                            //if (responce.code > 0)
                            //{
                            //    return Ok(new ApiResponse<object>
                            //    {
                            //        Code = ResponseCode.BadRequest,
                            //        Message = responce.message,
                            //        Data = null
                            //    });
                            //}
                        }
                           
                        _db.StockCreations.Update(existingReg);

                        _db.SaveChanges();

                        // add repurchase table
                        model.IsActive = true;
                        model.CreatedOn = model.CreatedOn;
                        model.CreatedBy = model.CreatedBy;
                        model.LastModified = DateTime.Now;
                        model.ModifiedBy = model.ModifiedBy;
                        model.LastModifiedUserName = model.LastModifiedUserName;

                        _db.RePurchase.Add(model);

                        // park idle property
                        string PropertyNo = existingReg.PropertyNo;
                        string PrefixProperty = existingReg.PrefixProperty;
                        int? numForProperty = existingReg.numForProperty;
                        string postfixForProperty = existingReg.postfixForProperty;
                        string Block = existingReg.Block;
                        string Category = existingReg.Category;
                        string RealStateType = existingReg.RealStateType;
                        string ActualSize = existingReg.ActualSize;
                        string ActualSizeUnit = existingReg.ActualSizeUnit;
                        string Project = existingReg.Project;
                        string Phase = existingReg.Phase;
                        string Type = existingReg.Type;
                        string Nature = existingReg.Nature;
                        string Finishing = existingReg.Finishing;
                        string Floor = existingReg.Floor;
                        bool? PossessionStatus = existingReg.PossessionStatus;
                        string ConstracutionStatus = existingReg.ConstracutionStatus;
                        decimal? coveredArea = existingReg.coveredArea;
                        string? LDAPlotNo = existingReg.LDAPlotNo;
                        string? LDAAreaSize = existingReg.LDAAreaSize;

                        existingReg.ID = 0;
                        existingReg.RegistrationNo = null;
                        existingReg.PropertyNo = PropertyNo;
                        existingReg.PrefixProperty = PrefixProperty;
                        existingReg.numForProperty = numForProperty;
                        existingReg.postfixForProperty = postfixForProperty;
                        existingReg.Block = Block;
                        existingReg.Category = Category;
                        existingReg.RealStateType = RealStateType;
                        existingReg.ActualSize = ActualSize;
                        existingReg.ActualSizeUnit = ActualSizeUnit;
                        existingReg.Project = Project;
                        existingReg.Phase = Phase;
                        existingReg.Type = Type;
                        existingReg.Nature = Nature;
                        existingReg.Finishing = Finishing;
                        existingReg.Floor = Floor;
                        existingReg.PossessionStatus = PossessionStatus;
                        existingReg.ConstracutionStatus = ConstracutionStatus;
                        existingReg.coveredArea = coveredArea;
                        existingReg.LDAPlotNo = LDAPlotNo;
                        existingReg.LDAAreaSize = LDAPlotNo;
                        existingReg.is_deleted = false;
                        existingReg.is_active = true;
                        existingReg.Is_StockCreationRequested = true;
                        existingReg.Is_StockCreationApproved = true;

                        _db.StockCreations.Add(existingReg);

                        //save all
                        _db.SaveChanges();

                    }

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Cancellation Successfull",
                        Data = null
                    });
                }

                if (model.RePurchasePropertyDivision.Count() <= 0)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "Please add atleast one row for property division",
                        Data = null
                    });
                }

                StockCreation block = (StockCreation)_db.StockCreations.Where(x => x.ID == model.StockCreationId)
                                                                       .FirstOrDefault();
                if (block == null)
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.NotFound,
                        Message = "Please select a property",
                        Data = null
                    });
                }

                if (model.RePurchasePropertyDivision.Count() > 0)
                {
                    foreach (var item in model.RePurchasePropertyDivision)
                    {
                        String.Format("{0:0000}", item.RegNumber);
                        String.Format("{0:0000}", item.PropNumber);
                    }
                }

                string propertyNo = "";

                if (model.RePurchasePropertyDivision.Count() == 1)
                {
                    foreach (var item in model.RePurchasePropertyDivision)
                    {
                        if (!item.PropPostfix.IsNullOrEmpty())
                        {
                            return Ok(new ApiResponse<object>
                            {
                                Code = ResponseCode.NotFound,
                                Message = "You are unable to add a division.",
                                Data = null
                            });
                        }

                        if (!item.PropPrefix.IsNullOrEmpty())
                        {
                            return Ok(new ApiResponse<object>
                            {
                                Code = ResponseCode.NotFound,
                                Message = "You are unable to add a Prefix.",
                                Data = null
                            });
                        }

                        if (item.PropNumber != 0)
                        {
                            return Ok(new ApiResponse<object>
                            {
                                Code = ResponseCode.NotFound,
                                Message = "Keep Prop Number 0",
                                Data = null
                            });
                        }

                        if (Convert.ToInt32(item.Size) == 0)
                        {
                            return Ok(new ApiResponse<object>
                            {
                                Code = ResponseCode.NotFound,
                                Message = "If property has no division add size.",
                                Data = null
                            });
                        }

                        if (item.PropNumber == 0)
                        {
                            propertyNo = block.PropertyNo;
                        }
                    }
                }

                if (model.RePurchasePropertyDivision.Count() > 1)
                {
                    var stock = _db.StockCreations.ToList();
                    var currentProp = stock.Find(x => x.ID == Convert.ToInt32(model.StockCreationId));
                    var distinctPropPostfix = model.RePurchasePropertyDivision.Select(x => x.PropPostfix).Distinct().ToList();

                    if (distinctPropPostfix.Count != model.RePurchasePropertyDivision.Count)
                    {
                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.BadRequest,
                            Message = "Duplicates found in the values",
                            Data = null
                        });
                    }

                    int index = 0;
                    foreach (var item in model.RePurchasePropertyDivision)
                    {
                        if (index > 0 && (!item.PropPostfix.IsNullOrEmpty() && item.PropNumber != 0))
                        {
                            bool IsExist = stock.Any(x => x.Block == currentProp.Block && x.postfixForProperty == item.PropPostfix && x.numForProperty == item.PropNumber);
                            if (IsExist)
                            {
                                return Ok(new ApiResponse<object>
                                {
                                    Code = ResponseCode.NotFound,
                                    Message = "Property Number Already Exist.",
                                    Data = null
                                });
                            }
                        }

                        if (index > 0 && (item.PropPostfix.IsNullOrEmpty() || item.PropNumber == 0))
                        {
                            return Ok(new ApiResponse<object>
                            {
                                Code = ResponseCode.NotFound,
                                Message = "You are missing division for the items excluding the first item or adding propnumber.",
                                Data = null
                            });
                        }

                        if (index == 0 && (item.PropNumber == 0 || item.PropPostfix != ""))
                        {
                            return Ok(new ApiResponse<object>
                            {
                                Code = ResponseCode.NotFound,
                                Message = "You are adding propnumber for the first item or putting PropPostfix.",
                                Data = null
                            });
                        }

                        if (Convert.ToInt32(item.Size) == 0 || item.Size.IsNullOrEmpty())
                        {
                            return Ok(new ApiResponse<object>
                            {
                                Code = ResponseCode.NotFound,
                                Message = "Please add size for each item",
                                Data = null
                            });
                        }
                        index++;
                    }
                }



                // need to check property duplication

                block.is_deleted = true;
                block.is_active = false;
                if (model.DeductionAmount != "")
                {
                    //responce = new SapIntegrationController(_db).PostingCreditNoteForRepurchase(model);
                    //if (responce.code > 0)
                    //{
                    //    return Ok(new ApiResponse<object>
                    //    {
                    //        Code = ResponseCode.BadRequest,
                    //        Message = responce.message,
                    //        Data = null
                    //    });
                    //}
                }
                _db.StockCreations.Update(block);
                _db.SaveChanges();

                if (model.RePurchasePropertyDivision.Count > 0)
                {
                    foreach (var item in model.RePurchasePropertyDivision)
                    {
                        block.ID = 0;
                        block.ActualSize = item.Size;
                        block.Category = item.Category;
                        block.PrefixRegistration = item.RegPrefix;
                        block.numForRegistration = item.RegNumber;
                        block.postfixForRegistration = item.RegPostfix;
                        block.RegistrationNo = item.RegPrefix + String.Format("{0:0000}", item.RegNumber) + item.RegPostfix;
                        block.MemberProfileId = null;
                        block.DealerId = null;
                        block.is_deleted = false;
                        block.is_active = true;
                        block.Is_StockCreationRequested = true;
                        block.Is_StockCreationApproved = true;

                        if (propertyNo.IsNullOrEmpty())
                        {
                            block.PrefixProperty = item.PropPrefix;
                            block.numForProperty = item.PropNumber;
                            block.postfixForProperty = item.PropPostfix;
                            block.PropertyNo = item.PropPrefix + String.Format("{0:0000}", item.PropNumber) + item.PropPostfix;
                        }

                        _db.StockCreations.Add(block);
                        _db.SaveChanges();
                    }
                }

                model.IsActive = true;
                model.CreatedOn = DateTime.Now;
                model.CreatedBy = model.CreatedBy;
                model.LastModified = DateTime.Now;
                model.ModifiedBy = model.ModifiedBy;
                model.LastModifiedUserName = model.LastModifiedUserName;

                _db.RePurchase.Add(model);
                _db.SaveChanges();



                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = null
                });
            }

            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetPropertiesForRepurchase")]
        public IActionResult GetPropertiesForRepurchase()
        {
            try
            {
                var result = _db.StockCreations.Where(x => !x.is_deleted &&
                                                x.MemberProfileId != null &&
                                                x.RegistrationNo != null &&
                                                x.PropertyNo != null)
                                               .Distinct()
                                               .Select(x => new
                                               {
                                                   x.ID,
                                                   x.RegistrationNo,
                                                   x.PropertyNo,
                                                   x.Dealer.PrincipalOwner,
                                                   x.MemberProfile.MemberName,
                                               })
                                               .ToList();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetAllRequest")]
        public IActionResult GetAllRequest()
        {
            try
            {
                var result = _db.RePurchase.Where(x => !x.IsDeleted)
                                           .Select(x => new
                                           {
                                               x.Id,
                                               x.StockCreation.RegistrationNo,
                                               x.StockCreation.PropertyNo,
                                               x.Type,
                                               x.CreatedOn
                                           })
                                           .ToList();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetRequest")]
        public IActionResult GetRequest(int id)
        {
            try
            {
                var result = _db.RePurchase.Where(x => !x.IsDeleted && x.Id == id)
                                           .Include(x => x.StockCreation)
                                           .ThenInclude(x => x.MemberProfile)
                                           .Include(x => x.RePurchasePropertyDivision)
                                           .FirstOrDefault();

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = result
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }


        [HttpDelete]
        [Route("/api/Repurchase/Cancel")]
        public IActionResult Cancel(int id)
        {
            try
            {

                var model = _db.RePurchase.Include(x => x.RePurchasePropertyDivision).FirstOrDefault(x => x.Id == id);

                if (model.RePurchasePropertyDivision.Count() == 0 && model.Type == "Cancellation")
                {

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "Use Property Binding If both Registration and PropertyNo are still vacant ",
                        Data = null
                    });
                }

                StockCreation block = (StockCreation)_db.StockCreations.Where(x => x.ID == model.StockCreationId)
                                                                       .FirstOrDefault();
                if (block != null)
                {


                    block.is_deleted = false;
                    block.is_active = true;

                    _db.StockCreations.Update(block);


                    if (model.RePurchasePropertyDivision.Count > 0)
                    {
                        foreach (var item in model.RePurchasePropertyDivision)
                        {
                            string regNo = (item.RegPrefix + item.RegNumber + item.RegPostfix).ToString().Trim();
                            var stockItem = _db.StockCreations.Where(x => x.RegistrationNo == regNo).FirstOrDefault();
                            stockItem.PropertyNo = null;
                            stockItem.MemberProfileId = null;

                            _db.StockCreations.Update(stockItem);
                        }
                    }
                    _db.SaveChanges();

                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.Success,
                        Message = "Reversal Successfully",
                        Data = null
                    });
                }
                else
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "something went wrong",
                        Data = null
                    });
                }
            }

            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }
    }
}
