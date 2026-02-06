import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CamelService, Camel } from './camel';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
// ... importok ...

export class App implements OnInit {
  camels: Camel[] = [];
  newCamel: Camel = { name: '', color: '', humpCount: 1, lastFed: new Date().toISOString() };
  isEditing = false; // Flag a szerkesztéshez
  selectedId: number | null = null;

  constructor(private camelService: CamelService) { }

  ngOnInit() { this.refreshList(); }

  refreshList() { this.camelService.getCamels().subscribe(data => this.camels = data); }


  editCamel(camel: Camel) {
    this.isEditing = true;
    this.selectedId = camel.id!;
    this.newCamel = { ...camel };
  }

  deleteCamel(id: number) {
    if (confirm("Biztosan törlöd ezt a tevét?")) {
      this.camelService.deleteCamel(id).subscribe(() => this.refreshList());
    }
  }

  saveCamel() {
    if (this.isEditing && this.selectedId) {
      this.camelService.updateCamel(this.selectedId, this.newCamel).subscribe(() => {
        this.resetForm();
        this.refreshList();
      });
    } else {
      this.camelService.addCamel(this.newCamel).subscribe(() => {
        this.resetForm();
        this.refreshList();
      });
    }
  }

  resetForm() {
    this.isEditing = false;
    this.selectedId = null;
    this.newCamel = { name: '', color: '', humpCount: 1, lastFed: new Date().toISOString() };
  }
}