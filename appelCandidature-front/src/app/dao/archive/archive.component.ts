import { Component } from '@angular/core';
import { SidebarComponent } from '../../layout/sidebar/sidebar.component';
import { TableComponent } from '../../layout/table/table.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-archive',
  standalone: true,
  imports: [
    SidebarComponent,
    TableComponent,
    CommonModule
  ],
  templateUrl: './archive.component.html',
  styleUrls: ['./archive.component.scss']
})
export class ArchiveComponent {
  items = [
    {
      label: 'PVs Archivés',
      isOpen: false,
      pvs: [
        { titre: 'PV Commission RH Janvier', dateCreation: new Date('2024-01-15') },
        { titre: 'PV Réunion Budget', dateCreation: new Date('2023-12-10') }
      ]
    },
    {
      label: 'Décisions Archivées',
      isOpen: false,
      decisions: [
        { intitule: 'Nomination Responsable IT', dateSignature: new Date('2024-02-03') },
        { intitule: 'Lancement projet Alpha', dateSignature: new Date('2023-11-20') }
      ]
    }
  ];

  toggleDropdown(index: number): void {
    this.items[index].isOpen = !this.items[index].isOpen;
  }
}

