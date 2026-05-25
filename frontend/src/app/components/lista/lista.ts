import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Contato } from '../../models/contato';
import { CommonModule} from '@angular/common';
import { ContatoService } from '../../services/contato-service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-lista',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatIconModule,
  ],
  templateUrl: './lista.html',
  styleUrls: ['./lista.scss'],
})
export class Lista implements OnInit {
  todosContatos: Contato[] = [];
  contatosFiltrados: Contato[] = [];
  
  constructor(private readonly contatoService: ContatoService, private readonly cdr: ChangeDetectorRef) {}

  termoBusca: string = '';

  ngOnInit(): void {
    this.getAll();
  }

  getAll(): void{
    this.contatoService.getAll().subscribe({
      next: (resultado) => {
        this.todosContatos = resultado;
        this.contatosFiltrados = this.todosContatos;
        this.termoBusca = '';

        this.cdr.detectChanges();
      },
      error: (erro) => {
        console.error('Erro ao buscar os contatos:', erro);
      }
    });
  }

  filtrarContatos(): void {
    if (!this.termoBusca.trim()) {
      this.contatosFiltrados = this.todosContatos;
      return;
    }

    const termo = this.termoBusca.toLowerCase();

    this.contatosFiltrados = this.todosContatos.filter(contato => {
      return contato.nome.toLowerCase().includes(termo) || 
             contato.cargo.toLowerCase().includes(termo) ||
             contato.email.toLowerCase().includes(termo);
    });

    this.cdr.detectChanges();
  }
}
