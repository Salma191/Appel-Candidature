import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxSelectDropdownComponent, SelectDropDownModule } from 'ngx-select-dropdown';
import { ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { provideHttpClient, withFetch } from '@angular/common/http';


@NgModule({
  declarations: [AppComponent], 
  imports: [
    NgxSelectDropdownComponent,
    CommonModule,
    ReactiveFormsModule,
    SelectDropDownModule,
  ],
  providers: [
    provideHttpClient(withFetch())  // Correct way to enable fetch in HttpClient
  ],
})
export class AppModule { }

