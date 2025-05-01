import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-decision-lance',
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './decision-lance.component.html',
  styleUrl: './decision-lance.component.scss'
})
export class DecisionLanceComponent {
  fileSelected: string = '';
  currentStep = 1;
  selectedFile: File | null = null;
  deadline: string = "";
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
        this.fileSelected = this.fileName ?? '';
        console.log('Selected file:', this.fileSelected);

        console.log('File data in JSON format:', JSON.stringify(fileData));
        
        // Émettre les données de fichier (facultatif)
        // this.confirmed.emit(JSON.stringify(fileData));
      };

      reader.readAsDataURL(file);  // Convertir le fichier en base64
      event.target.value = '';  // Réinitialiser le champ input
    }
  }
  

  confirm() {
    if (this.fileName) {
      const fileData = {
        PieceJointe: {
          Nom: this.fileName
        }
      };
  
      if (this.deadline) {
        const decisionData = {
            DateLimite: this.deadline
        };
  
        this.confirmed.emit({
          fileData: fileData,
          dateLimite: decisionData
        });
      } else {
        this.confirmed.emit({
          fileData: fileData,
          dateLimite: null 
        });
      }
    } else {
      console.error('Aucune pièce jointe sélectionnée.');
    }
  }
  

  nextStep() {
    if (this.fileName) {
      console.log('File confirmed:', this.fileName);
      const fileData = {
        PieceJointe: {
          Nom: this.fileName
        }
    };
        // Envoie les données de fichier via l'EventEmitter ou à l'API
      // this.confirmed.emit(JSON.stringify(fileData));
    } else {
      console.error('No file selected to confirm.');
    }
    // if (this.currentStep === 1 && !this.selectedFile) {
      
    //   alert('Veuillez ajouter une Décision signée.');
    //   return;
    // }
    this.currentStep++;
  }
  
  previousStep() {
    if (this.currentStep > 1) this.currentStep--;
  }
  
  
}