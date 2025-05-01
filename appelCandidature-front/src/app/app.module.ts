import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { HttpClientModule, provideHttpClient, withFetch } from '@angular/common/http';


@NgModule({
  declarations: [AppComponent], 
  imports: [
    CommonModule,
    ReactiveFormsModule,
  ],
  providers: [
  ],
})
export class AppModule { }

