import { Component } from '@angular/core';
import { PosteService } from '../../../services/poste.service';
import { CommonModule } from '@angular/common';
import { TableComponent } from '../../../layout/table/table.component';
import { SidebarComponent } from '../../../layout/sidebar/sidebar.component';

@Component({
  selector: 'app-sauvegarde',
  imports: [
    CommonModule,
    TableComponent,
    SidebarComponent
  ],
  templateUrl: './sauvegarde.component.html',
  styleUrl: './sauvegarde.component.scss'
})
export class SauvegardeComponent {

  postesEnregistres: any[] = []; // récupérés via le service

  constructor(private posteService: PosteService) { }

  ngOnInit() {
    this.loadPostesEnregistres();
}

loadPostesEnregistres() {
  this.posteService.getPosteLater().subscribe(data => {
    console.log(data);
    this.postesEnregistres = data;
  });
}

postuler(posteId: number) {
  // rediriger vers page de candidature ou ouvrir le formulaire
}

supprimer(posteId: number) {
  this.posteService.removePosteLater(posteId).subscribe(() => {
    this.postesEnregistres = this.postesEnregistres.filter(p => p.id !== posteId);
  });
}

}
