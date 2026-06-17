import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // add authorization header with jwt token if available
        let currentUser = JSON.parse(localStorage.getItem('currentUser') || '{}');
        if (currentUser && currentUser.token) {
            request = request.clone({
                setHeaders: {
                    Authorization: `Bearer ${currentUser.token}`
                }
            });
        }

        // Add base URL and versioning if the request is an API call
        if (request.url.startsWith('/api/') && !request.url.startsWith('http')) {
            // Replace /api/ with /api/v1/ globally for versioning
            let urlWithVersion = request.url.replace(/^\/api\//, '/api/v1/');
            request = request.clone({
                url: `${window.location.origin}${urlWithVersion}`
            });
        } else if (!request.url.startsWith('http') && !request.url.startsWith('/')) {
            // If it's a relative API path like 'api/Patient/...' 
            if(request.url.startsWith('api/')){
                let urlWithVersion = request.url.replace(/^api\//, '/api/v1/');
                request = request.clone({
                    url: `${window.location.origin}${urlWithVersion}`
                });
            }
        }

        return next.handle(request);
    }
}
