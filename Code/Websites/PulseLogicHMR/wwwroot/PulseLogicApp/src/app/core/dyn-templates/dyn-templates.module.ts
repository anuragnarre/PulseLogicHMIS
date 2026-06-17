import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DynamicTemplateEditComponent } from "./settings/dyn-template-edit.component"
import { QuestionAddComponent } from "./settings/question-add.component";
import { QuestionEditComponent } from "./settings/question-edit.component";
import { JsonTableComponent } from './controls/json-table.component';
import { QuestionComponent } from './controls/question.component';
import { QuestionHrcComponent } from './controls/question-hrc.component';
import { DynamicTemplateService } from './shared/dynamic-template-service';
//import { Ng2AutoCompleteModule } from 'ng2-auto-complete';
import { PulseLogicAutoCompleteModule } from '../../shared/pulselogic-autocomplete/pulselogic-auto-complete.module';
import { SharedModule } from '../../shared/shared.module'; 

@NgModule({
    providers: [DynamicTemplateService],
    imports: [
        CommonModule,
        
        FormsModule,
        //Ng2AutoCompleteModule
        PulseLogicAutoCompleteModule,
        SharedModule
    ],
    declarations: [
        QuestionEditComponent,
        QuestionAddComponent,
        DynamicTemplateEditComponent,
        JsonTableComponent,
        QuestionComponent,
        QuestionHrcComponent
    ],
    exports: [
        QuestionEditComponent,
        QuestionAddComponent,
        DynamicTemplateEditComponent,
        JsonTableComponent,
        QuestionComponent,
        QuestionHrcComponent
    ],
    bootstrap: []//do we need anything here ? <developer:2jan2017>
})
export class DynTemplateModule {

}
