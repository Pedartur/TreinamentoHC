import { Component } from '@angular/core';
import { Contato } from '../../models/contato';
import { ContatoService} from '../../services/contato-service';

@Component({
  selector: 'app-popup',
  standalone: false,
  templateUrl: './popup.html',
  styleUrl: './popup.scss',
})
export class Popup {
  DadoContato: Contato [] = [];

}
