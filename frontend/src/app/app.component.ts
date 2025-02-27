import { Component } from '@angular/core';
import { ConnectionContainerComponent } from "./connection-container/connection-container.component";

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  imports: [ConnectionContainerComponent]
})
export class AppComponent {
}
