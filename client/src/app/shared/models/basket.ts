import {v4 as uuidv4} from 'uuid';

export interface IBasket {
    id: string;
    items: IBasketItem[];
  }

export interface IBasketItem {
    id: number;
    productName: string;
    price: number;
    quantity: number;
    pictureUrl: string;
    brand: string;
    type: string;
  }

// tslint:disable-next-line: max-line-length
export class Basket implements IBasket{ // whenever we create a new instance of the Basket class, it's going to have a unique identifier and it's gonna have an empty array of items
    id = uuidv4();
    items: IBasketItem[] = [];

}

export interface IBasketTotals {
  shipping: number;
  subtotal: number;
  total: number;
}
