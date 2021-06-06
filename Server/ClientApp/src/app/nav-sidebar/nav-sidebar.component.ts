import { Component, OnInit } from '@angular/core';
import { AccountService } from "../../services/account.service";
import { User } from "../../models/user";
import {KorpaService} from "../../services/korpa.service";

@Component({
  selector: 'app-nav-sidebar',
  templateUrl: './nav-sidebar.component.html',
  styleUrls: ['./nav-sidebar.component.scss']
})
export class NavSidebarComponent {
  user: User;

  constructor(private accountService: AccountService, public korpaService: KorpaService) {
    accountService.currentUser$.subscribe((_user: User) => this.user = _user);
  }

  logout() {
    if (confirm('Da li želite da se odjavite?')) this.accountService.logout();
  }
}
