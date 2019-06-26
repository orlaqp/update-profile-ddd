import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ProfileComponent } from './containers/profile/profile.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatFormFieldModule, MatIconModule, MatInputModule, MatButtonModule, MatCardModule } from '@angular/material';

@NgModule({
  imports: [
    CommonModule,
    FlexLayoutModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,

    RouterModule.forChild([
      { path: '', pathMatch: 'full', component: ProfileComponent }
    ])
  ],
  declarations: [ProfileComponent]
})
export class ProfileFeatureModule {}
