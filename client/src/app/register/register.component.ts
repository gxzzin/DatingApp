import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, UntypedFormBuilder, FormControl, UntypedFormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
    @Output() cancelRegister = new EventEmitter();
    registerForm!: UntypedFormGroup;
    maxDate!: Date;
    validationErrors: string[] = [];

    constructor(private accountServive: AccountService
        , private toaster: ToastrService
        , private fb: UntypedFormBuilder
        , private router: Router) { }

    ngOnInit(): void {
        this.initializeForm();
        this.maxDate = new Date();
        this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
    }


    initializeForm() {
        this.registerForm = this.fb.group({
            gender: ["male"],
            userName: ["", Validators.required],
            knownAs: ["", Validators.required],
            dateOfBirth: ["", Validators.required],
            city: ["", Validators.required],
            country: ["", Validators.required],
            password: ["", [
                Validators.required,
                Validators.minLength(4),
                Validators.maxLength(8)
            ]],
            confirmPassword: ["", [Validators.required, this.matchValues("password")]]
        });

        this.registerForm.controls["password"].valueChanges.subscribe(() => {
            this.registerForm.controls["confirmPassword"].updateValueAndValidity();
        })
    }


    matchValues(matchTo: string): ValidatorFn {
        return (control: AbstractControl) => {
            const matchControl = (control.parent?.controls as { [key: string]: AbstractControl<any, any> })[matchTo];
            return control?.value === matchControl.value ? null : { isMatching: true };
        };
    }

    register() {
        this.accountServive.register(this.registerForm.value).subscribe(response => {
            this.router.navigateByUrl("/members");
        }, error => {
            this.validationErrors = error;
        });
    }

    cancel() {
        this.cancelRegister.emit(false);
    }

}
