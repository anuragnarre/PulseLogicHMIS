import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { PulseLogicAutoCompleteComponent } from './pulselogic-auto-complete.component';
import { PulseLogicAutoCompleteDirective } from './pulselogic-auto-complete.directive';
import { PulseLogicAutoComplete } from './pulselogic-auto-complete';

@NgModule({
  imports: [CommonModule, FormsModule],
  declarations: [
   PulseLogicAutoCompleteComponent, 
    PulseLogicAutoCompleteDirective
  ],
  exports:  [
    PulseLogicAutoCompleteComponent,
     PulseLogicAutoCompleteDirective
    ],
  entryComponents: [PulseLogicAutoCompleteComponent]
})
export class PulseLogicAutoCompleteModule {
  static forRoot() {
    return {
        ngModule: PulseLogicAutoCompleteModule,
      providers: [PulseLogicAutoComplete]
    }
  }
}


