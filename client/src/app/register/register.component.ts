import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
    @Output() cancelRegister = new EventEmitter();
    model: any = {};

    constructor(private accountServive: AccountService
        , private toaster: ToastrService) { }

    ngOnInit(): void {
    }

    register() {
        this.accountServive.register(this.model).subscribe(response => {
            console.log(response);
            this.cancel();
        }, error => {
            this.toaster.error(error.error);
        });
    }

    cancel() {
        this.cancelRegister.emit(false);
    }

}