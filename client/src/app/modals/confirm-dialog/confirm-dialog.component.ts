import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
    selector: 'app-confirm-dialog',
    templateUrl: './confirm-dialog.component.html',
    styleUrls: ['./confirm-dialog.component.css']
})
export class ConfirmDialogComponent implements OnInit {
    public title = "";
    public message = "";
    public btnOkText = "";
    public btnCancelText = "";
    public result = false;

    constructor(public bsModalRef: BsModalRef) { }

    ngOnInit(): void {
    }

    confirm() { 
        this.result = true;
        this.bsModalRef.hide();
    }

    decline() { 
        this.bsModalRef.hide();
    }

}
