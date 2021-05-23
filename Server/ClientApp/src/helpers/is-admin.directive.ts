import { Directive, OnDestroy, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { User } from "../models/user";
import { AccountService } from "../services/account.service";
import { Subject } from "rxjs";
import { takeUntil } from "rxjs/operators";

@Directive({
  selector: '[appIsAdmin]'
})
export class IsAdminDirective implements OnInit, OnDestroy {
  stop$ = new Subject();
  isVisible = false;

  constructor(
    private viewContainerRef: ViewContainerRef,
    private templateRef: TemplateRef<any>,
    private accountService: AccountService
  ) {}

  ngOnInit() {
    this.accountService.currentUser$
      .pipe(takeUntil(this.stop$))
      .subscribe(user => {
        if (user?.Tip === 'admin' && !this.isVisible) {
          this.isVisible = true;
          this.viewContainerRef.createEmbeddedView(this.templateRef);
        } else {
          if (this.isVisible) {
            this.isVisible = false;
            this.viewContainerRef.clear();
          }
        }
      });
  }

  ngOnDestroy() {
    this.stop$.next();
  }
}
