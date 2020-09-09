import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopComponent } from './shop.component';
import { ProductItemComponent } from './product-item/product-item.component';
import {SharedModule} from '../shared/shared.module';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { ShopRoutingModule } from './shop-routing.module';


@NgModule({
  declarations: [ShopComponent, ProductItemComponent, ProductDetailsComponent],
  imports: [
    CommonModule,
    SharedModule,
    // tslint:disable-next-line: max-line-length
    ShopRoutingModule     // and click on shop then we can now see that the shop module is being loaded where we actually activate that particular route. So it's not loaded when we initially start the application it's only loaded after we activate the shop route itself and by clicking on this will then activate that particular module so nice.
]                        // so we only load things that we actually want to load because the route has been activated.
})
export class ShopModule { }
