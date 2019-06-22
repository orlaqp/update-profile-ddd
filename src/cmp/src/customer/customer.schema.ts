import * as mongoose from 'mongoose';

export const CustomerSchema = new mongoose.Schema({
    firstName: String,
    lastName: String,
    email: String,
});

export interface Customer extends mongoose.Document {
    firstName: string;
    lastName: string;
    email: string;
}
