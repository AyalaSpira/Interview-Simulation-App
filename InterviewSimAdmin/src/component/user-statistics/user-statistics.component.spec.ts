import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UserStatisticsComponent } from './user-statistics.component';
import { AuthService } from '../../services/auth.service';
import { of } from 'rxjs';
import { HttpClientModule } from '@angular/common/http';
import { CardModule } from 'primeng/card';
import { ChartModule } from 'primeng/chart';

describe('UserStatisticsComponent', () => {
  let component: UserStatisticsComponent;
  let fixture: ComponentFixture<UserStatisticsComponent>;
  let authService: jasmine.SpyObj<AuthService>;

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('AuthService', ['getUsers']);

    await TestBed.configureTestingModule({
      imports: [HttpClientModule, CardModule, ChartModule],
      declarations: [UserStatisticsComponent],
      providers: [{ provide: AuthService, useValue: spy }]
    })
    .compileComponents();

    authService = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UserStatisticsComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display chart data after loading users', () => {
    const mockUsers = [
      { email: 'user1@example.com', summary: { mark: 80 } },
      { email: 'user2@example.com', summary: { mark: 90 } }
    ];
    authService.getUsers.and.returnValue(of(mockUsers));
    fixture.detectChanges();
    expect(component.chartData).toBeTruthy();
    expect(component.chartData.labels.length).toBe(2);
    expect(component.chartData.datasets[0].data.length).toBe(2);
  });

  it('should handle empty users list gracefully', () => {
    authService.getUsers.and.returnValue(of([]));
    fixture.detectChanges();
    expect(component.chartData).toBeFalsy();
  });
});
