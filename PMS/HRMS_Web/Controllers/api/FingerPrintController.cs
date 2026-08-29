using B_DB_Context;
using B_DB_Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using SourceAFIS;
using System.Runtime.Intrinsics.X86;
using System;
using static iTextSharp.text.pdf.AcroFields;
using B_Utility.Common;

namespace HRMS_Web.Controllers
{
    public class FingerPrintController : Controller
    {
        private readonly DataBase_Context _context;

        public FingerPrintController(DataBase_Context context)
        {
            _context = context;
        }
        public IActionResult FingerPrint()
        {
            return View();
        }
        public IActionResult VerifyFingerPrint()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetRegisterFingers(int id)
        {

            var data = _context.MemberBioMetrics.Where(x => x.MemberProfileId == id).Select(x => new { x.FingerId, x.Finger }).ToList();

            return Ok(new ApiResponse<object>
            {
                Code = ResponseCode.Success,
                Message = "Success",
                Data = data
            });

        }

        [HttpPost]
        public IActionResult StoreFingerprintData([FromBody] List<StoreFingerDto> dto)
        {
            try
            {

                foreach (var item in dto)
                {

                    var existingData = _context.MemberBioMetrics.SingleOrDefault(x => x.MemberProfileId == item.MemberProfileId && x.FingerId == item.FingerId);

                    if (existingData == null)
                    {
                        var newBiometricData = new MemberBioMetric
                        {
                            MemberProfileId = item.MemberProfileId,
                            FingerId = item.FingerId,
                            EncodedBase64 = item.EncodedBase64,
                            CreatedOn = DateTime.Now,
                            CreatedBy = item.ModifiedBy,
                            LastModifiedUserName = item.LastModifiedUserName,
                            IsActive = true,
                            IsDeleted = false,
                        };

                        _context.MemberBioMetrics.Add(newBiometricData);
                    }
                    else
                    {
                        return Ok(false);
                    }
                }

                _context.SaveChanges();

                return Ok(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error storing fingerprint data: {ex.Message}");
                return Ok("something went wrong.");
            }
        }

        [HttpPost]
        public IActionResult Compare(string fingerData, int FingerId, int MemberProfileId, string VerificationType, string LastModifiedUserName, int ModifiedBy)
        {

            var storedData = _context.MemberBioMetrics.SingleOrDefault(x => x.MemberProfileId == MemberProfileId && x.FingerId == FingerId);

            if (storedData != null)
            {
                try
                {
                    double threshold = (double)_context.SAPOperations.FirstOrDefault().FingerPrintThreshhold;
                    string encodedbase64 = storedData.EncodedBase64;

                    byte[] fingerData1 = Convert.FromBase64String(fingerData);
                    byte[] fingerData2 = Convert.FromBase64String(encodedbase64);

                    var probe = new FingerprintTemplate(new FingerprintImage(fingerData1));
                    var candidate = new FingerprintTemplate(new FingerprintImage(fingerData2));

                    var matcher = new FingerprintMatcher(probe);
                    double similarity = matcher.Match(candidate);

                    bool matches = similarity >= threshold;

                    var memberBioMetricHistory = new MemberBioMetricHistery
                    {
                        MemberProfileId = MemberProfileId,
                        FingerId = FingerId,
                        VerificationType = VerificationType,
                        IsMatched = matches,
                        VerificationDateTime = DateTime.Now,
                        CreatedOn = DateTime.Now,
                        CreatedBy = ModifiedBy,
                        LastModifiedUserName = LastModifiedUserName,
                        IsActive = true,
                        IsDeleted = false,
                    };

                    _context.MemberBioMetricHistery.Add(memberBioMetricHistory);
                    _context.SaveChanges();

                    return Json(matches);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error comparing fingerprints: {ex.Message}");
                }
            }

            return Json(false);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFolder([FromForm] IFormFile folderZip)
        {
            try
            {
                if (folderZip == null || folderZip.Length == 0)
                {
                    return BadRequest("No folder was uploaded.");
                }

                string tempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(tempFolder);

                string zipFilePath = Path.Combine(tempFolder, folderZip.FileName);
                using (var fileStream = new FileStream(zipFilePath, FileMode.Create))
                {
                    await folderZip.CopyToAsync(fileStream);
                }

                string extractPath = Path.Combine(tempFolder, "Extracted");
                Directory.CreateDirectory(extractPath);

                System.IO.Compression.ZipFile.ExtractToDirectory(zipFilePath, extractPath);

                string[] files = Directory.GetFiles(extractPath, "*.*", SearchOption.AllDirectories);

                if (!files.Any())
                {
                    return BadRequest("The uploaded folder is empty or contains no valid files.");
                }

                foreach (string filePath in files)
                {
                    string fileName = Path.GetFileNameWithoutExtension(filePath);

                    string[] nameParts = fileName.Split('-');
                    if (nameParts.Length < 2)
                    {
                        return BadRequest($"Invalid file name format: {fileName}. Expected format: 'MemberId-FingerId'.");
                    }

                    string memberId = nameParts[0];
                    string fingerId = nameParts[1];

                    byte[] imageBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                    string base64String = Convert.ToBase64String(imageBytes);

                    var existingData = _context.MemberBioMetrics
                        .SingleOrDefault(x => x.MemberProfileId == Convert.ToInt32(memberId) && x.FingerId == Convert.ToInt32(fingerId));

                    if (existingData == null)
                    {
                        var newBiometricData = new MemberBioMetric
                        {
                            MemberProfileId = Convert.ToInt32(memberId),
                            FingerId = Convert.ToInt32(fingerId),
                            EncodedBase64 = base64String,
                            CreatedOn = DateTime.Now,
                            CreatedBy = 1,
                            LastModifiedUserName = "Super Admin",
                            IsActive = true,
                            IsDeleted = false,
                        };

                        _context.MemberBioMetrics.Add(newBiometricData);
                    }
                    else
                    {
                        existingData.EncodedBase64 = base64String;
                        existingData.LastModifiedUserName = "Super Admin";
                    }
                }

                await _context.SaveChangesAsync();

                Directory.Delete(tempFolder, true);

                return Ok("Folder uploaded and processed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the folder.");
            }
        }

        public class StoreFingerDto
        {
            public int MemberProfileId { get; set; }
            public int FingerId { get; set; }
            public string EncodedBase64 { get; set; }
            public int? ModifiedBy { get; set; }
            public string? LastModifiedUserName { get; set; }
        }
    }
}
