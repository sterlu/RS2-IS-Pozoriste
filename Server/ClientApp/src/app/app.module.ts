import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';
import { MatIconModule } from '@angular/material/icon';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PredstaveComponent } from './predstave/predstave.component';
import { NavSidebarComponent } from './nav-sidebar/nav-sidebar.component';
import { PredstavaFormComponent } from './predstava-form/predstava-form.component';
import { PredstavaComponent } from './predstave/predstava/predstava.component';
import {ServiceWorkerModule} from "@angular/service-worker";
import { environment } from '../environments/environment';
import { KupovinaKarteComponent } from './kupovina-karte/kupovina-karte.component';

@NgModule({
  declarations: [
    AppComponent,
    PredstaveComponent,
    NavSidebarComponent,
    PredstavaFormComponent,
    PredstavaComponent,
    KupovinaKarteComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
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
    ServiceWorkerModule.register('ngsw-worker.js', {
      // enabled: environment.production,
      enabled: true,
      // Register the ServiceWorker as soon as the app is stable
      // or after 30 seconds (whichever comes first).
      registrationStrategy: 'registerWhenStable:30000'
    }),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
