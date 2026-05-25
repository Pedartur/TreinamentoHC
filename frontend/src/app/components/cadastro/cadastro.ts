import { ChangeDetectorRef, Component } from '@angular/core';
import { Contato } from '../../models/contato';
import { ContatoService} from '../../services/contato-service';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-cadastro',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './cadastro.html',
  styleUrl: './cadastro.scss',
})

export class Cadastro {

  constructor(private readonly contatoService: ContatoService, private readonly cdr: ChangeDetectorRef) {}

  novoContato: Contato = {
    id: -1,
    nome: '',
    telefone: '',
    email: '',
    cargo: ''
  };

  buttonText: string = 'Cadastrar';
  carregando: boolean = false;

  salvarContato(): void {
    if (this.carregando) return;

    if (!this.novoContato.nome || !this.novoContato.telefone || !this.novoContato.email || !this.novoContato.cargo) {
      alert('Por favor, preencha todos os campos');
      return;
    }

    this.buttonText = 'A salvar...';
    this.carregando = true;

    this.contatoService.create(this.novoContato).subscribe({
      next: (resultado) => {
        console.log('Salvo com sucesso!', resultado);
        alert('Contato cadastrado com sucesso!');

        this.contatoService.notificarMudanca();
        
        this.limparFormulario();
      },
      error: (erro) => {
        console.error('Erro ao enviar para a API:', erro);
        alert('Erro ao salvar o contato!');
        
        this.buttonText = 'Cadastrar';
        this.carregando = false;
      }
    });
  }

  limparFormulario(): void {
    this.novoContato = { id: -1, nome: '', telefone: '', email: '', cargo: '' };
    this.buttonText = 'Cadastrar';
    this.carregando = false;

    this.cdr.detectChanges();
  }
}
