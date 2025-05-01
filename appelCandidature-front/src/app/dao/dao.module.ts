import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DaoRoutingModule } from './dao-routing.module';
import { SafeUrlPipe } from '../safe-url.pipe';


@NgModule({
  declarations: [
  ],
  imports: [
    CommonModule,
    DaoRoutingModule,
    SafeUrlPipe
  ]
})
export class DaoModule { }
