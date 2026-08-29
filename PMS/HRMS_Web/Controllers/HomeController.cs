using HRMS_Web.Extensions;
using HRMS_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HRMS_Web.Controllers
{
    [SessionAuthorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View("Index");
        }
      
        public IActionResult Block()
        {
            return View();
        }
       
        public IActionResult SurchargeSetup()
        {
            return View();
        }
        public IActionResult MemberCategory()
        {
            return View();
        }
        public IActionResult Almt()
        {
            return View();
        }
        public IActionResult Quota()
        {
            return View();
        }
       
        public IActionResult Floor()
        {
            return View();
        }
        public IActionResult Sector()
        {
            return View();
        }
        public IActionResult Rank()
        {
            return View();
        }
        public IActionResult Force()
        {
            return View();
        }
        public IActionResult Postfix()
        {
            return View();
        }
        public IActionResult Prefix()
        {
            return View();
        }
        public IActionResult UOMDef()
        {
            return View();
        }
        public IActionResult PropertyType()
        {
            return View();
        }
        public IActionResult propertyList()
        {
            return View();
        }
        public IActionResult SocialStatus ()
        {
            return View();
        }
        public IActionResult FeaturesDef()
        {
            return View();
        }
        public IActionResult PhaseDef()
        {
            return View();
        }
        public IActionResult VerificationType()
        {
            return View();
        }
        public IActionResult Unitofmeasure()
        {
            return View();
        }
        
        public IActionResult Nature()
        {
            return View();
        }
        public IActionResult Category()
        {
            return View();
        }
        public IActionResult Finishes()
        {
            return View();
        }     
        public IActionResult RegistrationNPD()
        {
            return View();
        }

        public IActionResult Project()
        {
            return View();
        }
        public IActionResult RealEstate()
        {
            return View();
        }
        public IActionResult ConstructionStage()
        {
            return View();
        }
        public IActionResult StockCreation()
        {
           
            return View();
        }
        public IActionResult ConstructionM(int? id)
        {
            ViewBag.Id = id;
            return View();
        }
        public IActionResult ConstructionSecurity()
        {
            return View();
        }
        public IActionResult MeterialTesting()
        {
            return View();
        }
        public IActionResult DemarcationRequest()
        {
            return View();
        }
        public IActionResult ClearanceForm()
        {
            return View();
        } 
        public IActionResult LDAPlotNo()
        {
            return View();
        }
        public IActionResult SitePlan()
        {
            return View();
        }
        public IActionResult DemarcChargesI()
        {
            return View();
        }
        public IActionResult Demarcation()
        {
            return View();
        }
        public IActionResult DemarcationCharges()
        {
            return View();
        }
        public IActionResult PropertyProfile()
        {
            return View();
        }
        public IActionResult PossessionAnnouncement()
        {
            return View();
        }
        public IActionResult RegistrationNoProfile()
        {
            return View();
        }
        public IActionResult MapApproval(int? id)
        {
            ViewBag.Id = id;
            return View();
        }
        public IActionResult ReDesign()
        {
            return View();
        }

        public IActionResult PropertyBinding()
        {
            return View();
        }

        public IActionResult MapDesign()
        {
            return View();
        }

        public IActionResult StockCreationSetup()
        {
            return View();
        }
        public IActionResult FingurePrint()
        {
            return View();
        }
        public IActionResult PropertySetup()
        {
            return View();
        }

        public IActionResult Clearance()
        {
            return View();
        }
      
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}