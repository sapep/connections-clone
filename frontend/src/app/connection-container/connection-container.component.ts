import { Component, computed, inject, OnInit } from '@angular/core';
import { ConnectionsService } from '../_services/connections.service';
import { WordBlockComponent } from "../word-block/word-block.component";
import { Word } from '../types';

@Component({
  selector: 'app-connection-container',
  standalone: true,
  imports: [WordBlockComponent],
  templateUrl: './connection-container.component.html',
  styleUrl: './connection-container.component.css'
})
export class ConnectionContainerComponent implements OnInit {
  connectionsService = inject(ConnectionsService);
  selectedWords: Word[] = [];

  shuffledWords = computed(() => {
    const words: Word[] = this.connectionsService
      .randomConnections()
      .flatMap(connection => connection.words.map(word => ({ ...word, connectionId: connection.id })));

    return this.shuffleArray(words);
  });

  ngOnInit(): void {
    this.connectionsService.getRandomConnections();
  }

  shuffleArray<T>(array: T[]): T[] {
    const arr = [...array];
    for (let i = arr.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      [arr[i], arr[j]] = [arr[j], arr[i]];
    }
    return arr;
  }

  updateSelectedWords(word: Word) {
    const wordIndex = this.selectedWords.findIndex(w => w.id === word.id);

    if (wordIndex !== -1) {
      this.selectedWords.splice(wordIndex, 1);
    } else if (this.selectedWords.length < 4) {
      this.selectedWords.push(word);

      // TODO: request to backend to verify the connection
      if (this.selectedWords.length === 4) {
        const allWordsHaveSameConnection = this.selectedWords
          .every(word => word.connectionId === this.selectedWords[0].connectionId);

        if (allWordsHaveSameConnection) {
          this.selectedWords.forEach(word => {
            const index = this.shuffledWords().findIndex(w => w.id === word.id);
            this.shuffledWords().splice(index, 1);
          });
          this.selectedWords = [];
        }
      }
    }
  }
}
