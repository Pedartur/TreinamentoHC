import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { Cadastro } from './components/cadastro/cadastro';
import { Lista } from './components/lista/lista';
import {NgxMaskDirective, NgxMaskPipe, provideNgxMask} from 'ngx-mask';
import { FormsModule } from '@angular/forms';   

@NgModule({
  declarations: [App, Cadastro, Lista],
  imports: [BrowserModule, AppRoutingModule, NgxMaskDirective, NgxMaskPipe, FormsModule],
  providers: [provideBrowserGlobalErrorListeners(), provideNgxMask()],
  bootstrap: [App],
})
export class AppModule {}
