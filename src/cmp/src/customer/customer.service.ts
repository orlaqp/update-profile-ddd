import { Injectable, Inject } from '@nestjs/common';
import { CustomerType } from './customer.model';
import { CUSTOMER_MODEL } from 'src/constants';
import { Model } from 'mongoose';
import { Customer } from './customer.schema';

@Injectable()
export class CustomerService {

    constructor(@Inject(CUSTOMER_MODEL) private customerModel: Model<Customer>) {
    }

    getAll(): Promise<CustomerType[]> {
        return this.customerModel.find({}).exec();
    }

}
