namespace PulseLogicHMR.ServerModel
{
    public class CreateVerificationDto
    {
        public int EmployeeId { get; set; }
        public int CurrentVerificationLevel { get; set; }
        public int CurrentVerificationLevelCount { get; set; }
        public int MaxVerificationLevel { get; set; }
        public string VerificationStatus { get; set; }
        public string VerificationRemarks { get; set; }
        public int? ParentVerificationId { get; set; }
    }
}
