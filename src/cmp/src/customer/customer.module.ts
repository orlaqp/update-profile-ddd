import { Module } from '@nestjs/common';
import { GraphQLModule } from '@nestjs/graphql';
import { databaseProviders } from './database.providers';
import { CustomerService } from './customer.service';
import { CustomerResolver } from './customer.resolver';

@Module({
    imports: [
        GraphQLModule.forRoot({
            autoSchemaFile: 'schema.gql',
        }),
    ],
    providers: [...databaseProviders, CustomerService, CustomerResolver],
    exports: [...databaseProviders],
})
export class CustomerModule {}
