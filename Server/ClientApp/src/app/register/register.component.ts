import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AccountService } from "../../services/account.service";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  loading = false;
  submitted = false;
  greska = '';

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private accountService: AccountService,
  ) {
    if (this.accountService.currentUser) {
      this.router.navigate(['/']);
    }
  }

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      username: ['', Validators.required],
      email: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(6)]],
      emailObavestenja: ['', []],
    });
  }

  get f() { return this.registerForm.controls; }

  /**
   * Registracija korisnika.
   */
  onSubmit() {
    this.submitted = true;
    this.greska = '';

    if (this.registerForm.invalid) return;

    this.loading = true;
    this.accountService.register(this.registerForm.value)
      .pipe(first())
      .subscribe(() => {
        this.router.navigate(['/']);
      },
      (error) => {
        this.loading = false;
        this.greska = error.error || error.message;
      });
  }
}
