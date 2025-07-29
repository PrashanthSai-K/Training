import { Routes } from '@angular/router';
import { CategoryList } from './Category/category-list/category-list';
import { CategoryForm } from './Category/category-form/category-form';
import { CategoryDetails } from './Category/category-details/category-details';
import { ColorList } from './Color/color-list/color-list';
import { ColorCreate } from './Color/color-create/color-create';
import { ColorEdit } from './Color/color-edit/color-edit';
import { ColorDetails } from './Color/color-details/color-details';
import { ProductList } from './Product/product-list/product-list';
import { ProductDetail } from './Product/product-detail/product-detail';
import { ShoppingCart } from './shopping-cart/shopping-cart';
import { Checkout } from './checkout/checkout';
import { OrderSuccess } from './order-success/order-success';
import { OrderFailure } from './order-failure/order-failure';
import { OrderList } from './Order/order-list/order-list';
import { OrderDetail } from './Order/order-detail/order-detail';
import { Home } from './home/home';
import { NewsList as NewsManagementList } from './NewsManagement/news-list/news-list';
import { NewsList } from './News/news-list/news-list';
import { NewsForm } from './NewsManagement/news-form/news-form';
import { NewsDetail } from './NewsManagement/news-detail/news-detail';
import { ContactUs } from './contact-us/contact-us';
import { ProductCreate } from './Product/product-create/product-create';

export const routes: Routes = [
    { path: '', component: Home },
    { path: 'categories', component: CategoryList },
    { path: 'categories/create', component: CategoryForm },
    { path: 'categories/edit/:id', component: CategoryForm },
    { path: 'categories/details/:id', component: CategoryDetails },
    { path: 'colors', component: ColorList },
    { path: 'colors/create', component: ColorCreate },
    { path: 'colors/edit/:id', component: ColorEdit },
    { path: 'colors/details/:id', component: ColorDetails },
    { path: 'products', component: ProductList },
    { path: 'products/create', component: ProductCreate },
    { path: 'products/:id', component: ProductDetail },
    { path: 'cart', component: ShoppingCart },
    { path: 'checkout', component: Checkout },
    { path: 'order-success', component: OrderSuccess },
    { path: 'order-failure', component: OrderFailure },
    {
        path: 'orders',
        children: [
            { path: '', component: OrderList },
            { path: 'details/:id', component: OrderDetail }
        ]
    },
    { path: 'news', component: NewsList },
    { path: 'newslist', component: NewsManagementList },
    { path: 'news/create', component: NewsForm },
    { path: 'news/edit/:id', component: NewsForm },
    { path: 'news/:id', component: NewsDetail },
    { path: 'contactus', component: ContactUs },
];
