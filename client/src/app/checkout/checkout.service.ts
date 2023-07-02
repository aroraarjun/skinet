import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/enivronment';
import { DeliveryMethod } from '../shared/Models/deliveryMethod';
import { map } from 'rxjs';
import { Order, OrderToCreate } from '../shared/Models/order';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  createOrder(order: OrderToCreate) {
    return this.http.post<Order>(this.baseUrl + 'orders', order);
  }

  getDeliveryMethods() {
    return this.http.get<DeliveryMethod[]>(this.baseUrl + 'orders/deliveryMethods').pipe(
      map(dm => {
        return dm.sort((a, b) => b.price - a.price)
      })
    )
  }
}
