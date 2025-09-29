import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import {Nav} from "../layout/nav/nav";
import { AccountService } from '../core/services/account-service';


@Component({
  selector: 'app-root',
  imports: [Nav],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit{
  private AccountService = inject(AccountService);
  private http = inject(HttpClient);
  protected readonly title = 'Dating App';
  protected members = signal<any>([]);
  
    async ngOnInit(){
      this.members.set(await this.getMembers());
      this.setCurrentUser();
    }

    setCurrentUser(){
      const userString = localStorage.getItem('user');
      if (!userString) return;
      const user = JSON.parse(userString);
      this.AccountService.currentUser.set(user);
    }
  
  async getMembers(){
    try {
      return lastValueFrom(this.http.get('https://localhost:5001/Api/AppUser'));
    } catch (error) {
      console.log(error);
      throw error;
    }
    
  }
}
