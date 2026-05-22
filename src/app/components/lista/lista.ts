import { Component } from '@angular/core';
import { Contact } from '../../models/contact';

@Component({
  selector: 'app-lista',
  standalone: false,
  templateUrl: './lista.html',
  styleUrl: './lista.scss',
})
export class Lista {
  contact: Contact[] = [
    {
      id: 1,
      nome: 'Pedro Freitas',
      email: 'pedro@example.com',
      telefone: '(31) 0000-1234',
      cargo: 'Software Engineer',
    },

    {
      id: 2,
      nome: 'Maria Silva',
      email: 'maria@example.com',
      telefone: '(31) 0000-5678',
      cargo: 'Product Manager',
    },
    {
      id: 3,
      nome: 'João Santos',
      email: 'joao@example.com',
      telefone: '(31) 0000-9012',
      cargo: 'Designer',
    },

    {
      id: 4,
      nome: 'Ana Oliveira',
      email: 'ana@example.com',
      telefone: '(31) 0000-3456',
      cargo: 'Marketing Specialist',
    },

    {
      id: 5,
      nome: 'Carlos Pereira',
      email: 'carlos@example.com',
      telefone: '(31) 0000-7890',
      cargo: 'Sales Associate',
    }
  ];
}
