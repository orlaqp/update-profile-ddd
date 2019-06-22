import * as mongoose from 'mongoose';
import { DB_CONNECTION, CUSTOMER_MODEL } from 'src/constants';
import { CustomerSchema } from './customer.schema';

export const databaseProviders = [
    {
        provide: DB_CONNECTION,
        useFactory: (): Promise<typeof mongoose> =>
        mongoose.connect('mongodb://localhost/cmp'),
    },
    {
        provide: CUSTOMER_MODEL,
        useFactory: (connection: mongoose.Connection) => connection.model('Customer', CustomerSchema, 'customers'),
        inject: [DB_CONNECTION],
      },
];