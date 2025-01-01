
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyWebAPI.Context;
using MyWebAPI.Models;
using System.Text.Json;
using Azure.Core;

namespace MyWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrugsController : ControllerBase
    {   
        private readonly DrugDbContext _context;

        public DrugsController(DrugDbContext context)
        {
            _context = context;
        }

        // GET /api/drugs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Drug>>> GetDrugs()
        {
            return await _context.Drugs
                .GroupBy(d => d.DrugName)           // Group by drug name
                .Select(g => g.FirstOrDefault())    // Select first record from each group
                .ToListAsync();
        }

        // GET /api/drugs/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Drug>> GetDrug(int id)
        {
            var drugData = await _context.Drugs.FindAsync(id);

            if (drugData == null)
            {
                return NotFound();
            }

            // Retrieve unique insurance NDCs for the same DrugName
            var uniqueNDCS = await _context.Drugs
                .AsNoTracking() // Improve performance for read-only queries
                .Where(d => d.DrugName == drugData.DrugName) // Filter by DrugName
                .GroupBy(d => d.NDC) // Group by NDC to ensure uniqueness
                .Select(g => g.FirstOrDefault()) // Select the first record for each unique NDC
                .ToListAsync(); // Execute query asynchronously


            var uniqueInsurances = await _context.Prescriptions
                .Where(s => s.DrugName == drugData.DrugName)
                .Select(s => s.Ins)
                .Distinct()
                .ToListAsync();

            // Combine the data into an object
            var result = new
            {
                insurances = uniqueInsurances,
                //drugData = drugData,
                UniqueNDCs = uniqueNDCS
            };

            // Return the combined data
            return Ok(result);
        }


        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Drug>>> SearchDrugs(
            [FromQuery] int? id = null,
            [FromQuery] string? ndc = null,
            [FromQuery] string? insurance = null)
        {

            if (!id.HasValue )
            {
                return Ok(ndc );
                //return BadRequest("Both ID and NDC are required for this search.");
            }

            // Query the database for drugs
            var query = _context.Drugs.AsQueryable();
            var drugData = await _context.Drugs
                            .FindAsync(id);


            var Drugname = drugData.DrugName;
            var DrugClass = drugData.DrugClass;

            var prescriptionQuery = _context.Prescriptions.AsQueryable();
            prescriptionQuery = prescriptionQuery.Where(p =>
                p.DrugName == Drugname && p.NDC == ndc);

            if (!string.IsNullOrEmpty(insurance))
            {
                prescriptionQuery = prescriptionQuery.Where(p => p.Ins == insurance);

            }

            var prescriptions = await prescriptionQuery.ToListAsync();

            if (!prescriptions.Any())
            {
                return NotFound("No prescriptions found matching the specified criteria.");
            }

            // Return the results
            return Ok(prescriptions);

        }









        //[HttpPost("alternatives")]
        //public async Task<IActionResult> GetAlternatives([FromBody] JsonElement request)
        //{
        //    //return Ok(request.qwdqwdqwd);


        //    if (string.IsNullOrEmpty(request.drugName))
        //    {
        //        return BadRequest("DrugName (ID) is required.");
        //    }


        //    // Step 1: Fetch the full drug details from Drugs entity
        //    var drugData = await _context.Drugs
        //        .FindAsync(request.drugName);

        //    if (drugData == null)
        //    {
        //        return NotFound("Drug not found.");
        //    }

        //    // Extract drug name and class
        //    string drugName = (string)drugData.DrugName;
        //    string drugClass = (string)drugData.DrugClass;
        //    string insurance = request.insurance;

        //    // Step 3: Search the Prescriptions table using name and class and insurance if it's exists
        //    var query = _context.Prescriptions.AsQueryable();
        //    query = query.Where(p => p.DrugName == drugName && p.Class == drugClass);

        //    if (!string.IsNullOrEmpty(insurance))
        //    {
        //        query = query.Where(p => p.Ins == insurance);
        //    }

        //    var matchedPrescriptions = await query.ToListAsync();
        //    return Ok(matchedPrescriptions);

        //}


    }
}
