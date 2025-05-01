import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-decision-edit',
  imports: [],
  templateUrl: './decision-edit.component.html',
  styleUrl: './decision-edit.component.scss'
})
export class DecisionEditComponent {

  fileName: string | null = null;
  fileBase64: string | null = null;
  @Input() isOpen = false;
  @Input() title = '';
  @Output() closed = new EventEmitter<void>();
  @Output() confirmed = new EventEmitter<any>();
  @Input() confirmText: string = '';

  ngOnInit(): void {
    this.isOpen = true;
  }

  close() {
    this.closed.emit();
  }

  openModal() {
    this.isOpen = true;
  }

  closeModal() {
    this.isOpen = false;
  }

  onConfirm() {
    console.log('Action confirmée');
    this.isOpen = false;
  }
  
  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.fileName = file.name;
      const reader = new FileReader();
      
      reader.onload = (e: any) => {
        // Récupérer le contenu base64 sans la partie préfixe "data:[mime];base64,"
        // this.fileBase64 = e.target.result.split(',')[1];
        // console.log('File as base64:', this.fileBase64);

        const fileData = {
            Nom: this.fileName
            // Fichier: this.fileBase64,
        };
        console.log('File data in JSON format:', JSON.stringify(fileData));
        
        // Émettre les données de fichier (facultatif)
        this.confirmed.emit(JSON.stringify(fileData));
      };

      reader.readAsDataURL(file);  // Convertir le fichier en base64
      event.target.value = '';  // Réinitialiser le champ input
    }
  }
  
  confirm() {
    if (this.fileName) {
      console.log('File confirmed:', this.fileName);
      // Utilise l'objet JSON contenant les informations sur le fichier
      const fileData = {
        PieceJointe: {
          Nom: this.fileName
        }
    };
        // Envoie les données de fichier via l'EventEmitter ou à l'API
      this.confirmed.emit(JSON.stringify(fileData));
    } else {
      console.error('No file selected to confirm.');
    }
  }

}