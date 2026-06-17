using System;
using Microsoft.AspNetCore.Mvc;
using PulseLogicHMR.ServerModel;
using PulseLogicHMR.DalLayer;
using Microsoft.Extensions.Options;
using PulseLogicHMR.Core.Configuration;

namespace PulseLogicHMR.Controllers.Clinical
{
    public class ScribeController : CommonController
    {
        public ScribeController(IOptions<MyConfiguration> _config) : base(_config)
        {
        }

        /// <summary>
        /// Mock AI Scribe API for parsing voice/text to structured clinical data.
        /// </summary>
        [HttpPost]
        [Route("api/Scribe/Transcribe")]
        public IActionResult Transcribe([FromBody] dynamic payload)
        {
            try
            {
                string text = payload.text;
                
                // Simulate NLP processing to find symptoms and ICD10 mapping
                string mappedIcdCode = "J02.9";
                string mappedIcdDesc = "Acute pharyngitis, unspecified";
                string structuredNote = "Patient presents with " + text + ". AI processed.";

                // Simple mock logic based on text
                if (text != null && text.ToString().ToLower().Contains("fever"))
                {
                    mappedIcdCode = "R50.9";
                    mappedIcdDesc = "Fever, unspecified";
                }

                return Ok(new 
                { 
                    Status = "OK", 
                    Results = new { 
                        ICD10Code = mappedIcdCode, 
                        ICD10Description = mappedIcdDesc,
                        StructuredNote = structuredNote 
                    } 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = "Error", ErrorMessage = ex.Message });
            }
        }
    }
}
