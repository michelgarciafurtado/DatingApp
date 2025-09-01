import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';


@Component({
  selector: 'app-root',
  imports: [],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit{

  private http = inject(HttpClient);
  protected readonly title = 'Dating App';
  protected members: any;
  
    ngOnInit(): void {
    this.http.get('https://localhost:5001/Api/AppUser').subscribe({
      next: response => this.members = response,
      error: error => console.log,
      complete: () => console.log('Response has got')
    })
  }
}
