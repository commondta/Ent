using B_DB_Context;
using B_DB_Model;
using B_Utility.BLL;
using B_Utility.Common;
using HRMS_Web.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_Web.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserPermissionMappingController : ControllerBase
    {
        private readonly DataBase_Context _db;

        CommonBLL _commonBLL;
        ApprovalBLL _approvalBLL;
        public UserPermissionMappingController(DataBase_Context db)
        {
            _db = db;
            _approvalBLL = new ApprovalBLL(_db);
            _commonBLL = new CommonBLL(_db);
        }

        [HttpPost]
        [Route("SaveUserPermissions")]
        public IActionResult SaveUserPermisssions(List<UserPermissionMapping> userPermissions)
        {
            var userPermissionMapping = _db.UserPermissionMapping.Where(x => x.EMP_CODE == userPermissions[0].EMP_CODE)
                                                                 .ToList();
            _db.UserPermissionMapping.RemoveRange(userPermissionMapping);

            foreach (var userPermission in userPermissions)
            {
                userPermission.CreatedBy = userPermission.CreatedBy;
                userPermission.ModifiedBy = userPermission.ModifiedBy;
                userPermission.LastModifiedUserName = userPermission.LastModifiedUserName;
                userPermission.IsActive = true;
                userPermission.LastModified = DateTime.Now;
            }

            _db.UserPermissionMapping.AddRange(userPermissions);
            _db.SaveChanges();

            return Ok(new ApiResponse<object>
            {
                Code = ResponseCode.Success,
                Message = "Success",
                Data = null
            });
        }

        [HttpGet]
        [Route("GetUserPermissionAsync")]
        public  IActionResult GetUserPermissionAsync(int userId)
        {
            var userPermissions =   (from x in _db.PermissionForms
                                         join y in _db.UserPermissionMapping.Where(ee => ee.EMP_CODE == userId) on x.Id equals y.PermissionFormsId
                                         into xy
                                         from y in xy.DefaultIfEmpty()
                                         orderby x.SerialNo
                                         select new AllUserPermissionsDto
                                         {
                                             Id = x.Id,
                                             CanAdd = y == null ? false : y.CanAdd,
                                             CanDelete = y == null ? false : y.CanDelete,
                                             CanEdit = y == null ? false : y.CanEdit,
                                             CanView = y == null ? false : y.CanView,
                                             Name = x.Name
                                         }).ToList();

            return Ok(new ApiResponse<object>
            {
                Code = ResponseCode.Success,
                Message = "Success",
                Data = userPermissions
            });
        }

        //[HttpPost]
        //[Route("SaveUserPermissions")]
        //public IActionResult SaveUserPermissions(List<UserPermissionMapping> dto)
        //{
        //    try
        //    {
        //        if (dto.Count > 0)
        //        {
        //            int empCode = dto.Select(x => x.EMP_CODE).FirstOrDefault();

        //            var checkIsPermissionsExist = _db.UserPermissionMapping.Where(x => x.EMP_CODE == empCode).FirstOrDefault();

        //            if(checkIsPermissionsExist != null)
        //            {
        //                foreach(var item in dto)
        //                {
        //                    var existingPermissions = _db.UserPermissionMapping
        //                                                 .Where(x => x.PermissionFormsId == item.PermissionFormsId 
        //                                                          && x.EMP_CODE == item.EMP_CODE)
        //                                                 .FirstOrDefault();
        //                    if(existingPermissions != null)
        //                    {
        //                        existingPermissions.CanView = item.CanView;
        //                        existingPermissions.CanAdd = item.CanAdd;
        //                        existingPermissions.CanEdit = item.CanEdit;
        //                        existingPermissions.CanDelete = item.CanDelete;

        //                        _db.SaveChanges();
        //                    }
        //                    else
        //                    {
        //                        _db.UserPermissionMapping.Add(item);
        //                        _db.SaveChanges();
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                _db.UserPermissionMapping.AddRange(dto);
        //                _db.SaveChanges();
        //            }
        //        }
        //        return Ok(new ApiResponse<object>
        //        {
        //            Code = ResponseCode.Success,
        //            Message = "Success",
        //            Data = null
        //        });
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return Ok(UHelper.ApiExceptionResponse(ex.ToString()));
        //    }
        //}
    }
}
