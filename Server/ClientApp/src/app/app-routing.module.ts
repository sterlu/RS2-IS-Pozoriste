import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {PredstaveComponent} from './predstave/predstave.component';
import {PredstavaFormComponent} from './predstava-form/predstava-form.component';
import {KupovinaKarteComponent} from './kupovina-karte/kupovina-karte.component';

const routes: Routes = [
  // { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'predstave', component: PredstaveComponent },
  { path: 'predstava/dodaj', component: PredstavaFormComponent },
  { path: 'predstava/izmeni/:id', component: PredstavaFormComponent },
  { path: 'predstava/:id', component: KupovinaKarteComponent },
  // { path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthorizeGuard] },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
