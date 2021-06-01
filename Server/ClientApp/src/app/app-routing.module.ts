import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PredstaveComponent } from './predstave/predstave.component';
import { PredstavaFormComponent } from './predstava-form/predstava-form.component';
import { KupovinaKarteComponent } from './kupovina-karte/kupovina-karte.component';
import { AuthGuard } from "../helpers/auth.guard";
import { AdminGuard } from "../helpers/admin.guard";
import { LoginComponent } from "./login/login.component";
import { RegisterComponent } from "./register/register.component";

const routes: Routes = [
  { path: '', component: PredstaveComponent, pathMatch: 'full' },
  { path: 'predstave', component: PredstaveComponent },
  { path: 'predstava/dodaj', component: PredstavaFormComponent, canActivate: [AuthGuard, AdminGuard] },
  { path: 'predstava/izmeni/:id', component: PredstavaFormComponent, canActivate: [AuthGuard, AdminGuard] },
  { path: 'predstava/:id', component: KupovinaKarteComponent, canActivate: [AuthGuard] },

  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
