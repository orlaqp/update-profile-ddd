## Demo Structure

The differents solutions in this repo work together to solve a single requirement, updating the email address
of a customer. It is not that simple though, this email address has to be updated in two different data stores that belong
to two completely different systems. In addition, the customer need to be notify of such change and both the notification and
the update of the email address should be tried up to three times in case of failures.

## How is our code organized?

- **ui**: This is a small project built with Angular. It has just a single page that allow a user to enter a new email address
that's it.

- **patron**: This project was built using .Net Core and it is basically the entry point to our backend functionality of changing
the email address of our customer.

- **cmp**: The secondary API that will take care of updating the email address in that secondary store.

- **notification**: A backend system which only responsiblity is to send notifications out.

## Solution architecture

![Architecture](/assets/architecture-diagram.png)


