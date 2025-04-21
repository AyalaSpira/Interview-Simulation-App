import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ManageUsersComponent } from './manage-users.component';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { By } from '@angular/platform-browser';

describe('ManageUsersComponent', () => {
  let component: ManageUsersComponent;
  let fixture: ComponentFixture<ManageUsersComponent>;
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, ManageUsersComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(ManageUsersComponent);
    component = fixture.componentInstance;
    httpMock = TestBed.inject(HttpTestingController);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load users on init', () => {
    const mockUsers = [{ id: 1, email: 'test@example.com' }];
    const req = httpMock.expectOne('https://interview-simulation-app-ar3c.onrender.com/api/users');
    expect(req.request.method).toBe('GET');
    req.flush(mockUsers);
    expect(component.users.length).toBe(1);
    expect(component.users[0].email).toBe('test@example.com');
  });

  it('should delete user and reload list', () => {
    component.users = [{ id: 1, email: 'a@example.com' }];
    component.deleteUser(1);

    const deleteReq = httpMock.expectOne('https://interview-simulation-app-ar3c.onrender.com/api/users/1');
    expect(deleteReq.request.method).toBe('DELETE');
    deleteReq.flush({});

    const getReq = httpMock.expectOne('https://interview-simulation-app-ar3c.onrender.com/api/users');
    expect(getReq.request.method).toBe('GET');
    getReq.flush([]);
  });

  afterEach(() => {
    httpMock.verify();
  });
});
