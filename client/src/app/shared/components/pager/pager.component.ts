import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent implements OnInit {
  @Input() totalCount: number;
  @Input() pageSize: number;
  // tslint:disable-next-line: max-line-length
  @Output() pageChanged = new EventEmitter<number>(); // it will emit a number. an output property is a way that a child component our pager component is going to be a child component on the shop components page. And what we want to do is emit an output from our child component to our parent component and our shop component has the method in here to change the page and we still want to call this method but from our child component.

  constructor() { }

  ngOnInit(): void {
  }

  // tslint:disable-next-line: typedef
  onPagerChange(event: any){
    this.pageChanged.emit(event.page);
  }

}
