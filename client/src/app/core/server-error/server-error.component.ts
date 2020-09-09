import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.scss']
})
export class ServerErrorComponent implements OnInit {
  error: any;
              // tslint:disable-next-line: max-line-length
              // Now these navigation extras are only available in the constructor if we try and do this inside the ngOnInit(), then we will get undefined for this NavigationExtras
  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    this.error = navigation && navigation.extras && navigation.extras.state && navigation.extras.state.error;
  }

  ngOnInit(): void {
  }

}
