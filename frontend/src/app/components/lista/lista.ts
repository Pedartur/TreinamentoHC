import { Component } from '@angular/core';
import { Contato } from '../../models/contato';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-lista',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './lista.html',
  styleUrl: './lista.scss',
})
export class Lista {
  contact: Contato[] = [
    {
      id: 1,
      nome: 'Pedro Freitas',
      email: 'pedro@example.com',
      telefone: '(31) 997773933',
      cargo: 'Software Engineer',
    },

    {
      id: 2,
      nome: 'Maria Silva',
      email: 'maria@example.com',
      telefone: '31997773933',
      cargo: 'Product Manager',
    },
    {
      id: 3,
      nome: 'João Santos',
      email: 'joao@example.com',
      telefone: '3197773933',
      cargo: 'Designer',
    },

    {
      id: 4,
      nome: 'Ana Oliveira',
      email: 'ana@example.com',
      telefone: '553197773933',
      cargo: 'Marketing Specialist',
    },

    {
      id: 5,
      nome: 'Carlos Pereira',
      email: 'carlos@example.com',
      telefone: '5531997773933',
      cargo: 'Sales Associate',
    }
  ];
}
