using PulseLogicHMR.Services;
using PulseLogicHMR.Services.Incentive;
using PulseLogicHMR.Services.Admission;
using PulseLogicHMR.Services.Clinical;
using PulseLogicHMR.Services.Billing;
using PulseLogicHMR.Services.ClaimManagement;
using PulseLogicHMR.Services.Dispensary;
using PulseLogicHMR.Services.DispensaryTransfer;
using PulseLogicHMR.Services.DynamicTemplates;
using PulseLogicHMR.Services.IMU;
using PulseLogicHMR.Services.Inventory.InventoryDonation;
using PulseLogicHMR.Services.LIS;
using PulseLogicHMR.Services.MarketingReferral;
using PulseLogicHMR.Services.Maternity;
using PulseLogicHMR.Services.Medicare;
using PulseLogicHMR.Services.Pharmacy.PharmacyPO;
using PulseLogicHMR.Services.Pharmacy.Rack;
using PulseLogicHMR.Services.Pharmacy.SupplierLedger;
using PulseLogicHMR.Services.ProcessConfirmation;
using PulseLogicHMR.Services.ProvisionalDischarge;
using PulseLogicHMR.Services.QueueManagement;
using PulseLogicHMR.Services.SSF;
using PulseLogicHMR.Services.Utilities;
using PulseLogicHMR.Services.Vaccination;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PulseLogicHMR.Core.Billing.PaymentGateway;

namespace PulseLogicHMR.DependencyInjection
{
    /*
        Author: Krishna, 
        Creation: 19thMay'23
        Purpose: 1. Make Startup Clean and Readable
                2. To Achieve Separation of Concerns
                3. Startup class was getting bulky, which leads to less  maintainable code.
                4. DI requires registration of services in the Startup.cs
                5. The number of registrations will increase and become cumbersome to maintain Startup.cs
    */
    public static class PulseLogicServicesExtensions
    {
        public static IServiceCollection AddPulseLogicServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            // Register IMessagingService (Twilio Implementation)
            services.AddTransient<PulseLogicHMR.Core.Utilities.Messaging.IMessagingService, PulseLogicHMR.Core.Utilities.Messaging.TwilioMessagingService>();
            
            // Register FHIR Mapping Service
            services.AddTransient<PulseLogicHMR.Services.FHIR.IFHIRMappingService, PulseLogicHMR.Services.FHIR.FHIRMappingService>();

            services.AddTransient<IRackService, RackService>();
            services.AddTransient<IInventoryCompanyService, InventoryCompanyService>();
            services.AddTransient<IDesignationService, DesignationService>();
            services.AddTransient<IInventoryReceiptNumberService, InventoryReceiptNumberService>();
            services.AddTransient<IInventoryGoodReceiptService, InventoryGoodReceiptService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IFractionPercentService, FractionPercentService>();
            services.AddTransient<IFractionCalculationService, FractionCalculationService>();
            services.AddTransient<IVerificationService, VerificationService>();
            services.AddTransient<IDispensaryService, DispensaryService>();
            services.AddTransient<IDispensaryRequisitionService, DispensaryRequisitionService>();
            services.AddTransient<IMaternityService, MaternityService>();
            services.AddTransient<IDispensaryTransferService, DispensaryTransferService>();
            services.AddTransient<IActivateInventoryService, ActivateInventoryService>();
            services.AddTransient<IPharmacyPOService, PharmacyPOService>();
            services.AddTransient<IVaccinationService, VaccinationService>();
            services.AddTransient<ICssdItemService, CssdItemService>();
            services.AddTransient<ICssdReportService, CssdReportService>();
            services.AddTransient<INepaliReceiptService, NepaliReceiptService>();
            services.AddTransient<ISupplierLedgerService, SupplierLedgerService>();
            services.AddTransient<IFileUploadService, GoogleDriveFileUploadService>();
            services.AddTransient<ILISService, LISService>();
            services.AddTransient<IQueueManagementService, QueueManagementService>();
            services.AddTransient<IDonationService, DonationService>();
            services.AddTransient<IIMUService, IMUService>();
            services.AddTransient<ISSFService, SSFService>();
            services.AddTransient<IMedicareService, MedicareService>();
            services.AddTransient<IBillingMasterService, BillingMasterService>();
            services.AddTransient<IClaimManagementService, ClaimManagementService>();
            services.AddTransient<IUtilitiesService, UtilitiesService>();
            services.AddTransient<IProcessConfirmationService, ProcessConfirmationService>();
            services.AddTransient<IAdmissionMasterService, AdmissionMasterService>();
            services.AddTransient<IMarketingReferralService, MarketingReferralService>();
            services.AddTransient<IProvisionalDischargeService, ProvisionalDischargeService>();
            services.AddTransient<IDynamicTemplateService, DynamicTemplateService>();
            services.AddTransient<IClinicalVitalsService, ClinicalVitalsService>();
            services.AddTransient<IClinicalMedicationService, ClinicalMedicationService>();
            services.AddTransient<IPaymentGatewayService, RazorpayPaymentService>();

            // Task 1 — Incentive module service (refactored from IncentiveController God Class)
            services.AddTransient<IIncentiveService, IncentiveService>();

            return services;
        }
    }
}
