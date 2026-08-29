using B_DB_Context;
using B_DB_Model;
using B_Utility.Common;
using HRMS_Web.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FormsController : ControllerBase
    {
        private DataBase_Context _db;
        private List<FormsModel> forms = new List<FormsModel>() 
                                        {  
                                         new FormsModel   { FormName = "Demarcation", FormId = 1 },
                                         new FormsModel   { FormName = "Meter Bill Genration", FormId = 2 },
                                         new FormsModel   { FormName = "Monthly Bill Genration", FormId = 3 },
                                         new FormsModel   { FormName = "Individual Bill Genration", FormId = 4 }, 
                                         new FormsModel   { FormName = "Fixed Bill Genration", FormId = 5 }, 
                                         new FormsModel   { FormName = "Payment Plan Setup", FormId = 6 } ,
                                         new FormsModel   { FormName = "Booking Processing Charges", FormId = 7 } ,
                                         new FormsModel   { FormName = "NDC Member Charges", FormId = 8 } ,
                                         new FormsModel   { FormName = "NDC Dealer Charges", FormId = 9 } ,
                                         new FormsModel   { FormName = "Seller Govt Taxes", FormId = 10 } ,
                                         new FormsModel   { FormName = "Buyer Govt Taxes", FormId = 11 } ,
                                         new FormsModel   { FormName = "Resurrender Charges", FormId = 12 },
                                         new FormsModel   { FormName = "File Verification Charges", FormId = 13 },
                                         new FormsModel   { FormName = "File Request Charges", FormId = 14 },
                                         new FormsModel   { FormName = "Charges WavieOff", FormId = 15 }
                                        };
        private List<FormsModel> Alertforms = new List<FormsModel>()
                                        {
                                         new FormsModel   { FormName = "Alert On NDC Request", FormId = 1 },
                                         new FormsModel   { FormName = "Alert On NDC 1", FormId = 2 },
                                         new FormsModel   { FormName = "Alert On Transfer Set Recieveing", FormId = 3 },
                                         new FormsModel   { FormName = "Alert On Transfer Receipt", FormId = 5 },
                                         new FormsModel   { FormName = "Alert On Transfer", FormId = 6 },
                                         new FormsModel   { FormName = "Alert On Expiry NDC 10 Days Before", FormId = 4 },
                                         new FormsModel   { FormName = "Clearance (TP)", FormId = 7 },
                                         new FormsModel   { FormName = "Redesign MP-CM", FormId = 8 },
                                        };

        public FormsController( DataBase_Context context )
        {
            _db = context;
        }

        [HttpGet]
        [Route("GetFormsDetail")]
        public IActionResult GetFormsDetail(int id)
        {
            try
            {
                var allChargeGroups = _db.FormsChargeGroup.Where(x =>x.FormId == id && !x.IsDeleted).ToList();
                var chargeGroupsDetails = _db.GlobalChargeGroup.Where(x => x.Id != 0).ToList();


                List<ChargeGroupDTO> chargeGroupDTOList = new List<ChargeGroupDTO>();

                foreach ( var item in allChargeGroups )
                {
                    ChargeGroupDTO chargeGroupDto = new ChargeGroupDTO();
                    chargeGroupDto.ChargeGroupName = chargeGroupsDetails.Where(x => x.Id == item.ChargeGroupId).FirstOrDefault().ChargeGroupName;
                    chargeGroupDto.FormName = forms.Where(x => x.FormId == item.FormId).FirstOrDefault().FormName;
                    chargeGroupDto.Id = item.Id;
                    chargeGroupDto.ChargeGroupId = item.ChargeGroupId;
                    chargeGroupDto.FormId = item.FormId;
                    chargeGroupDTOList.Add(chargeGroupDto);
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = chargeGroupDTOList
                });
            }
            catch ( System.Exception ex )
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetAlertForms")]
        public IActionResult GetAlertForms()
        {
            try
            {
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = Alertforms
                });
            }
            catch (System.Exception ex)
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("GetForms")]
        public IActionResult GetForms()
        {
            try
            {
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = forms
                });
            }
            catch ( System.Exception ex )
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPost]
        [Route("AddForms")]
        public IActionResult AddForms( FormsChargeGroupRequestDto reqBody )
        {
            try
            {
                if ( !ModelState.IsValid || reqBody is null )
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                    });
                }

                if(reqBody is not null )
                {
                    var existingChargeGroups = _db.FormsChargeGroup.Where(x => x.FormId == reqBody.FormId).ToList();
                    _db.RemoveRange(existingChargeGroups);
                    _db.SaveChanges();

                    foreach ( var item in reqBody.ChargeGroupIds )
                    {
                        FormsChargeGroup model = new FormsChargeGroup();
                        model.FormId = reqBody.FormId;
                        model.ChargeGroupId = item;
                        model.LastModified = DateTime.Now;
                        model.CreatedOn = DateTime.Now;
                        model.IsActive = true;
                        model.IsDeleted = false;
                        _db.Add(model);
                       _db.SaveChanges();
                    }
                }
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success"
                });
            }
            catch ( System.Exception ex )
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpGet]
        [Route("DeleteForms")]
        public IActionResult DeleteForms(int id)
        {
            try
            {
                var forms =_db.FormsChargeGroup.Where(x=>x.Id== id).ToList();
                
                if ( forms == null )
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = "Error",
                    });
                }

                else
                {
                    _db.FormsChargeGroup.RemoveRange(forms);
                    _db.SaveChanges();
                }

                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.Success,
                    Message = "Success",
                    Data = forms
                });

            }

            catch ( System.Exception ex )
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

        [HttpPut]
        [Route("UpdateForms")]
        public IActionResult UpdateForms( ChargeGroupDTO reqBody,int formId )
        {
            try
            {
                if ( !ModelState.IsValid || reqBody is null )
                {
                    return Ok(new ApiResponse<object>
                    {
                        Code = ResponseCode.BadRequest,
                        Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))
                    });
                }

                if ( reqBody is not null && formId != 0 )
                {
                    var existingForms = _db.FormsChargeGroup.Where(x => !x.IsDeleted);

                    var existingFormDetail = existingForms.Where(x=>x.Id==reqBody.Id && !x.IsDeleted).FirstOrDefault();

                    var existingFormChargeGroup = existingForms.Where(x => x.FormId == reqBody.FormId && reqBody.ChargeGroupId==x.ChargeGroupId).FirstOrDefault();

                    if ( existingFormChargeGroup != null )
                    {
                        var chargeGroupName = _db.GlobalChargeGroup.Where(x => x.Id == existingFormChargeGroup.ChargeGroupId).FirstOrDefault();

                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.BadRequest,
                            Message = "The selected form already have " + chargeGroupName.ChargeGroupName + " charge group."
                        });
                    }
                    if( existingFormDetail != null )
                    {
                        existingFormDetail.ChargeGroupId = reqBody.ChargeGroupId;
                        existingFormDetail.FormId = reqBody.FormId;
                        existingFormDetail.LastModified = DateTime.Now;
                        _db.Update(existingFormDetail);
                        _db.SaveChanges();

                        return Ok(new ApiResponse<object>
                        {
                            Code = ResponseCode.Success,
                            Message = "Success"
                        });
                    }
                }
                return Ok(new ApiResponse<object>
                {
                    Code = ResponseCode.RecordNotFound,
                    Message = "Form not found"
                });
            }
            catch ( System.Exception ex )
            {
                return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
            }
        }

    }
}
