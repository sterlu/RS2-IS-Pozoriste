import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE, MatNativeDateModule } from '@angular/material/core';
import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';
import { MatIconModule } from '@angular/material/icon';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PredstaveComponent } from './predstave/predstave.component';
import { NavSidebarComponent } from './nav-sidebar/nav-sidebar.component';
import { PredstavaFormComponent } from './predstava-form/predstava-form.component';
import { PredstavaComponent } from './predstave/predstava/predstava.component';
import { ServiceWorkerModule } from "@angular/service-worker";
import { environment } from '../environments/environment';
import { IsAdminDirective } from '../helpers/is-admin.directive';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { MAT_MOMENT_DATE_FORMATS, MomentDateAdapter } from "@angular/material-moment-adapter";
import { KorpaComponent } from './korpa/korpa.component';
import { PredstavaPageComponent } from './predstava-page/predstava-page.component';
import { MatTableModule } from "@angular/material/table";
import { MatBadgeModule } from "@angular/material/badge";
import { TokenInterceptor } from "../helpers/token.interceptor";
import { GotovoPlacanjeComponent } from './gotovo-placanje/gotovo-placanje.component';

@NgModule({
  declarations: [
    AppComponent,
    PredstaveComponent,
    NavSidebarComponent,
    PredstavaFormComponent,
    PredstavaComponent,
    IsAdminDirective,
    RegisterComponent,
    LoginComponent,
    KorpaComponent,
    PredstavaPageComponent,
    GotovoPlacanjeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatCardModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    NgxMaterialTimepickerModule,
    MatIconModule,
    MatTableModule,
    MatBadgeModule,
    ServiceWorkerModule.register('ngsw-worker.js', {
      // enabled: environment.production,
      enabled: true,
      // Register the ServiceWorker as soon as the app is stable
      // or after 30 seconds (whichever comes first).
      registrationStrategy: 'registerWhenStable:30000'
    }),
  ],
  providers: [
    { provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
    { provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS },
    { provide: MAT_DATE_LOCALE, useValue: 'sr-sp' },
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
