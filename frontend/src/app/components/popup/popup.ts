import { ChangeDetectorRef, Component, OnInit, Inject } from '@angular/core';
import { Contato } from '../../models/contato';
import { ContatoService} from '../../services/contato-service';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-popup',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule
  ],
  templateUrl: './popup.html',
  styleUrl: './popup.scss',
})
export class Popup implements OnInit {

  id: number = -1;
  
  constructor(private readonly contatoService: ContatoService, private readonly cdr: ChangeDetectorRef,
    public dialogRef: MatDialogRef<Popup>, @Inject(MAT_DIALOG_DATA) public data: { id: number }
  ) {
    this.id = data.id;
  }

  contato: Contato = {
    id: -1,
    nome: '',
    telefone: '',
    email: '',
    cargo: ''
  };

  ngOnInit(): void{
    this.contatoService.getById(this.id).subscribe({
      next: (resultado) => {
        this.contato = resultado;

        this.cdr.detectChanges();
      },
      error: (erro) => {
        console.error('Erro ao enviar para a API:', erro);
        alert('Erro ao achar o contato!');

        this.fecharPopup();
      }
    });
  }

  editarContato(): void {
    if (!this.contato.nome || !this.contato.telefone || !this.contato.email || !this.contato.cargo) {
      alert('Por favor, preencha todos os campos');
      return;
    }

    this.contatoService.update(this.id, this.contato).subscribe({
      next: (resultado) => {
        console.log('Salvo com sucesso!', resultado);
        alert('Contato editado com sucesso!');
        
        this.fecharPopup();
      },
      error: (erro) => {
        console.error('Erro ao enviar para a API:', erro);
        alert('Erro ao editar o contato!');
      }
    });
  }

  delete(): void{
    this.contatoService.delete(this.id).subscribe({
      next: (resultado) => {
        console.log('Deletado com sucesso!', resultado);
        alert('Contato deletado com sucesso!');
        
        this.fecharPopup();
      },
      error: (erro) => {
        console.error('Erro ao enviar para a API:', erro);
        alert('Erro ao deletar o contato!');
      }
    });
  }

  fecharPopup(): void{
    this.dialogRef.close();
  }
}
