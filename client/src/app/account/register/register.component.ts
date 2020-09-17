import { Component, OnInit } from '@angular/core';
import { AsyncValidatorFn, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { of, timer } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  errors: string[];

  constructor(private fb: FormBuilder, private accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
    this.createRegisterform();
  }

  // tslint:disable-next-line: typedef
  createRegisterform(){
    this.registerForm = this.fb.group({
      displayName: [null, [Validators.required]],
      email: [null, [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')],   // these are the synchronous validators
                    [this.validateEmailNotTaken()]],                    // these are the Asynchronous valdiators and these are only called if the Synchronous valiators have passed validation
      password: [null, [Validators.required]]
    });
  }

  // tslint:disable-next-line: typedef
  onSubmit(){
    this.accountService.register(this.registerForm.value).subscribe(() => {
      this.router.navigateByUrl('/shop');
    }, error => {
      this.errors = error.errors;
    });
  }

  // tslint:disable-next-line: typedef
  validateEmailNotTaken(): AsyncValidatorFn {
    return control => {
      return timer(500).pipe(     // because we don't want to keep making requests to te server every time the user types a character, we have added timer(500) which will add a small delay before we go and check with the server
        switchMap(() => {         // what we need access to is we want to return the inner Observable and return it to our control which is the outer Observable and there's a special operator we can use to do this : switchMap()
          if (!control.value){    // we here check if our control doesn't have a value, then we return of() which is an Observable of sth
            return of(null);
          }                       // and we do ahve sth in our control and there has been a delay of 500ms, then we want to make the request to the API
          return this.accountService.checkEmailExists(control.value).pipe(
            map(res => {
              return res ? {emailExists: true} : null;    // emailExists this is just a name we can make up ourselves to call the validator just like Validators.pattern or Validators.required, we are calling our one emailExists so that when we use this inside our template this is what we want to refer to to check if our validator has failed
            })    // validation fails and the email exists so show the error, that's why we make this {emailExists: true} otherwise return null
          );
        })
      );
    };
  }

}
