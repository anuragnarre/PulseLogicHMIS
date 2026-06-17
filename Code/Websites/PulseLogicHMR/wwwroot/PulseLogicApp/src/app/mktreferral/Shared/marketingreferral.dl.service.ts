import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { PulseLogicHTTPResponse } from "../../shared/common-models";
import { ReferralCommission_DTO } from "./DTOs/referral-commission.dto";
import { ReferringOrganization_DTO } from "./DTOs/referral-organization.dto";
import { ReferralParty_DTO } from "./DTOs/referral-party.dto";


@Injectable()
export class MarketingReferralDLService {
    public optionsJson = {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    constructor(public http: HttpClient) {

    }

    public GetInvoiceList(fromDate, toDate): Observable<PulseLogicHTTPResponse> {
        return this.http.get<PulseLogicHTTPResponse>(`/api/MarketingReferral/Invoices?FromDate=${fromDate}&ToDate=${toDate}`, this.optionsJson);
    }
    public GetMarketingReferralDetailReport(fromDate, toDate, ReferringPartyId): Observable<PulseLogicHTTPResponse> {
        return this.http.get<PulseLogicHTTPResponse>(`/api/MarketingReferral/MarketingreferralDetailReport?FromDate=${fromDate}&ToDate=${toDate}&ReferringPartyId=${ReferringPartyId}`, this.optionsJson);
    }
    public GetBillDetails(billTransactionId): Observable<PulseLogicHTTPResponse> {
        return this.http.get<PulseLogicHTTPResponse>(`/api/MarketingReferral/BillDetails?billTransactionId=${billTransactionId}`, this.optionsJson);
    }
    public GetReferralScheme(): Observable<PulseLogicHTTPResponse> {
        return this.http.get<PulseLogicHTTPResponse>("/api/MarketingReferral/ReferralScheme", this.optionsJson);
    }
    public GetReferringParty(): Observable<PulseLogicHTTPResponse> {
        return this.http.get<PulseLogicHTTPResponse>("/api/MarketingReferral/ReferringParty", this.optionsJson);
    }
    public GetReferringPartyGroup(): Observable<PulseLogicHTTPResponse> {
        return this.http.get<PulseLogicHTTPResponse>("/api/MarketingReferral/ReferringPartyGroup", this.optionsJson);
    }
    public GetReferringOrganization(): Observable<PulseLogicHTTPResponse> {
        return this.http.get<PulseLogicHTTPResponse>("/api/MarketingReferral/ReferringOrganization", this.optionsJson);
    }
    public GetAlreadyAddedCommission(BillingTransactionId): Observable<PulseLogicHTTPResponse> {
        return this.http.get<PulseLogicHTTPResponse>(`/api/MarketingReferral/AlreadyAddedCommission?BillingTransactionId=${BillingTransactionId}`, this.optionsJson);
    }
    public DeleteReferralCommission(ReferralCommissionId): Observable<PulseLogicHTTPResponse> {
        return this.http.delete<PulseLogicHTTPResponse>(`/api/MarketingReferral/ReferralCommission?ReferralCommissionId=${ReferralCommissionId}`, this.optionsJson);
    }
    public SaveNewReferral(referralComission_DTO: ReferralCommission_DTO): Observable<PulseLogicHTTPResponse> {
        return this.http.post<PulseLogicHTTPResponse>("/api/MarketingReferral/NewReferralComission", referralComission_DTO, this.optionsJson);
    }
    public SaveReferringOrganization(referringOrganization_DTO: ReferringOrganization_DTO): Observable<PulseLogicHTTPResponse> {
        return this.http.post<PulseLogicHTTPResponse>("/api/MarketingReferral/NewReferringOrganization", referringOrganization_DTO, this.optionsJson);
    }
    public SaveReferringParty(referralParty_DTO: ReferralParty_DTO): Observable<PulseLogicHTTPResponse> {
        return this.http.post<PulseLogicHTTPResponse>("/api/MarketingReferral/NewReferringParty", referralParty_DTO, this.optionsJson);
    }
    public UpdateReferringOrganization(referringOrganization_DTO: ReferringOrganization_DTO): Observable<PulseLogicHTTPResponse> {
        return this.http.put<PulseLogicHTTPResponse>("/api/MarketingReferral/ReferringOrganization", referringOrganization_DTO, this.optionsJson);
    }
    public UpdateReferringParty(referringParty_DTO: ReferralParty_DTO): Observable<PulseLogicHTTPResponse> {
        return this.http.put<PulseLogicHTTPResponse>("/api/MarketingReferral/ReferringParty", referringParty_DTO, this.optionsJson);
    }
    public ActivateDeactivateOrganization(selectedItem): Observable<PulseLogicHTTPResponse> {
        return this.http.put<PulseLogicHTTPResponse>("/api/MarketingReferral/ActivateDeactivateOrganization", selectedItem, this.optionsJson);
    }
    public ActivateDeactivateParty(selectedItem): Observable<PulseLogicHTTPResponse> {
        return this.http.put<PulseLogicHTTPResponse>("/api/MarketingReferral/ActivateDeactivateParty", selectedItem, this.optionsJson);
    }
}
