import { Component, OnInit } from '@angular/core';
import { IProduct } from 'src/app/shared/models/product';
import { ShopService } from '../shop.service';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  product: IProduct;
  quantity = 1;

  constructor(private shopService: ShopService, private activeRoute: ActivatedRoute,
              private bcService: BreadcrumbService, private basketService: BasketService) {
                                              // tslint:disable-next-line: max-line-length
    this.bcService.set('productDetails', '');   // we make use of the breadcrumb service and we'll just set it to an empty string in the constructor and then as the product is loaded this is going to replace it with the actual product name it will be empty but empty is better than what we currently see.
   }

  ngOnInit(): void {
    this.loadProduct();
  }

  // tslint:disable-next-line: typedef
  addItemToBasket(){
    this.basketService.addItemToBasket(this.product, this.quantity);
  }

  // tslint:disable-next-line: typedef
  incrementQuantity(){
    this.quantity++;
  }

  // tslint:disable-next-line: typedef
  decrementQuantity(){
    if (this.quantity > 1){
      this.quantity--;
    }
  }

  // tslint:disable-next-line: typedef
  loadProduct(){               // + this is for converting string of Url to number to be able to pass it to the method
    this.shopService.getProduct(+this.activeRoute.snapshot.paramMap.get('id')).subscribe(product => {
      this.product = product;
      this.bcService.set('@productDetails', product.name);
    }, error => {
      console.log(error);
    });
  }

}
