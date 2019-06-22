import { Field, Int, ObjectType } from 'type-graphql';

@ObjectType()
export class CustomerType {
    @Field()
    id?: string;

    @Field({ nullable: true })
    firstName?: string;

    @Field({ nullable: true })
    lastName?: string;

    @Field({ nullable: true })
    email?: string;
}
