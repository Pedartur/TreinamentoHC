import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { Contato } from '../../models/contato';
import { CommonModule} from '@angular/common';
import { ContatoService } from '../../services/contato-service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { Popup } from '../popup/popup';
import { Subscription } from 'rxjs';

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
export class Lista implements OnInit, OnDestroy {
  todosContatos: Contato[] = [];
  contatosFiltrados: Contato[] = [];
  private inscricao!: Subscription;
  
  constructor(private readonly contatoService: ContatoService, private readonly cdr: ChangeDetectorRef, private readonly dialog: MatDialog) {}

  termoBusca: string = '';

  ngOnInit(): void {
    this.getAll();

    this.inscricao = this.contatoService.listaAtualizada$.subscribe(() => {
      this.getAll();
    });
  }

  ngOnDestroy(): void { 
    if (this.inscricao) {
      this.inscricao.unsubscribe();
    }
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

  abrirPopup(idContato: number): void {
    console.log("editando contato: " + idContato);

    const dialogRef = this.dialog.open(Popup, {
      width: '700px',   
      panelClass: 'popup-style',
      disableClose: false,    
      hasBackdrop: true,
      data: { id: idContato }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('O popup foi fechado!');
      
      this.getAll();
    });
  }
}
