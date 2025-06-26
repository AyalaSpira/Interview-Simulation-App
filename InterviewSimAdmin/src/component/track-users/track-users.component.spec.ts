import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrackUsersComponent } from './track-users.component';

describe('TrackUsersComponent', () => {
  let component: TrackUsersComponent;
  let fixture: ComponentFixture<TrackUsersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrackUsersComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TrackUsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
