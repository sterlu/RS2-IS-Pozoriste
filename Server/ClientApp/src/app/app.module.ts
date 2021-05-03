import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PredstaveComponent } from './predstave/predstave.component';
import { NavSidebarComponent } from './nav-sidebar/nav-sidebar.component';
import { PredstavaFormComponent } from './predstava-form/predstava-form.component';

@NgModule({
  declarations: [
    AppComponent,
    PredstaveComponent,
    NavSidebarComponent,
    PredstavaFormComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
