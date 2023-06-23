import { HttpClient } from '@angular/common/http';
import { Component,OnInit } from '@angular/core';
import { Product } from './Models/product';
import { Pagination } from './Models/pagination';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'skinet';
  products: Product[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.http.get<Pagination<Product[]>>('http://localhost:5114/api/products?pageSize=50').subscribe({
      next: response => this.products = response.data, //what to do next when req completed
      error: error=> console.log(error), // what to when error
      complete: () => {
        console.log('req completed');
        console.log('extra statement');
      }
      //automatically unsubscribes
    })
  }
}
