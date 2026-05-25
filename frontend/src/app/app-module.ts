import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing-module';
import { NgxMaskDirective, NgxMaskPipe, provideNgxMask } from 'ngx-mask';
import { FormsModule } from '@angular/forms';

import { App } from './app';
import { Cadastro } from './components/cadastro/cadastro';
import { Lista } from './components/lista/lista';
import { Popup } from './components/popup/popup';

@NgModule({
  declarations: [App],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgxMaskDirective,
    NgxMaskPipe,
    FormsModule,
    Cadastro,
    Lista,
    Popup
  ],
  providers: [provideBrowserGlobalErrorListeners(), provideNgxMask()],
  bootstrap: [App],
})
export class AppModule {}
