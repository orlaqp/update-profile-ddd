import { Resolver, Query, Args } from '@nestjs/graphql';
import { CustomerType } from './customer.model';
import { Int } from 'type-graphql';
import { CustomerService } from './customer.service';

@Resolver(of => CustomerType)
export class CustomerResolver {
  constructor(
    private readonly customerService: CustomerService,
  ) {}

  @Query(returns => [CustomerType])
  async customers() {
    return await this.customerService.getAll();
  }
}
