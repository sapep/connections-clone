import { Component, Input, Output, EventEmitter } from '@angular/core';
import { UpperCasePipe, NgClass } from '@angular/common';
import { Word } from '../types';

@Component({
  selector: 'app-word-block',
  standalone: true,
  imports: [UpperCasePipe, NgClass],
  templateUrl: './word-block.component.html',
  styleUrl: './word-block.component.css'
})
export class WordBlockComponent {
  @Input({ required: true }) word!: Word;
  @Input({ required: true }) totalSelectedCount!: number;
  @Output() selected = new EventEmitter<Word>();

  isSelected: boolean = false;

  toggleSelected() {
    this.selected.emit(this.word);
    if (this.totalSelectedCount < 4 && !this.isSelected) {
      this.isSelected = !this.isSelected;
    } else if (this.isSelected) {
      this.isSelected = false;
    }
  }
}
