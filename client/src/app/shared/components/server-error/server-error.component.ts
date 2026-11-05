import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MatCard, MatCardContent } from '@angular/material/card';

@Component({
  selector: 'app-server-error',
  imports: [MatCard, MatCardContent],
  templateUrl: './server-error.component.html',
  styleUrl: './server-error.component.scss',
})
export class ServerErrorComponent {
  error?: any;

  constructor(private router: Router) {
    const navigation = this.router.currentNavigation();

    this.error = navigation?.extras.state?.["error"];
  }
}
