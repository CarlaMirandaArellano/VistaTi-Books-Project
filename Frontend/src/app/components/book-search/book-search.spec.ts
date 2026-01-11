import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BookSearchComponent } from './book-search.component';

describe('BookSearch', () => {
  let component: BookSearchComponent;
  let fixture: ComponentFixture<BookSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BookSearchComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BookSearchComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
