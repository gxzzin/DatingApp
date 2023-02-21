import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { Member } from '../_models/member';
import { MembersService } from '../_services/members.service';

@Injectable({
    providedIn: 'root'
})
export class MemberDetailedResolver implements Resolve<Member | null> {

    constructor(private memberService: MembersService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<Member | null> {
        const userName = route.paramMap.get('userName');
        if (userName)
            return this.memberService.getMember(userName);
        else
            return of(null);
    }

}