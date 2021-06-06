import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { AccountService } from "../../services/account.service";
import { first } from "rxjs/operators";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  loading = false;
  submitted = false;
  greska = '';

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private accountService: AccountService,
    private route: ActivatedRoute,
  ) {
    if (this.accountService.currentUser) {
      this.router.navigate(['/']);
    }
  }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  get f() { return this.loginForm.controls; }

  onSubmit() {
    this.submitted = true;
    this.greska = '';

    if (this.loginForm.invalid) return;

    this.loading = true;
    this.accountService.login(this.loginForm.value)
      .pipe(first())
      .subscribe(() => {
        this.router.navigate([this.route.snapshot.queryParamMap.get('returnUrl') || '/']);
      },
      (error) => {
        this.loading = false;
        this.greska = error.error || error.message;
      });
  }
}
