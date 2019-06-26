import { async, TestBed } from '@angular/core/testing';
import { ProfileFeatureModule } from './profile-feature.module';

describe('ProfileFeatureModule', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ProfileFeatureModule]
    }).compileComponents();
  }));

  it('should create', () => {
    expect(ProfileFeatureModule).toBeDefined();
  });
});
