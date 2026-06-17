import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { SharedModule } from "../../shared/shared.module";
import { MarketingReferralBLService } from "./marketingreferral.bl.service";
import { MarketingReferralDLService } from "./marketingreferral.dl.service";



@NgModule({
    providers: [
        MarketingReferralDLService,
        MarketingReferralBLService],
    imports: [
        ReactiveFormsModule,
        FormsModule,
        CommonModule,
        
        SharedModule],
    declarations: [

        //MarketingReferralTransactionComponent

    ],
    exports: [
    ],
    bootstrap: []
})
export class MarketingreferralSharedModule { }

