/*
 File: CommonTypes.cs
 created: 28Jan'17-developer
 description: this class contains all type classes which are to be used commonly across the appliccation
 remarks: 
 -------------------------------------------------------------------
 change history:
 -------------------------------------------------------------------
 S.No     ModifiedBy/Date             description           remarks
 -------------------------------------------------------------------
 1.       developer/28Jan'17          created           -- added public class PulseLogicHTTPResponse<T>

 -------------------------------------------------------------------
 */


namespace PulseLogicHMR.CommonTypes
{
    public class PulseLogicHTTPResponse<T>
    {
        public T Results { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }

        public PulseLogicHTTPResponse()
        {
            this.Status = string.Empty;
            this.ErrorMessage = string.Empty;
        }

        public static PulseLogicHTTPResponse<T> FormatResult(T results)
        {
            return new PulseLogicHTTPResponse<T>() { Results = results };
        }

        public static PulseLogicHTTPResponse<T> FormatResult(T results, string status)
        {
            return new PulseLogicHTTPResponse<T>() { Status = status, Results = results };
        }

        public static PulseLogicHTTPResponse<T> FormatResult(T results, string status, string errorMessage)
        {
            return new PulseLogicHTTPResponse<T>() { Status = status, Results = results, ErrorMessage = errorMessage };
        }

    }
}
