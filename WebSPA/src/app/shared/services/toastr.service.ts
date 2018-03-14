import { Injectable } from '@angular/core'
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ToastrServices {

    constructor(private _toastr: ToastrService) { }

    success(message: string, title?: string) {
        this._toastr.success(message, title);
    }

    info(message: string, title?: string) {
        this._toastr.info(message, title);
    }

    warning(message: string, title?: string) {
        this._toastr.warning(message, title);
    }

    error(message: string, title?: string) {
        this._toastr.error(message, title);
    }


}