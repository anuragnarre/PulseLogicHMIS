import { Injectable } from '@angular/core';
import {
  HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { MessageboxService } from '../../shared/messagebox/messagebox.service';
import { ENUM_MessageBox_Status } from '../../shared/shared-enums';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private msgBoxServ: MessageboxService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = '';
        if (error.error instanceof ErrorEvent) {
          // client-side error
          errorMessage = `Error: ${error.error.message}`;
        } else {
          // server-side error
          errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
        }
        
        // Use the application's MessageboxService to show the error
        this.msgBoxServ.showMessage(ENUM_MessageBox_Status.Error, [errorMessage]);
        
        return throwError(() => new Error(errorMessage));
      })
    );
  }
}
